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
        public VideoDetailDTO Video { get; set; }
        public bool ShowVideo { get; set; }
        public async override Task PreRender()
        {
            if (!string.IsNullOrEmpty(Context.Parameters["text"].ToString()))
            {
                SearchParameter = Context.Parameters["text"].ToString();
            }
            Videos.OnLoadingDataAsync = option => PornRep.GetSearchResult(option, SearchParameter,LoadMobile,VideoSwitch);
            SearchCount = await PornRep.GetSearchResultCount(SearchParameter);
            await base.PreRender();
        }
        public async Task LoadVideo(VideoListDTO video)
        {
            ShowVideo = true;
            Video = await PornRep.GetVideoById(video.Id);
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
        
    }
}
