using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using HtmlAgilityPack;
using PornSite.Data;
using PornSite.DTO;
using PornSite.Repositories;

namespace PornSite.ViewModels
{
    public class AdminViewModel : MasterPageViewModel
    {
        public List<VideoDTO> Videos { get; set; } = new List<VideoDTO>();
        public IEnumerable<string> Urls { get; set; }
        public VideoDTO Video { get; set; }
        public PornRepository rep { get; set; } = new PornRepository();
        public IEnumerable<string> Categories { get; set; }

        public bool ModalSwitch { get; set; }

        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                Urls = rep.GetUrls();
                FillList();
            }
            return base.PreRender();
        }

        private void FillList()
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load("https://www.redtube.com/");
            IEnumerable<HtmlNode> liCountry = document.DocumentNode.SelectNodes("//ul[@id='block_hottest_videos_by_country']").First().ChildNodes.ToList();
            IEnumerable<HtmlNode> liRecent = document.DocumentNode.SelectNodes("//ul[@id='most_recent_videos']").First().ChildNodes.ToList();
            IEnumerable<HtmlNode> liComb = liCountry.Concat(liRecent).Where(x => x.Name == "li");
            foreach (HtmlNode item in liComb)
            {
                var a = item.ChildNodes[1].ChildNodes[3];
                if (a.Name == "a")
                {
                    var Url = a.GetAttributeValue("href", string.Empty).Substring(1);
                    if (!IfExists("https://embed.redtube.com/?id=" + Url))
                    {
                        VideoDTO video = new VideoDTO();
                        video.Url = "https://embed.redtube.com/?id=" + Url;
                        video.Title = a.ChildNodes[3].InnerHtml.Replace("  ", "").Substring(1);
                        video.Img = a.ChildNodes[1].ChildNodes[1].GetAttributeValue("data-thumb_url", string.Empty);
                        video.Preview = a.ChildNodes[1].ChildNodes[1].GetAttributeValue("data-mediabook", string.Empty);
                        Videos.Add(video);
                    }
                }
            }           
        }
        private bool IfExists(string Url)
        {
            int count = Urls.Where(x => x == Url).Count();
            if (count != 0)
                return true;
            else
                return false;
        }
        public void OpenModal(VideoDTO vid)
        {

            Video = vid;
            GetTags();
            ModalSwitch = true;
        }
        public void CloseModal()
        {
            ModalSwitch = false;
        }

        private void GetTags()
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load("https://www.redtube.com/" + Video.Url.Substring(30));
            var category = document.DocumentNode.SelectNodes("//div").Where(x => x.InnerHtml == "Categories").FirstOrDefault();
            if (category != null)
            {
                Categories = category.NextSibling.NextSibling.ChildNodes.Where(x => x.Name == "a").Select(p=>p.InnerText);
            }

        }

        public void AddVideo(VideoDTO vid)
        {
            Video video = new Video();
            video.Description = vid.Description;
            video.Title = vid.Title;
            video.Url = vid.Url;
            video.Img = vid.Img;
            video.Date = DateTime.Now;
            video.Preview = Video.Preview;
            PornRepository rep = new PornRepository();
            List<Category> listCat = new List<Category>();
            foreach (string item in Categories)
            {
                Category cat = new Category();
                cat.Name = item;
                listCat.Add(cat);
            }
            rep.AddPorn(video, listCat);
            ModalSwitch = false;
        }
    }
}

