using HtmlAgilityPack;
using PornSite.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PornSite.Repositories
{
    public class ScrapperRepository
    {
        public List<VideoDTO> FillList(string SelectedWebCategory, IEnumerable<string> Urls)
        {
            List<VideoDTO> Videos = new List<VideoDTO>();
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(SelectedWebCategory);
            IEnumerable<HtmlNode> liComb;
            if (SelectedWebCategory == "https://www.redtube.com")
            {
                IEnumerable<HtmlNode> liCountry = document.DocumentNode.SelectNodes("//ul[@id='block_hottest_videos_by_country']").First().ChildNodes.Where(x => x.Name == "li");
                IEnumerable<HtmlNode> liRecent = document.DocumentNode.SelectNodes("//ul[@id='most_recent_videos']").First().ChildNodes.Where(x => x.Name == "li");
                liComb = liCountry.Concat(liRecent);
            }
            else
            {
                liComb = document.DocumentNode.SelectNodes("//ul[@id='block_browse']").First().ChildNodes.Where(x => x.Name == "li");
            }
            int index = 0;
            foreach (HtmlNode item in liComb)
            {
                var a = item.ChildNodes[1].ChildNodes[3];
                if (a.Name == "a")
                {
                    var Url = a.GetAttributeValue("href", string.Empty).Substring(1);
                    GetVideoData(a, Url, ref index, Videos, Urls);
                }
                else if (a.Name == "span")
                {

                    var newA = a.PreviousSibling.PreviousSibling;
                    var Url = newA.GetAttributeValue("href", string.Empty).Substring(1);
                    GetVideoData(newA, Url, ref index, Videos, Urls);
                }
            }
            return Videos;
        }
        private void GetVideoData(HtmlNode node, string Url, ref int index, List<VideoDTO> Videos, IEnumerable<string> Urls)
        {
            if (!IfExists("https://embed.redtube.com/?id=" + Url,Urls))
            {
                VideoDTO video = new VideoDTO();
                video.Url = "https://embed.redtube.com/?id=" + Url;
                video.Title = node.ChildNodes[3].InnerHtml.Replace("  ", "").Substring(1);
                video.Img = node.ChildNodes[1].ChildNodes[1].GetAttributeValue("data-thumb_url", string.Empty);
                video.Preview = node.ChildNodes[1].ChildNodes[1].GetAttributeValue("data-mediabook", string.Empty);
                if (!string.IsNullOrWhiteSpace(video.Img) && !string.IsNullOrWhiteSpace(video.Preview))
                {
                    video.Index = index;
                    Videos.Add(video);
                    index++;
                }
            }
        }
        private bool IfExists(string Url, IEnumerable<string> Urls)
        {
            int count = Urls.Where(x => x == Url).Count();
            if (count != 0)
                return true;
            else
                return false;
        }

        public IEnumerable<string> GetTags(VideoDTO Video)
        {
            IEnumerable<string> Categories;
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load("https://www.redtube.com/" + Video.Url.Substring(30));
            var category = document.DocumentNode.SelectNodes("//div").Where(x => x.InnerHtml == "Categories").FirstOrDefault();
            if (category != null)
            {
                Categories = category.NextSibling.NextSibling.ChildNodes.Where(x => x.Name == "a").Select(p => p.InnerText);
            }
            else
            {
                Categories = null;
            }
            return Categories;
        }
    }
}