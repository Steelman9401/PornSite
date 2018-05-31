using DotVVM.Framework.Controls;
using PornSite.Data;
using PornSite.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace PornSite.Repositories.admin
{
    public class AdminRepository:ValidationService
    {
        public GridViewDataSetLoadedData<VideoListAdminDTO> GetAllVideos(IGridViewDataSetLoadOptions gridViewDataSetLoadOptions, string username)
        {
            using (var db = new myDb())
            {
                IQueryable<Video> query = db.Videos;
                if (username != "kelt" && username != "brych")
                {
                    query = query.Where(x => x.User.Username == username);
                }
                var finalQuery = query.Select(x => new VideoListAdminDTO()
                {
                    Id = x.Id,
                    Img = "Admin/Previews/" + x.Img,
                    Title = x.Title,
                    Preview = "Admin/Previews/" + x.Preview,
                }).OrderByDescending(p => p.Id).AsQueryable();
                return finalQuery.GetDataFromQueryable(gridViewDataSetLoadOptions);
            }
        }
        public GridViewDataSetLoadedData<CategoryDTO> GetAllCategories(IGridViewDataSetLoadOptions gridViewDataSetLoadOptions, string username)
        {
            using (var db = new myDb())
            {
                IQueryable<Category> query = db.Categories;
                if (username != "kelt" && username != "brych")
                {
                    query = query.Where(x => x.User.Username == username);
                }
                var finalQuery = query
                    .Select(p => new CategoryDTO()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Name_en = p.Name_en
                    }).OrderByDescending(a => a.Id).AsQueryable();
                return finalQuery.GetDataFromQueryable(gridViewDataSetLoadOptions);
            }
        }
        public async Task UpdateCategoryAsync(CategoryDTO category)
        {
            using (var db = new myDb())
            {
                var dbCat = await db.Categories.FindAsync(category.Id);
                dbCat.Name = category.Name;
                dbCat.Name_en = category.Name_en;
                await db.SaveChangesAsync();
            }
        }
        public async Task<VideoAdminDTO> GetVideoByIdAsync(int Id)
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
                        Title_en = p.Title_en,
                        Url = p.Url,
                        AllowMain = p.AllowMain,
                        Categories = p.Categories.Select(x => new CategoryDTO()
                        {
                            Name = x.Name,
                            Name_en = x.Name_en
                        })
                    }).FirstOrDefaultAsync();
            }
        }
        public async Task AddVideoAsync(VideoAdminDTO vid, string username)
        {
            var downloadPath = GetDownloadPath();
            var previewfileName = GetVideoFileName(vid.Preview);
            var imgfileName = GetVideoFileName(vid.Img);
            using (var client = new WebClient())
            {
                client.DownloadFile(vid.Preview, downloadPath + "/" + previewfileName);
                client.DownloadFile(vid.Img, downloadPath + "/" + imgfileName);
            }
            Video video = new Video();
            video.Description = vid.Description;
            video.Title = vid.Title;
            video.Title_en = vid.Title_en;
            video.Url = vid.Url;
            video.Img = imgfileName;
            video.TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            //var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            //string mp4Name = ChangeNameToMp4(previewfileName);
            //ffMpeg.ConvertMedia(downloadPath + "/" + previewfileName, downloadPath + "/" + "test.mp4", Format.mp4);
            //File.Delete(downloadPath + "/" + previewfileName);
            video.Preview = previewfileName;
            video.HD = vid.HD;
            video.Duration = vid.Duration;
            video.AllowMain = vid.AllowMain;
            using (var db = new myDb())
            {
                var user = db.Users.Where(x => x.Username == username).First();
                video.User = user;
                foreach (CategoryDTO item in vid.Categories)
                {
                    Category tag = await db.Categories
                   .Where(x => x.Name == item.Name)
                   .FirstAsync();
                    video.Categories.Add(tag);
                }
                db.Videos.Add(video);
                await db.SaveChangesAsync();

            }
        }
        public async Task UpdateVideoAsync(VideoAdminDTO video)
        {
            using (var db = new myDb())
            {
                var dbVideo = db.Videos.Find(video.Id);
                dbVideo.Title = video.Title;
                dbVideo.Description = video.Description;
                dbVideo.Img = video.Img;
                dbVideo.Preview = video.Preview;
                dbVideo.Url = video.Url;
                dbVideo.AllowMain = video.AllowMain;
                dbVideo.Categories.Clear();
                foreach (CategoryDTO item in video.Categories)
                {
                    var category = await db.Categories.Where(x => x.Name == item.Name).FirstAsync();
                    dbVideo.Categories.Add(category);
                }
                await db.SaveChangesAsync();
            }
        }
        public async Task RemoveVideoAsync(int Id)
        {
            using (var db = new myDb())
            {
                var video = await db.Videos.FindAsync(Id);
                db.Comments.RemoveRange(video.Comments);
                //string downloadPath = GetDownloadPath();
                //File.Delete(downloadPath + "/" + video.Img);
                //File.Delete(downloadPath + "/" + video.Preview);
                db.Videos.Remove(video);
                await db.SaveChangesAsync();
            }
        }
        public async Task AddCategoryAsync(CategoryDTO category, string username)
        {
            using (var db = new myDb())
            {
                var user = db.Users
                    .Where(x => x.Username == username)
                    .First();
                Category dbCat = new Category();
                dbCat.Name = category.Name;
                dbCat.Name_en = category.Name_en;
                dbCat.User = user;
                dbCat.Img = "placeholder.jpg";
                db.Categories.Add(dbCat);
                await db.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<string>> GetCategoriesAsync()
        {
            using (var db = new myDb())
            {
                return await db.Categories.OrderBy(p => p.Name)
                    .Select(x => x.Name).ToListAsync();
            }
        }
        public async Task RemoveCategoryAsync(int Id)
        {
            using (var db = new myDb())
            {
                var category = await db.Categories.FindAsync(Id);
                db.Categories.Remove(category);
                await db.SaveChangesAsync();
            }
        }

        private string GetVideoFileName(string url)
        {
            string[] array = url.Split('/');
            string trimmedUrl = "";
            for (int i = 1; i <= 4; i++)
            {
                trimmedUrl = trimmedUrl.Insert(0, array[array.Length - i]);
            }
            if (trimmedUrl.Contains('?'))
            {
                trimmedUrl = trimmedUrl.Substring(0, trimmedUrl.IndexOf("?"));
            }
            return trimmedUrl;
        }
        private string GetDownloadPath()
        {
            var virtualPath = Path.Combine(HttpContext.Current.Request.ApplicationPath, "Admin/Previews");
            var finalPath = HttpContext.Current.Request.MapPath(virtualPath);
            if (!Directory.Exists(finalPath))
            {
                Directory.CreateDirectory(finalPath);
            }
            return finalPath;
        }

        private string ChangeNameToMp4(string file)
        {
            int index = file.IndexOf('.');
            if (!file.Contains(".mp4"))
            {
                file = file.Substring(0, index) + ".mp4";
            }
            return file;
        }
    }
}