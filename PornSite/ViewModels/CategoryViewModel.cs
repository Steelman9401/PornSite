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
        public bool ShowVideo { get; set; }
        public int VideoSwitch { get; set; }
        public bool LoadMobile { get; set; }
        public int CategoryCount { get; set; }
        public GridViewDataSet<VideoListDTO> Videos { get; set; } = new GridViewDataSet<VideoListDTO>()
        {
            PagingOptions = { PageSize = 20 }
        };
        public VideoDetailDTO Video { get; set; } = new VideoDetailDTO();
        public async override Task PreRender()
        {
            if (!ShowVideo)
            {
                if (!string.IsNullOrEmpty(Context.Parameters["Id"].ToString()) && !string.IsNullOrEmpty(Context.Parameters["Name"].ToString()))
                {
                    PornRepository PornRep = new PornRepository();
                    try
                    {
                        Id = Convert.ToInt16(Context.Parameters["Id"].ToString());
                        Name = Context.Parameters["Name"].ToString();
                    }
                    catch
                    {
                        Context.RedirectToRoute("Default");
                    }
                    if (Videos.IsFirstPage)
                    {
                        CategoryCount = await PornRep.GetCategoryVideosCount(Id);
                    }
                    Videos.OnLoadingData = option => PornRep.GetVideosByCategory(option, Id, VideoSwitch, LoadMobile, currentCulture);
                }
            }
            await base.PreRender();
        }
        public async Task LoadVideo(VideoListDTO video)
        {
            PornRepository PornRep = new PornRepository();
            ShowVideo = true;
            Video = await PornRep.GetVideoByIdAsync(video.Id, currentCulture);
        }
        public void LoadLatestVideos()
        {
            Videos.PageIndex = 0;
            VideoSwitch = 0;
            Videos.IsRefreshRequired = true;
        }
        public void LoadMostViewedVideos()
        {
            VideoSwitch = 1;
            Videos.PageIndex = 0;
            Videos.IsRefreshRequired = true;
        }
        public void LoadMobileVideos()
        {
            Videos.PageIndex = 0;
            Videos.IsRefreshRequired = true;
        }
        public void HideVideo()
        {
            ShowVideo = false;
            Video.Url = "";
        }
    }
}
