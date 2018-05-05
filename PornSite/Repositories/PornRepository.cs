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
using System.Text;
using System.Globalization;

namespace PornSite.Repositories
{
    public class PornRepository
    {
        public async Task<int> GetSearchResultCount(string search)
        {
            using (var db = new myDb())
            {
                return await  db.Videos
                    .Where(x => x.Title
                    .Contains(search) || x.Categories
                    .Any(p => p.Name.Contains(search))|| x.Categories
                    .Any(o=>o.Name_en.Contains(search))).CountAsync();
            }
        }
        public async Task<int> GetCategoryVideosCount(int Id)
        {
            using (var db = new myDb())
            {
                return await db.Videos
                    .Where(x => x.Categories.Select(p => p.Id).Contains(Id))
                    .CountAsync();
            }
        }
        public async Task<GridViewDataSetLoadedData<VideoListDTO>> GetSearchResult(IGridViewDataSetLoadOptions gridViewDataSetLoadOptions, string search, bool loadmomobile, int specific)
        {
            using (var db = new myDb())
            {
                IQueryable<Video> query = db.Videos
                    .Where(x => x.Title
                    .Contains(search) || x.Categories
                    .Any(p => p.Name.Contains(search))|| x.Categories
                    .Any(o=>o.Name_en.Contains(search)));
                if (specific == 0) // nejnovejsi
                {
                    query = query.OrderByDescending(x => x.Id);
                    if (loadmomobile) // videa pouze pro mobil
                    {
                        query = query.Where(p => p.Url.Contains("tuber") || p.Url.Contains("pornhub"));
                    }
                }
                else // nejsledovanejsi
                {
                    query = query.OrderByDescending(x => x.Views);
                    if (loadmomobile) // nejsledovanejsi videa pouze pro mobil
                    {
                        query = query.Where(p => p.Url.Contains("tuber") || p.Url.Contains("pornhub"));
                    }
                }
                var finalquery = query // finalni poskladana query
                        .Select(a => new VideoListDTO
                        {
                            Id = a.Id,
                            Img = "/Admin/Previews/"+ a.Img,
                            Title = a.Title,
                            Duration = a.Duration,
                            HD = a.HD,
                            Preview = "/Admin/Previews/" + a.Preview,
                            Views = a.Views
                        }).AsQueryable();
                return finalquery.GetDataFromQueryable(gridViewDataSetLoadOptions);
            }
        }
        public async Task<GridViewDataSetLoadedData<VideoListDTO>> GetAllVideos(IGridViewDataSetLoadOptions gridViewDataSetLoadOptions, bool loadmobile, int specific)
        {
            using (var db = new myDb())
            {
                IQueryable<Video> query = db.Videos.Where(x=>x.AllowMain==true);
                if (specific == 0) //nejnovejsi
                {
                    query = query.OrderByDescending(x => x.Id);
                    if (loadmobile) //videa pouze pro mobil
                    {
                        query = query.Where(p => p.Url.Contains("tuber") || p.Url.Contains("pornhub"));
                    }
                }
                else //nejsledovanejsi videa
                {
                    query = query.OrderByDescending(x => x.Views);
                    if (loadmobile) // nejsledovanejsi videa pouze pro mobil
                    {
                        query = query.Where(p => p.Url.Contains("tuber") || p.Url.Contains("pornhub"));
                    }
                }
                DateTime currentDate = DateTime.Now;
                var finalquery = query // finalni poskladana query
                        .Select(a => new VideoListDTO
                        {
                            Id = a.Id,
                            Img = "/Admin/Previews/" + a.Img,
                            Title = a.Title,
                            Duration = a.Duration,
                            HD = a.HD,
                            Preview = "/Admin/Previews/" + a.Preview,
                            Views = a.Views,
                            TimeStamp = a.TimeStamp
            }).AsQueryable();
                return finalquery.GetDataFromQueryable(gridViewDataSetLoadOptions);
            }
        }
        public async Task<GridViewDataSetLoadedData<VideoListDTO>> GetVideosByCategory(IGridViewDataSetLoadOptions gridViewDataSetLoadOptions, int Id, int specific, bool loadmobile)
        {
            using (var db = new myDb())
            {
                IQueryable<Video> query = db.Videos.Where(x=>x.Categories.Select(p=>p.Id).Contains(Id));
                if (specific == 0) //nejnovejsi
                {
                    query = query.OrderByDescending(x => x.Id);
                    if (loadmobile) //videa pouze pro mobil
                    {
                        query = query.Where(p => p.Url.Contains("tuber") || p.Url.Contains("pornhub"));
                    }
                }
                else //nejsledovanejsi videa
                {
                    query = query.OrderByDescending(x => x.Views);
                    if (loadmobile) // nejsledovanejsi videa pouze pro mobil
                    {
                        query = query.Where(p => p.Url.Contains("tuber") || p.Url.Contains("pornhub"));
                    }
                }
                var finalquery = query.Select(x => new VideoListDTO()
                {
                    Id = x.Id,
                    Img = "/Admin/Previews/" + x.Img,
                    Title = x.Title,
                    Duration = x.Duration,
                    HD = x.HD,
                    Preview = "/Admin/Previews/" + x.Preview,
                }).AsQueryable();
                return finalquery.GetDataFromQueryable(gridViewDataSetLoadOptions);
            }
        }
        public async Task<List<VideoListDTO>> GetVideoHistory()
        {
            if (HttpContext.Current.Request.Cookies["History"] != null)
            {
                string[] array = HttpContext.Current.Request.Cookies["History"].Value.Split(',');
                if (array.Length < 5)
                {
                    using (var db = new myDb())
                    {
                        var predicate = PredicateBuilder.New<Video>();
                        foreach (string item in array)
                        {
                            try
                            {
                                int Id = Convert.ToInt32(item);
                                predicate = predicate.Or(x => x.Id == Id);
                            }
                            catch
                            {
                                HttpContext.Current.Response.Cookies["History"].Expires = DateTime.Now.AddDays(-1);
                                break;
                            }
                        }
                        return await db.Videos.AsExpandable()
                            .Where(predicate)
                            .Select(a => new VideoListDTO()
                            {
                                Id = a.Id,
                                Img = "/Admin/Previews/"+ a.Img,
                                Title = a.Title,
                                Duration = a.Duration,
                                HD = a.HD,
                                Preview = "/Admin/Previews/" + a.Preview
                            }).ToListAsync();
                    }
                }
                else
                {
                    HttpContext.Current.Response.Cookies["History"].Expires = DateTime.Now.AddDays(-1);
                    return new List<VideoListDTO>();
                }
            }
            else
            {
                return new List<VideoListDTO>();
            }
        }
        public async Task<List<VideoListDTO>> GetRecommendedVideos(bool loadmobile)
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
                            if(loadmobile)
                            predicate = predicate.Or(x => x.Categories.Select(p => p.Id).Contains(Id) && (x.Url.Contains("tuber") || x.Url.Contains("pornhub")));
                            else
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
                            IQueryable<Video> query;
                            if (loadmobile)
                                query = db.Videos.Where(x => x.Url.Contains("tuber") || x.Url.Contains("pornhub"));
                            else
                            {
                                query = db.Videos;
                            }
                            return await GetRecommendedVideosData(query);
                        }
                    }
                }
                catch
                {
                    using (var db = new myDb())
                    {
                        HttpContext.Current.Response.Cookies["CategoryCount"].Expires = DateTime.Now.AddDays(-1);
                        IQueryable<Video> query = db.Videos;
                        return await GetRecommendedVideosData(query);
                    }
                }
            }
            else //random videa
            {
                using (var db = new myDb())
                {
                    IQueryable<Video> query;
                    if (loadmobile)
                        query = db.Videos.Where(x => x.Url.Contains("tuber") || x.Url.Contains("pornhub"));
                    else
                    {
                        query = db.Videos;
                    }
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
                    Img = "/Admin/Previews/" + a.Img,
                    Title = a.Title,
                    Duration = a.Duration,
                    HD = a.HD,
                    Preview = "/Admin/Previews/"+ a.Preview
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
        public async Task<IEnumerable<VideoListDTO>> GetSuggestedVideos(List<int> Categories, int Id)
        {
            if (Categories.Count >= 3)
            {
                int a = Categories[0];
                int b = Categories[1];
                int c = Categories[2];
                using (var db = new myDb())
                {
                    return await db.Videos.Where(x => x.Id!= Id &&(x.Categories.Select(p => p.Id).Contains(a) || x.Categories.Select(p => p.Id).Contains(b) || x.Categories.Select(p => p.Id).Contains(c))) //hnus
                          .Select(p => new VideoListDTO
                          {
                              Id = p.Id,
                              Title = p.Title,
                              Img = "/Admin/Previews/" + p.Img,
                              Preview = "/Admin/Previews/" + p.Preview,
                              Duration = p.Duration,
                              HD = p.HD
                          }).Take(6).ToListAsync();
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
                        Name = x.Name,
                        Img = "/Admin/CategoryImg/" + x.Img
                    }).ToListAsync();
            }
        }
        private int GetRandomNumber(int n)
        {
            if (n > 3)
            {
                Random rnd = new Random();
                return rnd.Next(0, n - 4);
            }
            else
            {
                return 0;
            }
        }

    }
}