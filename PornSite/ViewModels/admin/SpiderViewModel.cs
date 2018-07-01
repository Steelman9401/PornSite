using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.Runtime.Filters;
using DotVVM.Framework.ViewModel;
using PornSite.DTO;
using PornSite.Repositories.admin;
using PornSite.SupportClasses;

namespace PornSite.ViewModels.admin
{
    [Authorize()]
    public class SpiderViewModel : AdminMasterPageViewModel
    {
        public List<VideoListAdminDTO> Videos { get; set; } = new List<VideoListAdminDTO>();
        public SpiderRepository SpiderRep { get; set; } = new SpiderRepository();
        public int SwitchWebsite { get; set; } = 3;
        public string ImageName { get; set; }
        public VideoAdminDTO Video { get; set; } = new VideoAdminDTO();
        public AdminRepository AdminRep { get; set; } = new AdminRepository();
        public List<WebCategory> WebCategories { get; set; } = new List<WebCategory>
        {
            new WebCategory() { Name = "Hlavní stránka", Url = ""},
            new WebCategory() { Name = "Anál", Url = "anal"},
            new WebCategory() { Name = "Amatérky", Url = "amateur"},
            new WebCategory() { Name = "Asiatky", Url = "asian"},
            new WebCategory() { Name = "Blondýny", Url = "blonde"},
            new WebCategory() { Name = "Èernošky", Url = "ebony"},
            new WebCategory() { Name = "Zrzky", Url = "redhead"},
            new WebCategory() { Name = "MILF", Url = "milf"},
            new WebCategory() { Name = "Gay", Url = "gay"},
            new WebCategory() { Name = "Brunetky", Url = "brunette"},
            new WebCategory() { Name = "Lesbièky", Url = "lesbian"},
            new WebCategory() { Name = "Teenky", Url = "teen"},
            new WebCategory() { Name = "Èešky", Url = "czech"},
            new WebCategory() { Name = "BDSM", Url = "bdsm"},
            new WebCategory() { Name = "Castingy", Url = "casting"},
            new WebCategory() { Name = "Celebrity", Url = "celebrity"},
            new WebCategory() { Name = "Dvojèata", Url = "twins"},
            new WebCategory() { Name = "Gangbang", Url = "gangbang"},
            new WebCategory() { Name = "Hraèky", Url = "toys"},
            new WebCategory() { Name = "Interracial", Url = "interracial"},
            new WebCategory() { Name = "Kompilace", Url = "compilation"},
            new WebCategory() { Name = "Obleèky", Url = "costume"},
            new WebCategory() { Name = "Latinská amerika", Url = "latina"},
            new WebCategory() { Name = "Romantika", Url = "romantic"},

        };
        public List<string> Hubs { get; set; } = new List<string>
        {
            "Pornhub","Redtube"
        };
        public string SelectedWebCategory { get; set; } = "";
        public string SelectedPage { get; set; }
        public string Search { get; set; }
        public async override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                SpiderRep.GetPornHubVideos(Videos, string.Empty);
            }
            if (SwitchWebsite == 0)
                ImageName = "../../Content/img/admin/redtube.png";
            else if (SwitchWebsite == 1)
                ImageName = "../../Content/img/admin/xhamster.png";
            else if (SwitchWebsite == 2)
                ImageName = "../../Content/img/admin/drtuber.png";
            else if (SwitchWebsite == 3)
                ImageName = "../../Content/img/admin/pornhub.png";

