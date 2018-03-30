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

namespace PornSite.ViewModels
{
    public class DefaultViewModel : MasterPageViewModel
    {
        public string SearchParametr { get; set; } = "";
        public bool LoadMobile { get; set; }
        public bool LoadRecommended { get; set; } = true;
        public int Switch { get; set; } = 0;
        public int CatComSwitch { get; set; } = 0;
        public bool ShowVideo { get; set; } = false;
        public VideoDetailDTO Video { get; set; } = new VideoDetailDTO();
        public string ComText { get; set; }
        PornRepository PornRep = new PornRepository();
        public List<VideoListDTO> RecommendedVideos { get; set; }
        public List<VideoListDTO> History { get; set; } = new List<VideoListDTO>();
        public GridViewDataSet<VideoListDTO> Videos { get; set; } = new GridViewDataSet<VideoListDTO>()
        {
            PagingOptions = { PageSize = 20 }
        };
        public string UserId
        {
            get { return Context.GetOwinContext().Authentication.User.Identity.GetUserId(); }
        }
        public async override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                History = await PornRep.GetVideoHistory();
            }
            if(LoadRecommended)
            {
                RecommendedVideos = await PornRep.GetRecommendedVideos();
            }
            if (!ShowVideo)
            {
                if (Videos.IsRefreshRequired || !Context.IsPostBack)
                {
                    Videos.OnLoadingDataAsync = option => PornRep.GetAllVideos(option,LoadMobile,Switch);
                }
            }
            else if (CatComSwitch == 1)
            {
                Video.GetComments();
            }
            LoadRecommended = true;
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
            CatComSwitch = 2;
            await Video.GetSuggestedVideos();
        }
        public void GetComments()
        {
            CatComSwitch = 1;
            Video.GetComments();
        }
        public async Task AddComment()
        {
            await Video.AddComment(ComText, Convert.ToInt32(UserId), CurrentUser);
        }
        public async Task LoadVideo(VideoListDTO video)
        {
            LoadRecommended = false;
            ShowVideo = true;
            Video = await PornRep.GetVideoById(video.Id);
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
            LoadRecommended = false;
            ShowVideo = false;
            CatComSwitch = 0;
        }
        public void GetMobileVideos()
        {
            Videos.PageIndex = 0;
            LoadRecommended = false;
            Videos.IsRefreshRequired = true;
        }
    }
}
