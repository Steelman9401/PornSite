using DotVVM.Framework.Controls;
using PornSite.Data;
using PornSite.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Threading.Tasks;
using LinqKit;

namespace PornSite.Repositories
{
    public class PornRepository
    {
        public async Task<int> GetSearchResultCount(string search)
        {
            using (var db = new myDb())
            {
                return await db.Videos
               .Where(x => x.Title.ToLower()
               .Contains(search) || x.Categories
               .Select(p => p.Name.ToLower()).Contains(search)).CountAsync();
            }
        }
        public async Task<GridViewDataSetLoadedData<VideoListDTO>> GetSearchResult(IGridViewDataSetLoadOptions gridViewDataSetLoadOptions, string search, bool loadmomobile, int specific)
        {
            using (var db = new myDb())
            {
                IQueryable<Video> query = db.Videos
                    .Where(x => x.Title.ToLower()
                    .Contains(search) || x.Categories
                    .Select(p => p.Name.ToLower()).Contains(search));
                if (specific == 0) // nejnovejsi
                {
                    query = query.OrderByDescending(x => x.Id);
                    if (loadmomobile) // videa pouze pro mobil
                    {
                        query = query.Where(p => p.Url.Contains("tuber"));
                    }
                }
                else // nejsledovanejsi
                {
                    query = query.OrderByDescending(x => x.Views);
                    if (loadmomobile) // nejsledovanejsi videa pouze pro mobil
                    {
                        query = query.Where(p => p.Url.Contains("tuber"));
                    }
                }
                var finalquery = query // finalni poskladana query
                        .Select(a => new VideoListDTO
                        {
                            Id = a.Id,
                            Img = a.Img,
                            Title = a.Title,
                            Duration = a.Duration,
                            HD = a.HD,
                            Preview = a.Preview,
                            Views = a.Views
                        }).AsQueryable();
                return finalquery.GetDataFromQueryable(gridViewDataSetLoadOptions);
            }
        }
        public async Task<GridViewDataSetLoadedData<VideoListDTO>> GetAllVideos(IGridViewDataSetLoadOptions gridViewDataSetLoadOptions, bool loadmobile, int specific)
        {
            using (var db = new myDb())
            {
                IQueryable<Video> query = db.Videos;
                if (specific == 0) //nejnovejsi
                {
                    query = query.OrderByDescending(x => x.Id);
                    if (loadmobile) //videa pouze pro mobil
                    {
                        query = query.Where(p => p.Url.Contains("tuber"));
                    }
                }
                else //nejsledovanejsi videa
                {
                    query = query.OrderByDescending(x => x.Views);
                    if (loadmobile) // nejsledovanejsi videa pouze pro mobil
                    {
                        query = query.Where(p => p.Url.Contains("tuber"));
                    }
                }
                var finalquery = query // finalni poskladana query
                        .Select(a => new VideoListDTO
                        {
                            Id = a.Id,
                            Img = a.Img,
                            Title = a.Title,
                            Duration = a.Duration,
                            HD = a.HD,
                            Preview = a.Preview,
                            Views = a.Views
                        }).AsQueryable();
                return finalquery.GetDataFromQueryable(gridViewDataSetLoadOptions);
            }
        }
        public async Task<GridViewDataSetLoadedData<VideoListDTO>> GetVideosByCategory(IGridViewDataSetLoadOptions gridViewDataSetLoadOptions, int Id)
        {
            using (var db = new myDb())
            {
                var query = db.Categories.Find(Id).Videos.Select(x => new VideoListDTO()
                {
                    Id = x.Id,
                    Img = x.Img,
                    Title = x.Title,
                    Duration = x.Duration,
                    HD = x.HD,
                    Preview = x.Preview
                }).OrderByDescending(a => a.Id).AsQueryable();
                return query.GetDataFromQueryable(gridViewDataSetLoadOptions);
            }
        }
        public async Task<string> GetCategoryNameById(int Id)
        {
            using (var db = new myDb())
            {
                return await db.Categories.Where(p => p.Id == Id).Select(x => x.Name).FirstOrDefaultAsync();
            }
        }
        public async Task<List<VideoListDTO>> GetVideoHistory()
        {
            if (HttpContext.Current.Request.Cookies["History"] != null)
            {
                string[] array = HttpContext.Current.Request.Cookies["History"].Value.Split(',');
                using (var db = new myDb())
                {
                    var predicate = PredicateBuilder.New<Video>();
                    foreach (string item in array)
                    {
                        int Id = Convert.ToInt32(item);
                        predicate = predicate.Or(x => x.Id == Id);
                    }
                    return await db.Videos.AsExpandable()
                        .Where(predicate)
                        .Select(a => new VideoListDTO()
                        {
                            Id = a.Id,
                            Img = a.Img,
                            Title = a.Title,
                            Duration = a.Duration,
                            HD = a.HD,
                            Preview = a.Preview
                        }).ToListAsync();
                }
            }
            else
            {
                return new List<VideoListDTO>();
            }
        }
        public async Task<List<VideoListDTO>> GetRecommendedVideos()
        {
            if (HttpContext.Current.Request.Cookies["CategoryCount"] != null)
            {
                List<string> categories = HttpContext.Current.Request.Cookies["CategoryCount"].Value
                    .Split(':')
                    .Take(2)
                    .Select(x => x.ToString().Split(',')[0]).ToList();
                try
                {
                    using (var db = new myDb())
                    {
                        var predicate = PredicateBuilder.New<Video>();
                        foreach (string item in categories)
                        {
                            int Id = Convert.ToInt32(item);
                            predicate = predicate.Or(x => x.Categories.Select(p => p.Id).Contains(Id));
                        }
                        int count = db.Videos.AsExpandable().Where(predicate).Count();
                        if (count > 3) //personalizovane vysledky
                        {
                            int skip = GetRandomNumber(count);
                            IQueryable<Video> query = db.Videos.AsExpandable().Where(predicate);
                            return await GetRecommendedVideosData(query);
                        }
                        else //random videa
                        {
                            IQueryable<Video> query = db.Videos;
                            return await GetRecommendedVideosData(query);
                        }
                    }
                }
                catch
                {
                    using (var db = new myDb())
                    {
                        HttpContext.Current.Request.Cookies["CategoryCount"].Expires = DateTime.Now.AddDays(-1);
                        IQueryable<Video> query = db.Videos;
                        return await GetRecommendedVideosData(query);
                    }
                }
            }
            else //random videa
            {
                using (var db = new myDb())
                {
                    IQueryable<Video> query = db.Videos;
                    return await GetRecommendedVideosData(query);
                }
            }
        }
        private async Task<List<VideoListDTO>> GetRecommendedVideosData(IQueryable<Video> query)
        {
            int count = query.Count();
            int skip = GetRandomNumber(count);
            return await query
                .Select(a => new VideoListDTO()
                {
                    Id = a.Id,
                    Img = a.Img,
                    Title = a.Title,
                    Duration = a.Duration,
                    HD = a.HD,
                    Preview = a.Preview
                }).OrderBy(s => s.Id).Skip(skip).Take(4).ToListAsync();
        }
        public async Task<VideoDetailDTO> GetVideoById(int Id)
        {

            using (var db = new myDb())
            {
                db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
                Video video = await db.Videos.FindAsync(Id);
                video.Views++;
                await db.SaveChangesAsync();
                VideoDetailDTO vid = new VideoDetailDTO();
                vid.Id = video.Id;
                vid.Img = video.Img;
                vid.Title = video.Title;
                vid.Url = video.Url;
                vid.Views = video.Views;
                db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
                vid.Categories = video.Categories.Select(x => new CategoryDTO()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
                CookieRepository CookieRep = new CookieRepository();
                CookieRep.UpdateCategoryCookie(vid.Categories);
                CookieRep.UpdateHistoryCookie(video.Id.ToString());
                return vid;
            }
        }
        public async Task<IEnumerable<VideoListDTO>> GetSuggestedVideos(List<int> Categories)
        {
            if (Categories.Count >= 3)
            {
                int a = Categories[0];
                int b = Categories[1];
                int c = Categories[2];
                using (var db = new myDb())
                {
                    return await db.Videos.Where(x => x.Categories.Select(p => p.Id).Contains(a) || x.Categories.Select(p => p.Id).Contains(b) || x.Categories.Select(p => p.Id).Contains(c)) //hnus
                          .Select(p => new VideoListDTO
                          {
                              Id = p.Id,
                              Title = p.Title,
                              Img = p.Img,
                              Preview = p.Preview,
                              Duration = p.Duration,
                              HD = p.HD
                          }).Take(8).ToListAsync();
                }
            }
            else
            {
                List<VideoListDTO> videos = new List<VideoListDTO>();
                return videos;
            }
        }
        public async Task<IEnumerable<CategoryDTO>> GetAllCategories()
        {
            using (var db = new myDb())
            {
                return await db.Categories
                    .Select(x => new CategoryDTO()
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToListAsync();
            }
        }
        private int GetRandomNumber(int n)
        {
            Random rnd = new Random();
            return rnd.Next(0, n - 4);
        }
    }
}