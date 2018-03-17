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
        public async Task<GridViewDataSetLoadedData<VideoDTO>> GetAllVideos(IGridViewDataSetLoadOptions gridViewDataSetLoadOptions, string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                using (var db = new myDb())
                {
                    db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
                    var query = await db.Videos.Select(x => new VideoDTO
                    {
                        Id = x.Id,
                        Img = x.Img,
                        Title = x.Title,
                        Views = x.Views,
                        Preview = x.Preview
                    }).OrderByDescending(p => p.Id).ToListAsync();
                    return query.AsQueryable().GetDataFromQueryable(gridViewDataSetLoadOptions);
                }
            }
            else
            {
                using (var db = new myDb())
                {
                    var query = await db.Videos
                        .Where(x => x.Title.ToLower().Contains(search) || x.Categories.Select(p => p.Name.ToLower()).Contains(search))
                        .Select(a => new VideoDTO
                        {
                            Id = a.Id,
                            Img = a.Img,
                            Title = a.Title,
                            Views = a.Views,
                            Preview = a.Preview
                        }).OrderByDescending(t => t.Id).ToListAsync();
                    return query.AsQueryable().GetDataFromQueryable(gridViewDataSetLoadOptions);
                }
            }
        }

        public async Task<GridViewDataSetLoadedData<VideoDTO>> GetSearchResult(IGridViewDataSetLoadOptions gridViewDataSetLoadOptions, string search)
        {
            using (var db = new myDb())
            {
                var query = await db.Videos
                    .Where(x => x.Title.ToLower().Contains(search) || x.Categories.Select(p => p.Name.ToLower()).Contains(search))
                    .Select(a => new VideoDTO
                    {
                        Id = a.Id,
                        Img = a.Img,
                        Title = a.Title,
                        Views = a.Views,
                        Preview = a.Preview
                    }).OrderByDescending(t => t.Id).ToListAsync();
                return query.AsQueryable().GetDataFromQueryable(gridViewDataSetLoadOptions);
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
        public async Task<GridViewDataSetLoadedData<VideoDTO>>GetAllVideosByViews(IGridViewDataSetLoadOptions gridViewDataSetLoadOptions, string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                using (var db = new myDb())
                {
                    var query = await db.Videos.Select(x => new VideoDTO
                    {
                        Id = x.Id,
                        Img = x.Img,
                        Title = x.Title,
                        Views = x.Views,
                        Preview = x.Preview
                    }).OrderByDescending(p => p.Views).ToListAsync();
                    return query.AsQueryable().GetDataFromQueryable(gridViewDataSetLoadOptions);
                }
            }
            else
            {
                using (var db = new myDb())
                {
                    var query = await db.Videos
                        .Where(x => x.Title.ToLower().Contains(search) || x.Categories.Select(p => p.Name.ToLower()).Contains(search))
                        .Select(a => new VideoDTO
                        {
                            Id = a.Id,
                            Img = a.Img,
                            Title = a.Title,
                            Views = a.Views,
                            Preview = a.Preview
                        }).OrderByDescending(t => t.Views).ToListAsync();
                    return query.AsQueryable().GetDataFromQueryable(gridViewDataSetLoadOptions);
                }
            }
        }
            public IEnumerable<VideoDTO> GetVideosByCategory(int catId)
        {
            using (var db = new myDb())
            {
                return db.Categories.Find(catId).Videos.Select( x=> new VideoDTO()
                {
                    Id = x.Id,
                    Img = x.Img,
                    Title = x.Title,
                    Preview = x.Preview
                }).OrderByDescending(a => a.Id).ToList();
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
        public async Task<VideoDTO> GetVideoById(int Id)
        {

            using (var db = new myDb())
            {
                db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
                Video video = await db.Videos.FindAsync(Id);
                video.Views++;
                await db.SaveChangesAsync();
                VideoDTO vid = new VideoDTO();
                vid.Id = video.Id;
                vid.Img = video.Img;
                vid.Title = video.Title;
                vid.Url = video.Url;
                vid.Views = video.Views;
                db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
                vid.Categories = video.Categories.Select(x=> new CategoryDTO()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
                return vid;
            }
        }
        public async Task<IEnumerable<VideoDTO>> GetSuggestedVideos(List<int> Categories)
        {
            if (Categories.Count >= 3)
            {
                int a = Categories[0];
                int b = Categories[1];
                int c = Categories[2];
                using (var db = new myDb())
                {
                    return  await db.Videos.Where(x => x.Categories.Select(p => p.Id).Contains(a) || x.Categories.Select(p => p.Id).Contains(b) || x.Categories.Select(p=>p.Id).Contains(c)) //hnus
                          .Select(p => new VideoDTO
                          {
                              Id = p.Id,
                              Title = p.Title,
                              Img = p.Img,
                              Preview = p.Preview
                          }).Take(8).ToListAsync();
                }
            }
            else
            {
                List<VideoDTO> videos = new List<VideoDTO>();
                return videos;
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
        public async Task AddPorn(VideoDTO vid)
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
        public async Task<IEnumerable<string>> GetAllCategories()
        {
            using (var db = new myDb())
            {
                return await db.Categories.Select(x => x.Name).ToListAsync();
            }
        }
        public void RemoveVideo(int Id)
        {
            using (var db = new myDb())
            {
                var video = db.Videos.Find(Id);
                db.Videos.Remove(video);
                db.SaveChanges();
            }
        }
    }
    }