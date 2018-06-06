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
using System.Threading;

namespace PornSite.Repositories
{
    public class PornRepository
    {
        public async Task<int> GetSearchResultCount(string search, string culture)
        {
            using (var db = new myDb())
            {
                if (culture == "cs-CZ")
                {
                    return await db.Videos
                        .Where(x => x.Title
                        .Contains(search) || x.Categories
                        .Any(p => p.Name.Contains(search)) || x.Categories
                        .Any(o => o.Name_en.Contains(search))).CountAsync();
                }
                else
                {
                    return await db.Videos
                       .Where(x => x.Title_en
                       .Contains(search) || x.Categories
                       .Any(p => p.Name.Contains(search)) || x.Categories
                       .Any(o => o.Name_en.Contains(search))).CountAsync();
                }
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
        public GridViewDataSetLoadedData<VideoListDTO> GetSearchResult(IGridViewDataSetLoadOptions gridViewDataSetLoadOptions, string search, bool loadmomobile, int specific, string culture)
        {
            using (var db = new myDb())
            {
                IQueryable<Video> query = db.Videos;
                if (specific == 0) // nejnovejsi
                {
                    query = query.OrderByDescending(x => x.Id);
                    if (loadmomobile) // videa pouze pro mobil
                    {
                        query = query.Where(p => p.Url.Contains("pornhub"));
                    }
                }
                else // nejsledovanejsi
                {
                    query = query.OrderByDescending(x => x.Views);
                    if (loadmomobile) // nejsledovanejsi videa pouze pro mobil
                    {
                        query = query.Where(p => p.Url.Contains("pornhub"));
                    }
                }
                IQueryable<VideoListDTO> finalquery;
                if (culture == "cs-CZ")
                {
                    query = query.Where(x => x.Title
                    .Contains(search) || x.Categories
                    .Any(p => p.Name.Contains(search)) || x.Categories
                    .Any(o => o.Name_en.Contains(search)));
                    finalquery = query // finalni poskladana query
                            .Select(a => new VideoListDTO
                            {
                                Id = a.Id,
                                Img = "Admin/Previews/" + a.Img,
                                Title = a.Title,
                                Duration = a.Duration,
                                HD = a.HD,
                                Preview = "Admin/Previews/" + a.Preview,
                                Views = a.Views
                            }).AsQueryable();
                }
                else
                {
                    query = query.Where(x => x.Title_en
                    .Contains(search) || x.Categories
                    .Any(p => p.Name.Contains(search)) || x.Categories
                    .Any(o => o.Name_en.Contains(search)));
                    finalquery = query // finalni poskladana query
                            .Select(a => new VideoListDTO
                            {
                                Id = a.Id,
                                Img = "Admin/Previews/" + a.Img,
                                Title = a.Title_en,
                                Duration = a.Duration,
                                HD = a.HD,
                                Preview = "Admin/Previews/" + a.Preview,
                                Views = a.Views
                            }).AsQueryable();
                }
                return finalquery.GetDataFromQueryable(gridViewDataSetLoadOptions);
            }
        }
        public GridViewDataSetLoadedData<VideoListDTO> GetAllVideos(IGridViewDataSetLoadOptions gridViewDataSetLoadOptions, bool loadmobile, int specific, string culture)
        {
            using (var db = new myDb())
            {
                IQueryable<Video> query = db.Videos.Where(x => x.AllowMain == true);
                if (specific == 0) //nejnovejsi
                {
                    query = query.OrderByDescending(x => x.Id);
                    if (loadmobile) //videa pouze pro mobil
                    {
                        query = query.Where(p => p.Url.Contains("pornhub"));
                    }
                }
                else //nejsledovanejsi videa
                {
                    query = query.OrderByDescending(x => x.Views);
                    if (loadmobile) // nejsledovanejsi videa pouze pro mobil
                    {
                        query = query.Where(p => p.Url.Contains("pornhub"));
                    }
                }
                IQueryable<VideoListDTO> finalquery;
                if (culture == "cs-CZ")
                {
                    finalquery = query // finalni poskladana query
                       .Select(a => new VideoListDTO
                       {
                           Id = a.Id,
                           Img = "Admin/Previews/" + a.Img,
                           Title = a.Title,
                           Duration = a.Duration,
                           HD = a.HD,
                           Preview = "Admin/Previews/" + a.Preview,
                           Views = a.Views,
                           TimeStamp = a.TimeStamp
                       }).AsQueryable();
                }
                 else
                {
                    finalquery = query // finalni poskladana query
                      .Select(a => new VideoListDTO
                      {
                          Id = a.Id,
                          Img = "Admin/Previews/" + a.Img,
                          Title = a.Title_en,
                          Duration = a.Duration,
                          HD = a.HD,
                          Preview = "Admin/Previews/" + a.Preview,
                          Views = a.Views,
                          TimeStamp = a.TimeStamp
                      }).AsQueryable();
                }
                return finalquery.GetDataFromQueryable(gridViewDataSetLoadOptions);
            }
        }
        public GridViewDataSetLoadedData<VideoListDTO> GetVideosByCategory(IGridViewDataSetLoadOptions gridViewDataSetLoadOptions, int Id, int specific, bool loadmobile, string culture)
        {
            using (var db = new myDb())
            {
                IQueryable<Video> query = db.Videos.Where(x => x.Categories.Select(p => p.Id).Contains(Id));
                if (specific == 0) //nejnovejsi
                {
                    query = query.OrderByDescending(x => x.Id);
                    if (loadmobile) //videa pouze pro mobil
                    {
                        query = query.Where(p => p.Url.Contains("pornhub"));
                    }
                }
                else //nejsledovanejsi videa
                {
                    query = query.OrderByDescending(x => x.Views);
                    if (loadmobile) // nejsledovanejsi videa pouze pro mobil
                    {
                        query = query.Where(p => p.Url.Contains("pornhub"));
                    }
                }
                IQueryable<VideoListDTO> finalquery;
                if (culture == "cs-CZ")
                {
                    finalquery = query.Select(x => new VideoListDTO()
                    {
                        Id = x.Id,
                        Img = "Admin/Previews/" + x.Img,
                        Title = x.Title,
                        Duration = x.Duration,
                        HD = x.HD,
                        Preview = "Admin/Previews/" + x.Preview,
                    }).AsQueryable();
                    return finalquery.GetDataFromQueryable(gridViewDataSetLoadOptions);
                }
                else
                {
                    finalquery = query.Select(x => new VideoListDTO()
                    {
                        Id = x.Id,
                        Img = "Admin/Previews/" + x.Img,
                        Title = x.Title_en,
                        Duration = x.Duration,
                        HD = x.HD,
                        Preview = "Admin/Previews/" + x.Preview,
                    }).AsQueryable();
                    return finalquery.GetDataFromQueryable(gridViewDataSetLoadOptions);
                }
            }
        }
        public async Task<List<VideoListDTO>> GetVideoHistoryAsync(string culture)
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
                        if (culture == "cs-CZ")
                        {
                            return await db.Videos.AsExpandable()
                                .Where(predicate)
                                .Select(a => new VideoListDTO()
                                {
                                    Id = a.Id,
                                    Img = "Admin/Previews/" + a.Img,
                                    Title = a.Title,
                                    Duration = a.Duration,
                                    HD = a.HD,
                                    Preview = "Admin/Previews/" + a.Preview
                                }).ToListAsync();
                        }
                        else
                        {
                            return await db.Videos.AsExpandable()
                               .Where(predicate)
                               .Select(a => new VideoListDTO()
                               {
                                   Id = a.Id,
                                   Img = "Admin/Previews/" + a.Img,
                                   Title = a.Title_en,
                                   Duration = a.Duration,
                                   HD = a.HD,
                                   Preview = "Admin/Previews/" + a.Preview
                               }).ToListAsync();
                        }
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
        public async Task<IEnumerable<VideoListDTO>> GetRecommendedVideosAsync(string culture)
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
                            predicate = predicate.Or(x => x.Categories.Any(p => p.Id == Id));

                        }
                        int count = db.Videos.AsExpandable().Where(predicate).Count();
                        if (count > 3) //personalizovane vysledky
                        {
                            int skip = GetRandomNumber(count);
                            IQueryable<Video> query = db.Videos.AsExpandable().Where(predicate);
                            return await GetRecommendedVideosDataAsync(query, culture);
                        }
                        else //random videa
                        {
                            IQueryable<Video> query;
                            query = db.Videos.Where(x=>x.AllowMain);
                            return await GetRecommendedVideosDataAsync(query,culture);
                        }
                    }
                }
                catch
                {
                    using (var db = new myDb())
                    {
                        HttpContext.Current.Response.Cookies["CategoryCount"].Expires = DateTime.Now.AddDays(-1);
                        IQueryable<Video> query = db.Videos;
                        return await GetRecommendedVideosDataAsync(query,culture);
                    }
                }
            }
            else //random videa
            {
                using (var db = new myDb())
                {
                    IQueryable<Video> query;
                    query = db.Videos.Where(x=>x.AllowMain);
                    return await GetRecommendedVideosDataAsync(query,culture);
                }
            }
        }
        private async Task<List<VideoListDTO>> GetRecommendedVideosDataAsync(IQueryable<Video> query, string culture)
        {
            int count = query.Count();
            int skip = GetRandomNumber(count);
            if (culture == "cs-CZ")
            {
                return await query
                    .Select(a => new VideoListDTO()
                    {
                        Id = a.Id,
                        Img = "Admin/Previews/" + a.Img,
                        Title = a.Title,
                        Duration = a.Duration,
                        HD = a.HD,
                        Preview = "Admin/Previews/" + a.Preview
                    }).OrderBy(s => s.Id).Skip(skip).Take(4).ToListAsync();
            }
            else
            {
                return await query
                   .Select(a => new VideoListDTO()
                   {
                       Id = a.Id,
                       Img = "Admin/Previews/" + a.Img,
                       Title = a.Title_en,
                       Duration = a.Duration,
                       HD = a.HD,
                       Preview = "Admin/Previews/" + a.Preview
                   }).OrderBy(s => s.Id).Skip(skip).Take(4).ToListAsync();
            }
        }
        public async Task<VideoDetailDTO> GetVideoByIdAsync(int Id, string culture)
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
                if (culture == "cs-CZ")
                {
                    vid.Title = video.Title;
                    vid.Categories = video.Categories.Select(x => new CategoryDTO()
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList();
                }
                else
                {
                    vid.Title = video.Title_en;
                    vid.Categories = video.Categories.Select(x => new CategoryDTO()
                    {
                        Id = x.Id,
                        Name = x.Name_en
                    }).ToList();
                }
                vid.Url = video.Url;
                vid.Views = video.Views;
                db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
                CookieRepository CookieRep = new CookieRepository();
                CookieRep.UpdateCategoryCookie(vid.Categories);
                CookieRep.UpdateHistoryCookie(video.Id.ToString());
                return vid;
            }
        }
        public async Task<IEnumerable<VideoListDTO>> GetSuggestedVideosAsync(List<int> Categories, int Id, string culture)
        {
            List<string> CategoriesCounts = new List<string>();
            if (Categories.Count > 2)
            {
                var result = GetPermutations(Categories, 3);
                using (var db = new myDb())
                {
                    foreach (var perm in result)
                    {
                        List<int> Combination = new List<int>();
                        foreach (var item in perm)
                        {
                            Combination.Add(Convert.ToInt32(item));
                        }
                        int a = Combination[0];
                        int b = Combination[1];
                        int c = Combination[2];
                        int count = db.Videos.Where(x => x.Categories.Any(p => p.Id == a) && x.Categories.Any(o => o.Id == b) && x.Categories.Any(k => k.Id == c) && x.Id != Id).Count();
                        CategoriesCounts.Add(a + ";" + b + ";" + c + ":" + count);
                    }
                    List<int> finalResult = GetHighestCategoryCount(CategoriesCounts);
                    if (finalResult[3] > 0)
                    {
                        int first = finalResult[0];
                        int second = finalResult[1];
                        int third = finalResult[2];
                        if (culture == "cs-CZ")
                        {
                            return await db.Videos.Where(x => x.Categories.Any(p => p.Id == first) && x.Categories.Any(o => o.Id == second) && x.Categories.Any(k => k.Id == third) && x.Id != Id)
                                .Select(p => new VideoListDTO
                                {
                                    Id = p.Id,
                                    Title = p.Title,
                                    Img = "Admin/Previews/" + p.Img,
                                    Preview = "Admin/Previews/" + p.Preview,
                                    Duration = p.Duration,
                                    HD = p.HD
                                }).Take(6).ToListAsync();
                        }
                        else
                        {
                            return await db.Videos.Where(x => x.Categories.Any(p => p.Id == first) && x.Categories.Any(o => o.Id == second) && x.Categories.Any(k => k.Id == third) && x.Id != Id)
                                .Select(p => new VideoListDTO
                                {
                                    Id = p.Id,
                                    Title = p.Title_en,
                                    Img = "Admin/Previews/" + p.Img,
                                    Preview = "Admin/Previews/" + p.Preview,
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
            }
            else
            {
                using (var db = new myDb())
                {
                    IQueryable<Video> query = db.Videos;
                    foreach(int item in Categories)
                    {
                        query = query.Where(x => x.Categories.Any(p => p.Id == item));
                    }
                    return await query.Where(x=>x.Id!=Id)
                         .Select(p => new VideoListDTO
                         {
                             Id = p.Id,
                             Title = p.Title,
                             Img = "Admin/Previews/" + p.Img,
                             Preview = "Admin/Previews/" + p.Preview,
                             Duration = p.Duration,
                             HD = p.HD
                         }).Take(6).ToListAsync();
                }
            }
        }
        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync(string culture)
        {
            using (var db = new myDb())
            {
                if (culture == "cs-CZ")
                {
                    return await db.Categories
                        .Select(x => new CategoryDTO()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Img = "Admin/CategoryImg/" + x.Img
                        }).OrderBy(p => p.Name).ToListAsync();
                }
                else
                {
                    return await db.Categories
                        .Select(x => new CategoryDTO()
                        {
                            Id = x.Id,
                            Name = x.Name_en,
                            Img = "Admin/CategoryImg/" + x.Img
                        }).OrderBy(p => p.Name).ToListAsync();
                }
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
        static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> items, int count)
        {
            int i = 0;
            foreach (var item in items)
            {
                if (count == 1)
                    yield return new T[] { item };
                else
                {
                    foreach (var result in GetPermutations(items.Skip(i + 1), count - 1))
                        yield return new T[] { item }.Concat(result);
                }

                ++i;
            }
        }

        private List<int> GetHighestCategoryCount(List<string> CategoriesCount)
        {
            int index = 0;
            int loop = 0;
            int max = 0;
            foreach (string item in CategoriesCount)
            {
                int count = Convert.ToInt32(item.Split(':')[1]);
                if (count > max)
                {
                    index = loop;
                    max = count;
                }
                loop++;
            }
            string catArray = CategoriesCount[index].Split(':')[0];
            string[] categories = catArray.Split(';');
            List<int> result = new List<int>();
            result.Add(Convert.ToInt32(categories[0]));
            result.Add(Convert.ToInt32(categories[1]));
            result.Add(Convert.ToInt32(categories[2]));
            result.Add(max);
            return result;
        }

    }
}