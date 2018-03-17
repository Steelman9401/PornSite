using HtmlAgilityPack;
using PornSite.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace PornSite.Repositories
{
    public class ScrapperRepository
    {
        public void GetXhamsterVideos(List<VideoDTO> Videos, string category, IEnumerable<string> Images)
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
                    VideoDTO video = new VideoDTO();
                    var a = item.FirstChild.NextSibling;
                    video.Img = a.ChildNodes[3].GetAttributeValue("src", string.Empty);
                    if (!ImageExists(video.Img, Images))
                    {
                        video.Preview = a.GetAttributeValue("data-previewvideo", string.Empty);
                        video.Url = a.GetAttributeValue("href", string.Empty);
                        video.Title = a.ChildNodes[3].GetAttributeValue("alt", string.Empty);
                        video.Duration = a.ChildNodes[5].InnerText;
                        Videos.Add(video);
                    }
                }
            }

        }
        public void GetRedTubeVideos(List<VideoDTO> Videos, string category, IEnumerable<string> Images)
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
                    VideoDTO video = new VideoDTO();
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
                        video.Preview = img.GetAttributeValue("data-mediabook", string.Empty);
                        video.Title = img.GetAttributeValue("alt", string.Empty);
                        video.Url = "https://redtube.com" + a.GetAttributeValue("href", string.Empty);
                        if (!string.IsNullOrWhiteSpace(video.Img) && !string.IsNullOrWhiteSpace(video.Preview))
                        {
                            Videos.Add(video);
                        }
                    }
                }

            }
        }
        private bool ImageExists(string image, IEnumerable<string> Images)
        {
            int count = Images.Where(x => x == image).Count();
            if (count != 0)
                return true;
            else
                return false;
        }
        public void GetCategoriesXhamster(ref VideoDTO video)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("user-agent", "Mozilla / 5.0(Windows NT 10.0; WOW64; Trident / 7.0; rv: 11.0) like Gecko");
                var html = client.DownloadString(video.Url);
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                string[] array = video.Url.Split('-');
                video.Url = "https://xhamster.com/xembed.php?video=" + array[array.Length - 1];
                try
                {
                    video.Categories = document.DocumentNode.SelectNodes("//div[@class='entity-description-container__categories categories-container']")
                        .First()
                        .ChildNodes
                        .Where(x => x.Name == "a")
                        .Select(p => p.InnerText.Replace(" ", "").Replace("\n", "")).Select(o => new CategoryDTO()
                        {
                            Name = o
                        }).ToList();
                }
                catch
                {

                }
            }
        }
        public void GetCategoriesRedTube(ref VideoDTO video)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("user-agent", "Mozilla / 5.0(Windows NT 10.0; WOW64; Trident / 7.0; rv: 11.0) like Gecko");
                var html = client.DownloadString(video.Url);
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                var url = video.Url.Substring(20);
                video.Url = "https://embed.redtube.com/?id=" + url;
                video.Categories = document.DocumentNode.SelectNodes("//div[@class='video-infobox-content']")[4].SelectNodes(".//a")
                    .Select(x => new CategoryDTO()
                    {
                        Name = x.InnerText
                    }).ToList();
            }
        }
    }
}