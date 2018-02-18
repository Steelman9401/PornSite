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
        public ScrapperRepository scrapRep { get; set; } = new ScrapperRepository();
        public IEnumerable<string> Urls { get; set; }
        public VideoDTO Video { get; set; }
        public PornRepository pornRep { get; set; } = new PornRepository();
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
                Videos = scrapRep.FillList(SelectedWebCategory,Urls);
                taskCat.Wait();
            }
            
            await base.PreRender();
        }

        private async Task GetCategories()
        {
            DatabaseCategories = await pornRep.GetAllCategories();
        }
        private async Task GetUrls()
        {
            Urls = await pornRep.GetUrls();
        }
       
        public void OpenModal(VideoDTO vid)
        {

            Video = vid;
            Categories = scrapRep.GetTags(vid);
            ModalSwitch = true;
        }
        public void CloseModal()
        {
            ModalSwitch = false;
        }

        public void AddVideo(VideoDTO vid)
        {
            Task.Run(() => this.AddVideoAsync(vid));
            ModalSwitch = false;
            Videos.RemoveAt(vid.Index);
        }
        public async Task AddVideoAsync(VideoDTO vid)
        {            
            await pornRep.AddPorn(vid, Categories);
        }
        public void ChangeCategoryList()
        {
            Videos = new List<VideoDTO>();
            Videos = scrapRep.FillList(SelectedWebCategory, Urls);
        }
    }
}

