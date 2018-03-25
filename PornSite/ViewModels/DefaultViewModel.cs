﻿using System;
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
        public bool LoadMobile { get; set; } = false;
        public PornRepository rep { get; set; } = new PornRepository();
        public int Switch { get; set; } = 0;
        public int CatComSwitch { get; set; } = 0;
        public bool ShowVideo { get; set; } = false;
        public VideoDetailDTO Video { get; set; } = new VideoDetailDTO();
        public string ComText { get; set; }
        PornRepository PornRep = new PornRepository();
        public List<VideoListDTO> RecommendedVideos { get; set; } = new List<VideoListDTO>();
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
            if (Videos.IsFirstPage && !Context.IsPostBack) //first load
            {
                RecommendedVideos = await PornRep.GetRecommendedVideos();
            }
            else if(!ShowVideo && Videos.IsRefreshRequired)
            {
                if (Videos.IsRefreshRequired && Videos.IsFirstPage)
                {
                    RecommendedVideos = await PornRep.GetRecommendedVideos();
                }
                else
                {
                    RecommendedVideos = null;
                }
            }
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
            Videos = new GridViewDataSet<VideoListDTO>()
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
        public async Task LoadVideo(VideoListDTO video)
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
