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
        public GridViewDataSet<VideoDTO> Videos { get; set; } = new GridViewDataSet<VideoDTO>()
        {
            PagingOptions = { PageSize = 20 }
        };
        public string SearchParametr { get; set; } = "";
        public string UserId
        {
            get { return Context.GetOwinContext().Authentication.User.Identity.GetUserId(); }
        }
        public bool LoadMobile { get; set; } = false;
        public PornRepository rep { get; set; } = new PornRepository();
        public int Switch { get; set; } = 0;
        public int CatComSwitch { get; set; } = 0;
        public bool ShowVideo { get; set; } = false;
        public VideoDTO Video { get; set; } = new VideoDTO();
        public string ComText { get; set; }
        PornRepository PornRep = new PornRepository();
        public async override Task PreRender()
        {
            if (!string.IsNullOrEmpty(Context.Parameters["text"].ToString()))
            {
                SearchParametr = Context.Parameters["text"].ToString();
            }
            if (!ShowVideo)
            {
                if (Switch == 0 && (Videos.IsRefreshRequired || !Context.IsPostBack))
                {
                    Videos.OnLoadingDataAsync = option => rep.GetAllVideos(option, SearchParametr, LoadMobile);
                }
                else if( Switch == 1 && (Videos.IsRefreshRequired || !Context.IsPostBack))
                {
                    Videos.OnLoadingDataAsync = option => rep.GetAllVideosByViews(option, SearchParametr, LoadMobile);
                }
            }
            else if (CatComSwitch == 1)
            {
                Video.GetComments();
            }
            await base.PreRender();
        }
        public void LoadMostViewed()
        {
            Videos.IsRefreshRequired = true;
            Switch = 1;
        }

        public void LoadLatest()
        {
            Switch = 0;
            Videos = new GridViewDataSet<VideoDTO>()
            {
                PagingOptions = { PageSize = 20 }
            };
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
        public async Task LoadVideo(VideoDTO video)
        {
            ShowVideo = true;
            Video = await PornRep.GetVideoById(video.Id);
        }
        public void HideVideo()
        {
            ShowVideo = false;
            CatComSwitch = 0;
        }
        public void GetMobileVideos()
        {
            Videos.IsRefreshRequired = true;
        }
    }
}
