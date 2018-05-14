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
    public class SearchViewModel : MasterPageViewModel
    {
        public string SearchParameter { get; set; }
        public GridViewDataSet<VideoListDTO> Videos { get; set; } = new GridViewDataSet<VideoListDTO>()
        {
            PagingOptions = { PageSize = 20 }
        };
        public int VideoSwitch { get; set; } //switch between most viewed and latest
        PornRepository PornRep = new PornRepository();
        public int SearchCount { get; set; }
        public bool LoadMobile { get; set; }
        public VideoDetailDTO Video { get; set; } = new VideoDetailDTO();
        public bool ShowVideo { get; set; }
        public async override Task PreRender()
        {
            if (!ShowVideo)
            {
                if (!string.IsNullOrEmpty(Context.Parameters["text"].ToString()))
                {
                    SearchParameter = Context.Parameters["text"].ToString();
                }               
                    Videos.OnLoadingData = option => PornRep.GetSearchResult(option, SearchParameter, LoadMobile, VideoSwitch);
                if (Videos.IsFirstPage)
                {
                    SearchCount = await PornRep.GetSearchResultCount(SearchParameter);
                }
            }
            await base.PreRender();
        }
        public async Task LoadVideo(VideoListDTO video)
        {
            ShowVideo = true;
            Video = await PornRep.GetVideoByIdAsync(video.Id);
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
