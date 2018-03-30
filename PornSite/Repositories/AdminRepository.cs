using DotVVM.Framework.Controls;
using PornSite.Data;
using PornSite.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PornSite.Repositories
{
    public class AdminRepository
    {
        public async Task<GridViewDataSetLoadedData<VideoAdminDTO>> GetAllVideosAdmin(IGridViewDataSetLoadOptions gridViewDataSetLoadOptions)
        {
            using (var db = new myDb())
            {
                var query = await db.Videos.Select(x => new VideoAdminDTO
                {
                    Id = x.Id,
                    Img = x.Img,
                    Title = x.Title,
                    Preview = x.Preview,
                    Description = x.Description
                }).OrderByDescending(p => p.Id).ToListAsync();
                return query.AsQueryable().GetDataFromQueryable(gridViewDataSetLoadOptions);
            }
        }
        public async Task<IEnumerable<string>> GetImages()
        {
            using (var db = new myDb())
            {
                return await db.Videos.OrderByDescending(p => p.Id)
                    .Select(x => x.Img).Take(100).ToListAsync();
            }
        }
        public async Task<VideoAdminDTO> GetVideoById(int Id)
        {
            using (var db = new myDb())
            {
                return await db.Videos.
                    Where(x => x.Id == Id)
                    .Select(p => new VideoAdminDTO
                    {
                        Id = p.Id,
                        Description = p.Description,
                        Img = p.Img,
                        Preview = p.Preview,
                        Title = p.Title,
                        Url = p.Url,
                        Categories = p.Categories
                       .Select(a => new CategoryDTO
                       {
                           Id = a.Id,
                           Name = a.Name
                       })
                    }).FirstOrDefaultAsync();
            }
        }
        public async Task AddPorn(VideoAdminDTO vid)
        {
            Video video = new Video();
            video.Description = vid.Description;
            video.Title = vid.Title;
            video.Url = vid.Url;
            video.Img = vid.Img;
            video.Date = DateTime.Now;
            video.Preview = vid.Preview;
            video.HD = vid.HD;
            video.Duration = vid.Duration;
            PornRepository rep = new PornRepository();
            List<Category> listCat = new List<Category>();
            foreach (CategoryDTO item in vid.Categories)
            {
                Category cat = new Category();
                cat.Name = item.Name;
                listCat.Add(cat);
            }
            using (var db = new myDb())
            {
                foreach (Category item in listCat)
                {
                    Category tag = await db.Categories
                  .Where(x => x.Name == item.Name)
                  .FirstOrDefaultAsync();
                    if (tag == null)
                    {
                        video.Categories.Add(item);
                    }
                    else
                    {
                        video.Categories.Add(tag);
                    }
                }
                db.Videos.Add(video);
                await db.SaveChangesAsync();

            }
        }
        public async Task UpdateVideo(VideoAdminDTO video)
        {
            using (var db = new myDb())
            {
                var dbVideo = db.Videos.Find(video.Id);
                dbVideo.Title = video.Title;
                dbVideo.Description = video.Description;
                dbVideo.Img = video.Img;
                dbVideo.Preview = video.Preview;
                dbVideo.Url = video.Url;
                foreach(CategoryDTO item in video.Categories)
                {
                    var category = await db.Categories.FindAsync(item.Id);
                    if (category != null)
                    {
                        category.Name = item.Name;
                    }
                    else
                    {
                        Category dbCat = new Category();
                        dbCat.Name = item.Name;
                        dbVideo.Categories.Add(dbCat);
                    }
                }
                await db.SaveChangesAsync();
            }
        }
        public async Task RemoveVideo(int Id)
        {
            using (var db = new myDb())
            {
                var video = await db.Videos.FindAsync(Id);
                db.Comments.RemoveRange(video.Comments);
                db.Videos.Remove(video);
                await db.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<string>> GetCategories()
        {
            using (var db = new myDb())
            {
                return await db.Categories.Select(x => x.Name).ToListAsync();
            }
        }
    }

}