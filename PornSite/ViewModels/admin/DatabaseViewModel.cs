using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Runtime.Filters;
using DotVVM.Framework.ViewModel;
using PornSite.DTO;
using PornSite.Repositories.admin;

namespace PornSite.ViewModels.admin
{
    [Authorize()]
    public class DatabaseViewModel : AdminMasterPageViewModel
    {
        public AdminRepository AdminRep { get; set; } = new AdminRepository();
        public GridViewDataSet<VideoListAdminDTO> Videos { get; set; } = new GridViewDataSet<VideoListAdminDTO>()
        {
            PagingOptions = { PageSize = 20 }
        };
        public VideoAdminDTO Video { get; set; }
        public override Task PreRender()
        {
            Videos.OnLoadingData = option => AdminRep.GetAllVideos(option, CurrentUser);
            return base.PreRender();
        }
        public async Task EditVideo(VideoListAdminDTO vid)
        {
            Video = await AdminRep.GetVideoByIdAsync(vid.Id);
            Video.DatabaseCategories = await AdminRep.GetCategoriesAsync();
            Video.IsEdited = true;
        }
        public async Task UpdateVideo()
        {
            await Video.UpdateVideo();
            var editedVideo = Videos.Items
                 .Where(x => x.Id == Video.Id)
                 .First();
            editedVideo.Title = Video.Title;
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
            Video.IsEdited = false;
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
    }
}

