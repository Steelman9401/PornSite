using HtmlAgilityPack;
using PornSite.Data;
using PornSite.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace PornSite.Repositories.admin
{
    public class SpiderRepository:ValidationService
    {
        public IEnumerable<string> Images { get; set; }
        public void GetXhamsterVideos(List<VideoListAdminDTO> Videos, string category)
        {
            using (var client = new WebClient())
            {
                var html = "";
                client.Headers.Add("user-agent", "Mozilla / 5.0(Windows NT 10.0; WOW64; Trident / 7.0; rv: 11.0) like Gecko");
                if (string.IsNullOrEmpty(category))
                {
                    html = client.DownloadString("https://xhamster.com");
                }
                else
                {
                    try
                    {
                        html = client.DownloadString("https://xhamster.com/categories/" + category);
                    }
                    catch
                    {
                        html = client.DownloadString("https://xhamster.com/categories/" + category + "s");
                    }
                }
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                IEnumerable<HtmlNode> videoDivsDate;
                IEnumerable<HtmlNode> videoDivsProm = document.DocumentNode.SelectNodes("//div[@class='thumb-list__item video-thumb']").ToList();
                try
                {
                    videoDivsDate = document.DocumentNode.SelectNodes("//div[@class='thumb-list__item video-thumb video-thumb--dated']").ToList();
                }
                catch
                {
                    videoDivsDate = null;
                }
                IEnumerable<HtmlNode> videosConc;
                if (videoDivsDate != null)
                {
                    videosConc = videoDivsProm.Concat(videoDivsDate);
                }
                else
                {
                    videosConc = videoDivsProm;
                }
                Images = GetImages(category);
                int count = 0;
                foreach (HtmlNode item in videosConc)
                {
                    VideoListAdminDTO video = new VideoListAdminDTO();
                    var a = item.SelectSingleNode(".//a");
                    video.Img = a.ChildNodes[3].GetAttributeValue("src", string.Empty);
                    if (!ImageExists(video.Img, Images))
                    {
                        var hd = a.SelectSingleNode(".//i[@class='thumb-image-container__icon thumb-image-container__icon--hd']");
                        if (hd != null)
                        {
                            video.HD = true;
                        }
                        video.Preview = a.GetAttributeValue("data-previewvideo", string.Empty);
                        video.Url = a.GetAttributeValue("href", string.Empty);
                        video.Title_en = a.SelectSingleNode(".//img").GetAttributeValue("alt", string.Empty);
                        video.Duration = a.SelectSingleNode(".//div[@class='thumb-image-container__duration']").InnerText;
                        video.Id = count;
                        count = count + 1;
                        Videos.Add(video);
                    }
                }
            }

        }
        public void GetRedTubeVideos(List<VideoListAdminDTO> Videos, string category)
        {
            using (var client = new WebClient())
            {
                var html = "";
                client.Headers.Add("user-agent", "Mozilla / 5.0(Windows NT 10.0; WOW64; Trident / 7.0; rv: 11.0) like Gecko");
                if (string.IsNullOrWhiteSpace(category))
                {
                    html = client.DownloadString("https://redtube.com");
                }
                else
                {
                    if (category == "czech")
                    {
                        html = client.DownloadString("https://www.redtube.com/new?search=czech");
                    }
                    else if (category == "teen")
                    {
                        html = client.DownloadString("https://www.redtube.com/redtube/teens");
                    }
                    else if (category == "bdsm")
                    {
                        html = client.DownloadString("https://www.redtube.com/new?search=bdsm");
                    }
                    else
                    {
                        try
                        {
                            html = client.DownloadString("https://redtube.com/redtube/" + category);
                        }
                        catch
                        {
                            html = client.DownloadString("https://redtube.com/redtube/" + category + "s");
                        }
                    }
                }
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                var videoDivs = document.DocumentNode.SelectNodes("//div[@class='video_block_wrapper']").ToList();
                int count = 0;
                Images = GetImages(category);
                foreach (HtmlNode item in videoDivs)
                {
                    VideoListAdminDTO video = new VideoListAdminDTO();
                    var a = item.SelectSingleNode(".//a");
                    var img = a.SelectSingleNode(".//img");
                    video.Img = img.GetAttributeValue("data-thumb_url", string.Empty);
                    if (!ImageExists(video.Img, Images))
                    {
                        var duration = item.SelectSingleNode(".//span[@class='duration']").InnerText.Replace(" ", "").Replace("\n", "");
                        if (duration.Substring(0, 2) == "HD")
                        {
                            video.HD = true;
                            video.Duration = duration.Substring(2);
                        }
                        else
                        {
                            video.Duration = duration;
                        }
                        video.Id = count;
                        count = count + 1;
                        video.Preview = img.GetAttributeValue("data-mediabook", string.Empty);
                        video.Title_en = img.GetAttributeValue("alt", string.Empty);
                        video.Url = "https://redtube.com" + a.GetAttributeValue("href", string.Empty);
                        if (!string.IsNullOrWhiteSpace(video.Img) && !string.IsNullOrWhiteSpace(video.Preview))
                        {
                            Videos.Add(video);
                        }
                    }
                }

            }
        }
        public void GetPornHubVideos(List<VideoListAdminDTO> Videos, string category)
        {
            using (var client = new WebClient())
            {
                var html = "";
                client.Headers.Add("user-agent", "Mozilla / 5.0(Windows NT 10.0; WOW64; Trident / 7.0; rv: 11.0) like Gecko");
                if (string.IsNullOrWhiteSpace(category))
                {
                    html = client.DownloadString("https://www.pornhub.com/");
                }
                else
                {
                    html = client.DownloadString(ConvertToCategoryId(category));
                }
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                var wraps = document.DocumentNode.SelectNodes("//div[@class='phimage']").ToList();
                int count = 0;
                Images = GetImages(category);
                foreach (HtmlNode item in wraps)
                {
                    VideoListAdminDTO video = new VideoListAdminDTO();
                    var a = item.SelectSingleNode(".//a");
                    var img = a.SelectSingleNode(".//img");
                    try
                    {
                        var durationHDdiv = item.SelectSingleNode(".//var[@class='duration']").ParentNode;
                        if (durationHDdiv.SelectSingleNode(".//span[@class='hd-thumbnail']") != null)
                        {
                            video.HD = true;
                        }
                        video.Duration = durationHDdiv.SelectSingleNode(".//var[@class='duration']").InnerText;
                        video.Title_en = img.GetAttributeValue("alt", string.Empty);
                        video.Img = img.GetAttributeValue("data-mediumthumb", string.Empty);
                        if (string.IsNullOrEmpty(video.Img))
                        {
                            video.Img = img.GetAttributeValue("data-image", string.Empty);
                        }
                        if (!ImageExists(video.Img, Images))
                        {
                            video.Preview = img.GetAttributeValue("data-mediabook", string.Empty);
                            video.Url = "https://www.pornhub.com" + a.GetAttributeValue("href", string.Empty);
                            video.Id = count;
                            count = count + 1;
                            Videos.Add(video);
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }
        public void GetDrTuberVideos(List<VideoListAdminDTO> Videos, string category)
        {
            using (var client = new WebClient())
            {
                var html = "";
                client.Headers.Add("user-agent", "Mozilla / 5.0(Windows NT 10.0; WOW64; Trident / 7.0; rv: 11.0) like Gecko");
                try
                {
                    if (category == "ebony")
                    {
                        html = client.DownloadString("http://www.drtuber.com/black-and-ebony");
                    }
                    else
                    {
                        html = client.DownloadString("http://www.drtuber.com/" + category);
                    }
                }
                catch
                {
                    html = client.DownloadString("http://www.drtuber.com/" + category + "s");
                }
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                var videos = document.DocumentNode.SelectNodes("//a[@class='th ch-video']").ToList();
                Images = GetImages(category);
                int count = 0;
                foreach (HtmlNode item in videos)
                {
                    VideoListAdminDTO video = new VideoListAdminDTO();
                    var img = item.SelectSingleNode(".//img");
                    if (!ImageExists(img.GetAttributeValue("src", string.Empty), Images))
                    {
                        var toolbar = item.SelectSingleNode("./strong");
                        var hd = toolbar.SelectSingleNode(".//i[@class='quality']");
                        if (hd != null)
                        {
                            video.HD = true;
                        }
                        video.Duration = toolbar.SelectSingleNode(".//em[@class='time_thumb']").InnerText.Replace(" ", "");
                        video.Img = img.GetAttributeValue("src", string.Empty);
                        video.Preview = img.GetAttributeValue("data-webm", string.Empty);
                        video.Title_en = img.GetAttributeValue("alt", string.Empty);
                        video.Url = "http://www.drtuber.com" + item.GetAttributeValue("href", string.Empty);
                        video.Id = count;
                        count = count + 1;
                        Videos.Add(video);
                    }

                }
            }
        }
        public void GetCategoriesXhamster(VideoAdminDTO video)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("user-agent", "Mozilla / 5.0(Windows NT 10.0; WOW64; Trident / 7.0; rv: 11.0) like Gecko");
                var html = client.DownloadString(video.Url);
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                if (!video.Url.Contains("embed"))
                {
                    string[] array = video.Url.Split('-');
                    video.Url = "https://xhamster.com/xembed.php?video=" + array[array.Length - 1];
                }
                try
                {
                    video.Categories = document.DocumentNode.SelectNodes("//div[@class='entity-description-container__categories categories-container']")
                        .First()
                        .ChildNodes
                        .Where(x => x.Name == "a")
                        .Select(p => p.InnerText.Replace(" ", "").Replace("\n", "")).Select(o => new CategoryDTO()
                        {
                            Name_en = o
                        });
                }
                catch
                {
                    List<CategoryDTO> list = new List<CategoryDTO>();
                    video.Categories = list;
                }
            }
        }
        public void GetCategoriesRedTube(VideoAdminDTO video)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("user-agent", "Mozilla / 5.0(Windows NT 10.0; WOW64; Trident / 7.0; rv: 11.0) like Gecko");
                var html = client.DownloadString(video.Url);
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                if (!video.Url.Contains("embed"))
                {
                    var url = video.Url.Substring(20);
                    video.Url = "https://embed.redtube.com/?id=" + url;
                }

                var divs = document.DocumentNode.SelectNodes("//div[@class='video-infobox-content']");
                video.Categories = divs[divs.Count - 2].SelectNodes(".//a")
                    .Select(x => new CategoryDTO()
                    {
                        Name_en = x.InnerHtml,
                    });
            }
        }
        public void GetCategoriesPornHub(VideoAdminDTO video)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("user-agent", "Mozilla / 5.0(Windows NT 10.0; WOW64; Trident / 7.0; rv: 11.0) like Gecko");
                string oldUrl = video.Url;
                try
                {
                    var html = client.DownloadString(video.Url);
                    video.Url = "https://www.pornhub.com/embed/" + video.Url.Substring(47);
                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(html);
                    video.Categories = document.DocumentNode.SelectSingleNode("//div[@class='categoriesWrapper']").SelectNodes(".//a").Select(x => new CategoryDTO()
                    {
                        Name_en = x.InnerText
                    });
                    video.Categories = video.Categories.Reverse().Skip(1);
                }
                catch
                {
                    video.Url = "https://www.pornhub.com/embed/" + video.Url.Substring(47);
                    video.LoadError = true;
                }
            }
        }
        public void GetCategoriesDrTuber(VideoAdminDTO video)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("user-agent", "Mozilla / 5.0(Windows NT 10.0; WOW64; Trident / 7.0; rv: 11.0) like Gecko");
                var html = client.DownloadString(video.Url);
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                if (!video.Url.Contains("embed"))
                {
                    var array = video.Url.Split('/');
                    video.Url = "http://www.drtuber.com/embed/" + array[4];
                }
                try
                {
                    video.Categories = document.DocumentNode.SelectSingleNode("//div[@class='categories_list']")
                        .SelectNodes(".//a")
                        .Select(x => x.GetAttributeValue("title", string.Empty))
                        .Select(o => new CategoryDTO()
                        {
                            Name_en = o
                        });
                }
                catch
                {
                    List<CategoryDTO> list = new List<CategoryDTO>();
                    video.Categories = list;
                }
            }
        }
        private IEnumerable<string> GetImages(string category)
        {
            using (var db = new myDb())
            {
                //var query = db.Videos.Where(x=>x.Categories.Select(p=>p.Name).Contains(category));
                //var query = db.Videos.OrderByDescending(p => p.Id)
                //    .Select(x => x.Img).Take(50).ToList();
                IQueryable<Video> query = db.Videos;
                if (!string.IsNullOrEmpty(category))
                {
                    query = query.Where(x => x.Categories.Select(p => p.Name_en.ToLower()).Contains(category));
                }
                return query
                    .OrderByDescending(p => p.Id)
                    .Select(x => x.Img).Take(200).ToList();
            }
        }
        private string ConvertToCategoryId(string category)
        {
            if (category == "anal")
            {
                return "https://www.pornhub.com/video?c=" + "35";
            }
            else if (category == "amateur")
            {
                return "https://www.pornhub.com/video?c=" + "3";
            }
            else if (category == "asian")
            {
                return "https://www.pornhub.com/video?c=" + "1";
            }
            else if (category == "blonde")
            {
                return "https://www.pornhub.com/video?c=" + "9";
            }
            else if (category == "ebony")
            {
                return "https://www.pornhub.com/video?c=" + "17";
            }
            else if (category == "redhead")
            {
                return "https://www.pornhub.com/video?c=" + "42";
            }
            else if (category == "milf")
            {
                return "https://www.pornhub.com/video?c=" + "29";
            }
            else if (category == "gay")
            {
                return "https://www.pornhub.com/gayporn";
            }
            else if (category == "brunette")
            {
                return "https://www.pornhub.com/video?c=" + "11";
            }
            else if (category == "lesbian")
            {
                return "https://www.pornhub.com/video?c=" + "27";
            }
            else if (category == "czech")
            {
                return "https://www.pornhub.com/video?c=" + "100";
            }
            else if (category == "teen")
            {
                return "https://www.pornhub.com/categories/teen";
            }
            else if (category == "bdsm")
            {
                return "https://www.pornhub.com/video/search?search=bdsm&o=mr";
            }
            else if (category == "hentai")
            {
                return "https://www.pornhub.com/categories/hentai";
            }
            return null;
        }
    }
}