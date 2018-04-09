using HtmlAgilityPack;
using PornSite.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Threading.Tasks;
using PornSite.Data;
using System.Data.Entity;

namespace PornSite.Repositories
{
    public class ScrapperRepository
    {
        public IEnumerable<string> Images { get; set; }
        public async Task GetXhamsterVideos(List<VideoListAdminDTO> Videos, string category)
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
                foreach (HtmlNode item in videosConc)
                {
                    VideoListAdminDTO video = new VideoListAdminDTO();
                    var a = item.SelectSingleNode(".//a");
                    video.Img = a.ChildNodes[3].GetAttributeValue("src", string.Empty);
                    Images = GetImages();
                    if (!ImageExists(video.Img, Images))
                    {
                        var hd = a.SelectSingleNode(".//i[@class='thumb-image-container__icon thumb-image-container__icon--hd']");
                        if(hd!=null)
                        {
                            video.HD = true;
                        }
                        video.Preview = a.GetAttributeValue("data-previewvideo", string.Empty);
                        video.Url = a.GetAttributeValue("href", string.Empty);
                        video.Title = a.SelectSingleNode(".//img").GetAttributeValue("alt", string.Empty);
                        video.Duration = a.SelectSingleNode(".//div[@class='thumb-image-container__duration']").InnerText;
                        Videos.Add(video);
                    }
                }
            }

        }
        public async Task GetRedTubeVideos(List<VideoListAdminDTO> Videos, string category)
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
                    try
                    {
                        html = client.DownloadString("https://redtube.com/redtube/" + category);
                    }
                    catch
                    {
                        html = client.DownloadString("https://redtube.com/redtube/" + category + "s");
                    }
                }
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                var videoDivs = document.DocumentNode.SelectNodes("//div[@class='video_block_wrapper']").ToList();
                foreach (HtmlNode item in videoDivs)
                {
                    VideoListAdminDTO video = new VideoListAdminDTO();
                    var a = item.SelectSingleNode(".//a");
                    var img = a.SelectSingleNode(".//img");
                    video.Img = img.GetAttributeValue("data-thumb_url", string.Empty);
                    Images = GetImages();
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
                foreach (HtmlNode item in videos)
                {
                    VideoListAdminDTO video = new VideoListAdminDTO();
                    var img = item.SelectSingleNode(".//img");
                    Images = GetImages();
                    if (!ImageExists(img.GetAttributeValue("src", string.Empty),Images))
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
                        video.Title = img.GetAttributeValue("alt", string.Empty);
                        video.Url = "http://www.drtuber.com" + item.GetAttributeValue("href", string.Empty);
                        Videos.Add(video);
                    }

                }
            }
        }
        private bool ImageExists(string image, IEnumerable<string> Images)
        {
            var array = image.Split('/');
            string trimmedUrl = "";
            for(int i = 1;i<=4;i++)
            {
                trimmedUrl = trimmedUrl.Insert(0, array[array.Length - i]);
            }
            if(trimmedUrl.Contains('?'))
            {
                trimmedUrl = trimmedUrl.Substring(0, trimmedUrl.IndexOf("?"));
            }
            int count = Images.Where(x => x == trimmedUrl).Count();
            if (count != 0)
                return true;
            else
                return false;
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
                        .Select(p => p.InnerText.Replace(" ", "").Replace("\n", "")).Select(o => o);
                }
                catch
                {
                    List<string> list = new List<string>();
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
                try
                {
                    video.Categories = document.DocumentNode.SelectNodes("//div[@class='video-infobox-content']")[4].SelectNodes(".//a")
                        .Select(x => x.InnerText);
                }
                catch
                {
                    List<string> list = new List<string>();
                    video.Categories = list;
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
                        .Select(x => x.GetAttributeValue("title", string.Empty)).ToList();
                }
                catch
                {
                    List<string> list = new List<string>();
                    video.Categories = list;
                }
            }
        }
        private IEnumerable<string> GetImages()
        {
            using (var db = new myDb())
            {
                //var query = db.Videos.Where(x=>x.Categories.Select(p=>p.Name).Contains(category));
                var images = db.Videos.OrderByDescending(p => p.Id)
                    .Select(x => x.Img).Take(50).ToList();
                return images.Select(x => x.Split('/')[2]);
            }
        }
    }
}