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

namespace PornSite.ViewModels
{
    public class DefaultViewModel : MasterPageViewModel
    {
        public GridViewDataSet<VideoDTO> Videos { get; set; }
        public PornRepository rep { get; set; } = new PornRepository();
        public int Switch { get; set; } = 0;
        public bool ShowVideo { get; set; } = false;
        public VideoDTO Video { get; set; } = new VideoDTO();
        public int Id { get; set; }
        public IEnumerable<VideoDTO> SuggestedVideos { get; set; }
        public string ComText { get; set; }
        public GridViewDataSet<CommentDTO> Comments { get; set; } = new GridViewDataSet<CommentDTO>()
        {
            PagingOptions = { PageSize = 3 }
        };
        PornRepository PornRep = new PornRepository();
        public CommentRepository ComRep { get; set; } = new CommentRepository();
        public DefaultViewModel()
		{
            
		}
        public override Task PreRender()
        {
            if (Switch == 0)
            {
                if (Videos.IsRefreshRequired || !Context.IsPostBack)
                {
                    Videos.OnLoadingData = option => rep.GetAllVideos(option);
                }
            }
            else
            {
                if (Videos.IsRefreshRequired || !Context.IsPostBack)
                {
                    Videos.OnLoadingData = option => rep.GetAllVideosByViews(option);
                }
            }
            return base.PreRender();
        }

        public override Task Init()
        {
            Videos = new GridViewDataSet<VideoDTO>()
            {
                PagingOptions = { PageSize = 20 }
            };
            return base.Init();
        }

        public void  LoadMostViewed()
        {
            Switch = 1;
            Videos = new GridViewDataSet<VideoDTO>()
            {
                PagingOptions = { PageSize = 20 }
            };
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
            List<int> Categories = Video.Categories.Select(x => x.Id).ToList();
            Switch = 0;
            PornRepository rep = new PornRepository();
            SuggestedVideos = await rep.GetSuggestedVideos(Categories);
        }

        public void GetComments()
        {
            Switch = 1;
            Comments.OnLoadingData = option => ComRep.GetCommentsByVideoId(option, Id);
        }

        public async Task AddComment()
        {
            CommentDTO comment = new CommentDTO();
            comment.Text = ComText;
            comment.User_Id = 1;
            comment.Video_Id = Id;
            comment.Username = "Tvuj koment";
            await ComRep.AddComment(comment);
            Comments.Items.Insert(0, comment);
        }

        public async Task LoadVideo(VideoDTO video)
        {
            //System.Threading.Thread.Sleep(3000);
            ShowVideo = true;
            Video = await PornRep.GetVideoById(video.Id);
        }

        public void HideVideo()
        {
            ShowVideo = false;
        }

    }
}
