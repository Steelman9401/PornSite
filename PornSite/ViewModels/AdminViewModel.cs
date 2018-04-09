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
        public List<VideoListAdminDTO> Videos { get; set; } = new List<VideoListAdminDTO>();
        public ScrapperRepository ScrapRep { get; set; } = new ScrapperRepository();
        public int SwitchWebsite { get; set; }
        public VideoAdminDTO Video { get; set; } = new VideoAdminDTO();
        public AdminRepository AdminRep { get; set; } = new AdminRepository();
        public PornRepository PornRep { get; set; } = new PornRepository();
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

        public async override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ScrapRep.GetRedTubeVideos(Videos, string.Empty);
            }

            await base.PreRender();
        }

        public async Task OpenModal(VideoListAdminDTO vid)
        {
            Video.Duration = vid.Duration;
            Video.HD = vid.HD;
            Video.Img = vid.Img;
            Video.Preview = vid.Preview;
            Video.Title_en = vid.Title_en;
            Video.Url = vid.Url;
            Video.DatabaseCategories = await AdminRep.GetCategories();
            if (SwitchWebsite == 0)
            ScrapRep.GetCategoriesRedTube(Video);
            else if(SwitchWebsite==1)          
            ScrapRep.GetCategoriesXhamster(Video);
            else if (SwitchWebsite == 2)
            ScrapRep.GetCategoriesDrTuber(Video);
        }
        public void ChangeCategoryList()
        {
            Videos = new List<VideoListAdminDTO>();
            if (SwitchWebsite == 1)
                ScrapRep.GetXhamsterVideos(Videos, SelectedWebCategory);
            else if(SwitchWebsite==0)
                ScrapRep.GetRedTubeVideos(Videos, SelectedWebCategory);
            else if(SwitchWebsite==2)
                ScrapRep.GetDrTuberVideos(Videos, SelectedWebCategory);
        }
        public void SwitchToRedTube()
        {
            SwitchWebsite = 0;
            Videos = new List<VideoListAdminDTO>();
            ScrapRep.GetRedTubeVideos(Videos, SelectedWebCategory);
        }
        public async Task AddVideo()
        {
            await Video.AddVideo();
            Videos.RemoveAll(x => x.Url == Video.Url);
        }
        public void CreateCustomVideo()
        {
            Video = new VideoAdminDTO();
            Video.IsCustom = true;
        }
        public void SwitchToXhamster()
        {
            SwitchWebsite = 1;
            Videos = new List<VideoListAdminDTO>();
            ScrapRep.GetXhamsterVideos(Videos, SelectedWebCategory);
        }
        public void SwitchToDrTuber()
        {
            SwitchWebsite = 2;
            Videos = new List<VideoListAdminDTO>();
            ScrapRep.GetDrTuberVideos(Videos, SelectedWebCategory);
        }
        public void HideVideo()
        {
            Video = null;
        }
        public void DoStuff()
        {

        }
    }
}

