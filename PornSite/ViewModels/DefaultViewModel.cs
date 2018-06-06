using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Hosting;
using PornSite.DTO;
using PornSite.Repositories;
using DotVVM.Framework.Controls;
using PornSite.Data;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Web;

namespace PornSite.ViewModels
{
    public class DefaultViewModel : MasterPageViewModel
    {
        public string SearchParametr { get; set; } = "";
        public bool LoadMobile { get; set; }
        public bool LoadRecommended { get; set; }
        public int Switch { get; set; } = 0;
        public long TimeStamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        public bool ShowVideo { get; set; } = false;
        public VideoDetailDTO Video { get; set; } = new VideoDetailDTO();
        PornRepository PornRep = new PornRepository();
        public IEnumerable<VideoListDTO> RecommendedVideos { get; set; }
        public List<VideoListDTO> History { get; set; } = new List<VideoListDTO>();
        public GridViewDataSet<VideoListDTO> Videos { get; set; } = new GridViewDataSet<VideoListDTO>()
        {
            PagingOptions = { PageSize = 20 }
        };

        public async override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                History = await PornRep.GetVideoHistoryAsync(currentCulture);
                RecommendedVideos = await PornRep.GetRecommendedVideosAsync(currentCulture);
            }
            if(Videos.IsFirstPage)
            {
                LoadRecommended = true;
            }
            else
            {
                LoadRecommended = false;
            }
            if (!ShowVideo)
            {
                if (Videos.IsRefreshRequired || !Context.IsPostBack)
                {
                    Videos.OnLoadingData = option => PornRep.GetAllVideos(option,LoadMobile,Switch,currentCulture);
                }
            }
            await base.PreRender();
        }
        public void LoadMostViewedVideos()
        {
            LoadRecommended = false;
            Videos.PageIndex = 0;
            Videos.IsRefreshRequired = true;
            Switch = 1;
        }

        public void LoadLatestVideos()
        {
            Videos.PageIndex = 0;
            LoadRecommended = false;
            Videos.IsRefreshRequired = true;
            Switch = 0;
        }
        public async Task GetSuggestions()
        {
            await Video.LoadSuggestedVideos(currentCulture);
        }
        public async Task LoadVideo(VideoListDTO video)
        {
            LoadRecommended = false;
            ShowVideo = true;
            Video = await PornRep.GetVideoByIdAsync(video.Id, currentCulture);
            if (!History.Select(x=>x.Id).Contains(video.Id))
            {
                History.Insert(0, video);
            }
            if (History.Count == 5)
            {
                History.RemoveAt(History.Count - 1);
            }
        }
        public void HideVideo()
        {
            ShowVideo = false;
            Video.Url = "";
        }
        public async Task LoadMobileVideos()
        {
            Videos.PageIndex = 0;
            LoadRecommended = false;
            Videos.IsRefreshRequired = true;
        }
    }
}
