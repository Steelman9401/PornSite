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
        public List<VideoAdminDTO> Videos { get; set; } = new List<VideoAdminDTO>();
        public ScrapperRepository ScrapRep { get; set; } = new ScrapperRepository();
        public IEnumerable<string> Images { get; set; }
        public int SwitchWebsite { get; set; }
        public VideoAdminDTO Video { get; set; }
        public AdminRepository AdminRep { get; set; } = new AdminRepository();
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
                Images = await AdminRep.GetImages();
                ScrapRep.GetRedTubeVideos(Videos, string.Empty, Images);
            }

            await base.PreRender();
        }

        public void OpenModal(VideoAdminDTO vid)
        {
            Video = vid;
            if (SwitchWebsite == 0)
            ScrapRep.GetCategoriesRedTube(ref vid);
            else if(SwitchWebsite==1)          
            ScrapRep.GetCategoriesXhamster(ref vid);
            else if (SwitchWebsite == 2)
            ScrapRep.GetCategoriesDrTuber(ref vid);
            ModalSwitch = true;
        }
        public void CloseModal()
        {
            ModalSwitch = false;
        }

        public async Task AddVideo(VideoAdminDTO vid)
        {
            ModalSwitch = false;
            Videos.RemoveAll(x => x.Url == vid.Url);
            await vid.AddVideo();
        }
        public void ChangeCategoryList()
        {
            Videos = new List<VideoAdminDTO>();
            if (SwitchWebsite == 1)
                ScrapRep.GetXhamsterVideos(Videos, SelectedWebCategory,Images);
            else if(SwitchWebsite==0)
                ScrapRep.GetRedTubeVideos(Videos, SelectedWebCategory,Images);
            else if(SwitchWebsite==2)
                ScrapRep.GetDrTuberVideos(Videos, SelectedWebCategory, Images);
        }
        public void SwitchToRedTube()
        {
            SwitchWebsite = 0;
            Videos = new List<VideoAdminDTO>();
            ScrapRep.GetRedTubeVideos(Videos, SelectedWebCategory,Images);
        }
        public void SwitchToXhamster()
        {
            SwitchWebsite = 1;
            Videos = new List<VideoAdminDTO>();
            ScrapRep.GetXhamsterVideos(Videos, SelectedWebCategory,Images);
        }
        public void SwitchToDrTuber()
        {
            SwitchWebsite = 2;
            Videos = new List<VideoAdminDTO>();
            ScrapRep.GetDrTuberVideos(Videos, SelectedWebCategory, Images);
        }
    }
}

