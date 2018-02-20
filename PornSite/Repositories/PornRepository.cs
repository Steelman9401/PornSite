using DotVVM.Framework.Controls;
using PornSite.Data;
using PornSite.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Threading.Tasks;

namespace PornSite.Repositories
{
    public class PornRepository
    {
        public GridViewDataSetLoadedData<VideoDTO> GetAllVideos(IGridViewDataSetLoadOptions gridViewDataSetLoadOptions)
        {           
            using (var db = new myDb())
            {
                var query = db.Videos.Select(x => new VideoDTO
                {
                    Id = x.Id,
                    Img = x.Img,
                    Title = x.Title,
                    Views = x.Views,
                    Preview = x.Preview
                }).OrderByDescending(p=>p.Id).AsQueryable();
                return query.GetDataFromQueryable(gridViewDataSetLoadOptions);
            }
        }
        public GridViewDataSetLoadedData<VideoDTO> GetAllVideosAdmin(IGridViewDataSetLoadOptions gridViewDataSetLoadOptions)
        {
            using (var db = new myDb())
            {
                var query = db.Videos.Select(x => new VideoDTO
                {
                    Id = x.Id,
                    Img = x.Img,
                    Title = x.Title,
                    Views = x.Views,
                    Preview = x.Preview,
                    Description = x.Description
                }).OrderByDescending(p => p.Id).AsQueryable();
                return query.GetDataFromQueryable(gridViewDataSetLoadOptions);
            }
        }
        public GridViewDataSetLoadedData<VideoDTO> GetAllVideosByViews(IGridViewDataSetLoadOptions gridViewDataSetLoadOptions)
        {
            using (var db = new myDb())
            {
                var query = db.Videos.Select(x => new VideoDTO
                {
                    Id = x.Id,
                    Img = x.Img,
                    Title = x.Title,
                    Views = x.Views,
                    Preview = x.Preview
                }).OrderByDescending(p => p.Views).AsQueryable();
                return query.GetDataFromQueryable(gridViewDataSetLoadOptions);
            }
        }
            public IEnumerable<VideoDTO> GetVideosByCategory(int catId)
        {
            using (var db = new myDb())
            {
                return db.Videos
                     .Where(p => p.Categories.Select(x => x.Id).ToList().Contains(catId)) //to list mozna neni potreba
                     .Select(x => new VideoDTO()
                     {
                         Id = x.Id,
                         Img = x.Img,
                         Title = x.Title,
                         Preview = x.Preview
                     }).OrderByDescending(a => a.Id).ToList();
            }
        }
        public async Task UpdateViews(int Id)
        {
            using (var db = new myDb())
            {
                Video video = await db.Videos
                .Where(x => x.Id == Id).FirstOrDefaultAsync();
                video.Views++;
                await db.SaveChangesAsync();
            }
        }

        public VideoDTO GetVideoById(int Id)
        {

            using (var db = new myDb())
            {
                return db.Videos
                    .Where(x => x.Id == Id)
                    .Select(p => new VideoDTO
                    {
                        Id = p.Id,
                        Img = p.Img,
                        Title = p.Title,
                        Url = p.Url,
                        Description = p.Description,
                        Views = p.Views,
                        Categories = p.Categories
                        .Select(o => new CategoryDTO
                        {
                            Id = o.Id,
                            Name = o.Name
                        })
                    }).FirstOrDefault();
            }
        }
        public IEnumerable<VideoDTO> GetSuggestedVideos(List<int> Categories)
        {
            if (Categories.Count >= 3)
            {
                int a = Categories[0];
                int b = Categories[1];
                int c = Categories[2];
                using (var db = new myDb())
                {
                    return db.Videos.Where(x => x.Categories.Select(p => p.Id).Contains(a) || x.Categories.Select(p => p.Id).Contains(b) || x.Categories.Select(p=>p.Id).Contains(c)) //hnus
                          .Select(p => new VideoDTO
                          {
                              Id = p.Id,
                              Title = p.Title,
                              Img = p.Img
                          }).Take(8).ToList();
                }
            }
            else
            {
                List<VideoDTO> videos = new List<VideoDTO>();
                return videos;
            }
        }
        public async Task<IEnumerable<string>> GetUrls()
        {
            using (var db = new myDb())
            {
                return await db.Videos.OrderByDescending(p => p.Id)
                    .Select(x => x.Url).Take(100).ToListAsync();
            }
        }

        public async Task AddPorn(VideoDTO vid, IEnumerable<string> Categories)
        {
            Video video = new Video();
            video.Description = vid.Description;
            video.Title = vid.Title;
            video.Url = vid.Url;
            video.Img = vid.Img;
            video.Date = DateTime.Now;
            video.Preview = vid.Preview;
            PornRepository rep = new PornRepository();
            List<Category> listCat = new List<Category>();
            foreach (string item in Categories)
            {
                Category cat = new Category();
                cat.Name = item;
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
        public async Task<IEnumerable<string>> GetAllCategories()
        {
            using (var db = new myDb())
            {
                return await db.Categories.Select(x => x.Name).ToListAsync();
            }
        }
    }
    }