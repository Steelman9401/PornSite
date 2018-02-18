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
using PornSite.SupportClasses;

namespace PornSite.ViewModels
{
    public class AdminViewModel : MasterPageViewModel
    {
        public List<VideoDTO> Videos { get; set; } = new List<VideoDTO>();
        public IEnumerable<string> Urls { get; set; }
        public VideoDTO Video { get; set; }
        public PornRepository rep { get; set; } = new PornRepository();
        public IEnumerable<string> Categories { get; set; }
        public IEnumerable<string> DatabaseCategories { get; set; }
        public List<WebCategory> WebCategories { get; set; } = new List<WebCategory>
        {
            new WebCategory() { Name = "Main", Url = "https://www.redtube.com"},
            new WebCategory() { Name = "Anal", Url = "https://www.redtube.com/redtube/anal"},
            new WebCategory() { Name = "Amateur", Url = "https://www.redtube.com/redtube/amateur"},
            new WebCategory() { Name = "Asian", Url = "https://www.redtube.com/redtube/asian"},
            new WebCategory() { Name = "Blonde", Url = "https://www.redtube.com/redtube/blonde"},
            new WebCategory() { Name = "Ebony", Url = "https://www.redtube.com/redtube/ebony"},
            new WebCategory() { Name = "Zrzky", Url = "https://www.redtube.com/redtube/redhead"},
            new WebCategory() { Name = "MILF", Url = "https://www.redtube.com/redtube/milf"},

        };
        public string SelectedWebCategory { get; set; } = "https://www.redtube.com";
        public bool ModalSwitch { get; set; }

        public async override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                var taskCat = Task.Run(() => this.GetCategories());
                var taskUrl = Task.Run(() => this.GetUrls());
                taskUrl.Wait();
                FillList();
                taskCat.Wait();
            }
            
            await base.PreRender();
        }

        private async Task GetCategories()
        {
            DatabaseCategories = await rep.GetAllCategories();
        }
        private async Task GetUrls()
        {
            Urls = await rep.GetUrls();
        }
        private void FillList()
        {
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
                    GetVideoData(a, Url, ref index);                   
                }
                else if (a.Name == "span")
                {
                    
                    var newA = a.PreviousSibling.PreviousSibling;
                    var Url = newA.GetAttributeValue("href", string.Empty).Substring(1);
                    GetVideoData(newA, Url, ref index);
                }
            }
        }
        private void GetVideoData(HtmlNode node, string Url, ref int index)
        {          
            if (!IfExists("https://embed.redtube.com/?id=" + Url))
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

        private  void GetTags()
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
            Task.Run(() => this.AddVideoAsync(vid));
            ModalSwitch = false;
            Videos.RemoveAt(vid.Index);
        }
        public async Task AddVideoAsync(VideoDTO vid)
        {
            Video video = new Video();
            video.Description = vid.Description;
            video.Title = vid.Title;
            video.Url = vid.Url;
            video.Img = vid.Img;
            video.Date = DateTime.Now;
            video.Preview = vid.Preview;
            PornRepository rep = new PornRepository();
            List<Category> listCat = new List<Category>();
            foreach (string item in Categories)
            {
                Category cat = new Category();
                cat.Name = item;
                listCat.Add(cat);
            }
            await rep.AddPorn(video, listCat);
        }
        public void ChangeCategoryList()
        {
            Videos = new List<VideoDTO>();
            FillList();
        }
    }
}

