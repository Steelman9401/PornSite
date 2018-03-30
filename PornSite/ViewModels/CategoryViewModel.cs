using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.Controls;
using DotVVM.Framework.ViewModel;
using PornSite.DTO;
using PornSite.Repositories;

namespace PornSite.ViewModels
{
    public class CategoryViewModel : MasterPageViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool LoadMobile { get; set; }
        public GridViewDataSet<VideoListDTO> Videos { get; set; } = new GridViewDataSet<VideoListDTO>()
        {
            PagingOptions = { PageSize = 20 }
        };
        public VideoDetailDTO Video { get; set; }
        public async override Task PreRender()
        {
            if (!string.IsNullOrEmpty(Context.Parameters["Id"].ToString()))
            {

                PornRepository PornRep = new PornRepository();
                Id = Convert.ToInt16(Context.Parameters["Id"].ToString());
                Name = await PornRep.GetCategoryNameById(Id);
                Videos.OnLoadingDataAsync = option => PornRep.GetVideosByCategory(option,Id);
            }
            await base.PreRender();
        }
        public async Task LoadVideo(VideoListDTO video)
        {

        }
        public void LoadLatestVideos()
        {

        }
        public void LoadMostViewedVideos()
        {

        }
        public void LoadMobileVideos()
        {

        }
    }
}