            await base.PreRender();
        }

        public async Task OpenModal(VideoListAdminDTO vid)
        {
            Video.Duration = vid.Duration;
            Video.HD = vid.HD;
            Video.Img = vid.Img;
            Video.Preview = vid.Preview;
            Video.Title = vid.Title;
            Video.Title_en = vid.Title_en;
            Video.Url = vid.Url;
            Video.Id = vid.Id;
            if (SelectedWebCategory == "gay")
            {
                Video.AllowMain = false;
                Video.CheckBoxEnabled = false;
            }
            if (Video.DatabaseCategories.Count() == 0)
            {
                Video.DatabaseCategories = await AdminRep.GetCategoriesAsync();
            }
            if (SwitchWebsite == 0)
                SpiderRep.GetCategoriesRedTube(Video);
            else if (SwitchWebsite == 1)
                SpiderRep.GetCategoriesXhamster(Video);
            else if (SwitchWebsite == 2)
                SpiderRep.GetCategoriesDrTuber(Video);
            else if (SwitchWebsite == 3)
                SpiderRep.GetCategoriesPornHub(Video);
        }
        public void ChangeCategoryList()
        {
            Videos = new List<VideoListAdminDTO>();
            if (SwitchWebsite == 1)
                SpiderRep.GetXhamsterVideos(Videos, SelectedWebCategory);
            else if (SwitchWebsite == 0)
                SpiderRep.GetRedTubeVideos(Videos, SelectedWebCategory);
            else if (SwitchWebsite == 2)
                SpiderRep.GetDrTuberVideos(Videos, SelectedWebCategory);
            else if (SwitchWebsite == 3)
                SpiderRep.GetPornHubVideos(Videos, SelectedWebCategory);
        }
        public void SearchVideos()
        {
            if(!string.IsNullOrWhiteSpace("Search"))
            {
                if(SelectedPage == "Redtube")
                {
                    SwitchWebsite = 0;
                    Videos = new List<VideoListAdminDTO>();
                    SpiderRep.GetRedTubeVideos(Videos, Search);
                }
                else if (SelectedPage == "Pornhub")
                {
                    SwitchWebsite = 3;
                    Videos = new List<VideoListAdminDTO>();
                    SpiderRep.GetPornHubVideos(Videos, Search);
                }
            }
        }
        public void SwitchToRedTube()
        {
            SwitchWebsite = 0;
            Videos = new List<VideoListAdminDTO>();
            SpiderRep.GetRedTubeVideos(Videos, SelectedWebCategory);
        }
        public void SwitchToPornHub()
        {
            SwitchWebsite = 3;
            Videos = new List<VideoListAdminDTO>();
            SpiderRep.GetPornHubVideos(Videos, SelectedWebCategory);
        }
        public async Task AddVideo()
        {
            await Video.AddVideo(CurrentUser);
            Videos.RemoveAll(x => x.Id == Video.Id);
        }
        public async Task AddCategory()
        {
            if (await Video.AddCategoryToDatabase(CurrentUser))
            {
                Video.NewCategory.Name = "";
                Video.NewCategory.Name_en = "";
                Video.DatabaseCategories = await AdminRep.GetCategoriesAsync();
            }
        }
        public async Task CreateCustomVideo()
        {
            if (Video.DatabaseCategories.Count() == 0)
            {
                Video.DatabaseCategories = await AdminRep.GetCategoriesAsync();
            }
            Video.IsCustom = true;
            List<CategoryDTO> list = new List<CategoryDTO>();
            for (int i = 0; i < 3; i++)
            {
                CategoryDTO cat = new CategoryDTO();
                list.Add(cat);
            }
            Video.Categories = list;
        }
        public void SwitchToXhamster()
        {
            SwitchWebsite = 1;
            Videos = new List<VideoListAdminDTO>();
            SpiderRep.GetXhamsterVideos(Videos, SelectedWebCategory);
        }
        public void SwitchToDrTuber()
        {
            SwitchWebsite = 2;
            Videos = new List<VideoListAdminDTO>();
            SpiderRep.GetDrTuberVideos(Videos, SelectedWebCategory);
        }
        public void CloseModal()
        {
            Video.Categories = new List<CategoryDTO>();
            Video.CategoryAdded = false;
            Video.ErrorMessage = "";
            Video.IsCustom = false;
            Video.IsEdited = false;
            Video.NewCategory = new CategoryDTO();
            Video.ShowCategoryOption = false;
            Video.Success = false;
            Video.Url = null;
            Video.AllowMain = true;
            Video.CheckBoxEnabled = true;
            Video.LoadError = false;
        }
    }
}

