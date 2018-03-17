using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Storage;
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
        public ScrapperRepository ScrapRep { get; set; } = new ScrapperRepository();
        public IEnumerable<string> Images { get; set; }
        public int SwitchWebsite { get; set; }
        public VideoDTO Video { get; set; }
        public PornRepository PornRep { get; set; } = new PornRepository();
        public IEnumerable<string> DatabaseCategories { get; set; }
        public List<WebCategory> WebCategories { get; set; } = new List<WebCategory>
        {
            new WebCategory() { Name = "Main", Url = ""},
            new WebCategory() { Name = "Anal", Url = "anal"},
            new WebCategory() { Name = "Amateur", Url = "amateur"},
            new WebCategory() { Name = "Asian", Url = "asian"},
            new WebCategory() { Name = "Blonde", Url = "blonde"},
            new WebCategory() { Name = "Ebony", Url = "ebony"},
            new WebCategory() { Name = "Zrzky", Url = "redhead"},
            new WebCategory() { Name = "MILF", Url = "milf"},

        };
        public string SelectedWebCategory { get; set; } = "https://www.redtube.com";
        public bool ModalSwitch { get; set; }

        public async override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                DatabaseCategories = await PornRep.GetAllCategories();
                Images = await PornRep.GetImages();
                ScrapRep.GetRedTubeVideos(Videos,string.Empty,Images);
            }

            await base.PreRender();
        }

        public void OpenModal(VideoDTO vid)
        {
            Video = vid;
            if (SwitchWebsite == 0)
            {
                ScrapRep.GetCategoriesRedTube(ref vid);
            }
            else
            {
                ScrapRep.GetCategoriesXhamster(ref vid);
            }
            ModalSwitch = true;
        }
        public void CloseModal()
        {
            ModalSwitch = false;
        }

        public async Task AddVideo(VideoDTO vid)
        {
            await PornRep.AddPorn(vid);
            ModalSwitch = false;
            Videos.RemoveAll(x => x.Url == vid.Url);
        }
        public void ChangeCategoryList()
        {
            Videos = new List<VideoDTO>();
            if (SwitchWebsite == 1)
                ScrapRep.GetXhamsterVideos(Videos, SelectedWebCategory,Images);
            else
                ScrapRep.GetRedTubeVideos(Videos, SelectedWebCategory,Images);
        }
        public void SwitchToRedTube()
        {
            SwitchWebsite = 0;
            Videos = new List<VideoDTO>();
            ScrapRep.GetRedTubeVideos(Videos, SelectedWebCategory,Images);
        }
        public void SwitchToXhamster()
        {
            SwitchWebsite = 1;
            Videos = new List<VideoDTO>();
            ScrapRep.GetXhamsterVideos(Videos, SelectedWebCategory,Images);
        }
    }
}

