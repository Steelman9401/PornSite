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
        public async Task<GridViewDataSetLoadedData<VideoListDTO>> GetAllVideos(IGridViewDataSetLoadOptions gridViewDataSetLoadOptions, string search, bool loadmobile)
        {
            if (loadmobile)
            {
                if (string.IsNullOrEmpty(search))
                {
                    using (var db = new myDb())
                    {
                        db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
                        var query = await db.Videos.Where(p => p.Url.Contains("tuber")).Select(x => new VideoListDTO
                        {
                            Id = x.Id,
                            Img = x.Img,
                            Title = x.Title,
                            Preview = x.Preview,
                            Duration = x.Duration,
                            HD = x.HD
                        }).OrderByDescending(p => p.Id).ToListAsync();
                        return query.AsQueryable().GetDataFromQueryable(gridViewDataSetLoadOptions);
                    }
                }
                else
                {
                    using (var db = new myDb())
                    {
                        var query = await db.Videos
                            .Where(x => x.Url.Contains("tuber") && (x.Title.ToLower().Contains(search) || x.Categories.Select(p => p.Name.ToLower()).Contains(search)))
                            .Select(a => new VideoListDTO
                            {
                                Id = a.Id,
                                Img = a.Img,
                                Title = a.Title,
                                Preview = a.Preview,
                                Duration = a.Duration,
                                HD = a.HD
                            }).OrderByDescending(t => t.Id).ToListAsync();
                        return query.AsQueryable().GetDataFromQueryable(gridViewDataSetLoadOptions);
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(search))
                {
                    using (var db = new myDb())
                    {
                        db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
                        var query = await db.Videos.Select(x => new VideoListDTO
                        {
                            Id = x.Id,
                            Img = x.Img,
                            Title = x.Title,
                            Preview = x.Preview,
                            Duration = x.Duration,
                            HD = x.HD
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
                            .Select(a => new VideoListDTO
                            {
                                Id = a.Id,
                                Img = a.Img,
                                Title = a.Title,
                                Duration = a.Duration,
                                HD = a.HD,
                                Preview = a.Preview
                            }).OrderByDescending(t => t.Id).ToListAsync();
                        return query.AsQueryable().GetDataFromQueryable(gridViewDataSetLoadOptions);
                    }
                }
            }
        }
        public async Task<List<VideoListDTO>> GetRecommendedVideos()
        {
            if (HttpContext.Current.Request.Cookies["CategoryCount"] != null)
            {
                List<string> categories = HttpContext.Current.Request.Cookies["CategoryCount"]["array"]
                    .Split(':')
                    .Take(2)
                    .Select(x => x.ToString().Split(',')[0]).ToList();
                int first = Convert.ToInt32(categories[0]);
                int second = Convert.ToInt32(categories[1]);
                using (var db = new myDb())
                {
                    int count = db.Videos
                        .Where(x => x.Categories.Select(p => p.Id)
                        .Contains(first) && x.Categories
                        .Select(o => o.Id)
                        .Contains(second)).Count();
                    if (count > 3)
                    {
                        int skip = GetRandomNumber(count);
                        return await db.Videos
                            .Where(x => x.Categories
                            .Select(p => p.Id)
                            .Contains(first) && x.Categories
                            .Select(o => o.Id)
                            .Contains(second))
                            .Select(a => new VideoListDTO()
                            {
                                Id = a.Id,
                                Img = a.Img,
                                Title = a.Title,
                                Duration = a.Duration,
                                HD = a.HD,
                                Preview = a.Preview
                            }).OrderBy(s=>s.Id).Skip(skip).Take(4).ToListAsync();
                    }
                    else
                    {
                        count = db.Videos.Count();
                        int skip = GetRandomNumber(count);
                        return await db.Videos
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
                }
            }
            else
            {
                using (var db = new myDb())
                {
                    int count = db.Videos.Count();
                    int skip = GetRandomNumber(count);
                    return await db.Videos
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
            }
        }
        public async Task<GridViewDataSetLoadedData<VideoListDTO>>GetAllVideosByViews(IGridViewDataSetLoadOptions gridViewDataSetLoadOptions, string search, bool loadmobile)
        {
            if (loadmobile)
            {
                if (string.IsNullOrEmpty(search))
                {
                    using (var db = new myDb())
                    {
                        var query = await db.Videos
                            .Where(p=>p.Url.Contains("tuber"))
                            .Select(x => new VideoListDTO
                        {
                            Id = x.Id,
                            Img = x.Img,
                            Title = x.Title,
                            Duration = x.Duration,
                            HD = x.HD,
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
                            .Select(a => new VideoListDTO
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
            else
            {
                if (string.IsNullOrEmpty(search))
                {
                    using (var db = new myDb())
                    {
                        var query = await db.Videos.Select(x => new VideoListDTO
                        {
                            Id = x.Id,
                            Img = x.Img,
                            Title = x.Title,
                            Views = x.Views,
                            Duration = x.Duration,
                            HD = x.HD,
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
                            .Select(a => new VideoListDTO
                            {
                                Id = a.Id,
                                Img = a.Img,
                                Title = a.Title,
                                Views = a.Views,
                                Duration = a.Duration,
                                HD = a.HD,
                                Preview = a.Preview
                            }).OrderByDescending(t => t.Views).ToListAsync();
                        return query.AsQueryable().GetDataFromQueryable(gridViewDataSetLoadOptions);
                    }
                }
            }
        }
            public IEnumerable<VideoListDTO> GetVideosByCategory(int catId)
        {
            using (var db = new myDb())
            {
                return db.Categories.Find(catId).Videos.Select( x=> new VideoListDTO()
                {
                    Id = x.Id,
                    Img = x.Img,
                    Title = x.Title,
                    Duration = x.Duration,
                    HD = x.HD,
                    Preview = x.Preview
                }).OrderByDescending(a => a.Id).ToList();
                //return db.Videos
                //     .Where(p => p.Categories.Select(x => x.Id).ToList().Contains(catId)) //to list mozna neni potreba
                //     .Select(x => new VideoDTO()
                //     {
                //         Id = x.Id,
                //         Img = x.Img,
                //         Title = x.Title,
                //         Preview = x.Preview
                //     }).OrderByDescending(a => a.Id).ToList();
            }
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
                vid.Categories = video.Categories.Select(x=> new CategoryDTO()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
                UpdateCookie(vid.Categories);
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
                    return  await db.Videos.Where(x => x.Categories.Select(p => p.Id).Contains(a) || x.Categories.Select(p => p.Id).Contains(b) || x.Categories.Select(p=>p.Id).Contains(c)) //hnus
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
        public async Task<IEnumerable<string>> GetAllCategories()
        {
            using (var db = new myDb())
            {
                return await db.Categories.Select(x => x.Name).ToListAsync();
            }
        }
        private void BubbleSort(string cookie, ref string[] arr, int type)
        {
            if (!string.IsNullOrEmpty(cookie))
            {
                arr = cookie.Split(':');
            }
            for (int i = 0; i < arr.Length - 1; i++)
            {
                for (int j = 0; j < arr.Length - i - 1; j++)
                {
                    if (Convert.ToInt32(arr[j + 1].Split(',')[type]) > Convert.ToInt32(arr[j].Split(',')[type]))
                    {
                        string tmp = arr[j + 1];
                        arr[j + 1] = arr[j];
                        arr[j] = tmp;
                    }
                }
            }
        }
        private void UpdateCookieValue(ref string[] inputArray, int key)
        {
            int min = 0;
            bool found = false;
            int max = inputArray.Length - 1;
            while (min <= max)
            {
                int mid = (min + max) / 2;
                if (key == Convert.ToInt32(inputArray[mid].Split(',')[0]))
                {
                    var cat = inputArray[mid].Split(',')[0];
                    var count = Convert.ToInt32(inputArray[mid].Split(',')[1]);
                    count++;
                    inputArray[mid] = cat + "," + count;
                    found = true;
                    break;
                }
                else if (key > Convert.ToInt32(inputArray[mid].Split(',')[0]))
                {
                    max = mid - 1;
                }
                else
                {
                    min = mid + 1;
                }
            }
            if(!found)
            {
                Array.Resize(ref inputArray, inputArray.Length + 1);
                inputArray[inputArray.Length - 1] = key + "," + "1";

            }
        }
        private void UpdateCookie(IEnumerable<CategoryDTO> CategoryIds)
        {
            string cookie = null;
            if (HttpContext.Current.Request.Cookies["CategoryCount"] != null)
            {
                cookie = HttpContext.Current.Request.Cookies["CategoryCount"]["array"];
            }
            if (!string.IsNullOrEmpty(cookie))
            {
                string[] sorted = null;
                BubbleSort(cookie, ref sorted, 0);
                foreach (CategoryDTO item in CategoryIds)
                {
                    UpdateCookieValue(ref sorted, item.Id);
                }
                BubbleSort(string.Empty, ref sorted, 1);
                HttpContext.Current.Response.Cookies["CategoryCount"]["array"] = String.Join(":", sorted);
            }
            else
            {
                string newCookieString = null;
                foreach(CategoryDTO item in CategoryIds)
                {
                    newCookieString = newCookieString + item.Id + ",1:";
                }
                HttpCookie myCookie = new HttpCookie("CategoryCount");
                myCookie["array"] = newCookieString.Remove(newCookieString.Length - 1);
                myCookie.Expires = DateTime.Now.AddDays(36500d);
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
        }
        private int GetRandomNumber(int n)
        {
            Random rnd = new Random();
            return rnd.Next(0, n-4);
        }
    }
    }