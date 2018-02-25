using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.Controls;
using DotVVM.Framework.ViewModel;
using PornSite.Data;
using PornSite.DTO;
using PornSite.Repositories;

namespace PornSite.ViewModels
{
    public class VideoViewModel : MasterPageViewModel
    {
        public VideoDTO Video { get; set; }
        public int Id { get; set; }
        public IEnumerable<VideoDTO> SuggestedVideos { get; set; }
        public string ComText { get; set; }
        public int Switch { get; set; } = 0;
        public GridViewDataSet<CommentDTO> Comments { get; set; } = new GridViewDataSet<CommentDTO>()
        {
                PagingOptions = { PageSize = 3 }
        };
        PornRepository PornRep = new PornRepository();
        public CommentRepository ComRep { get; set; } = new CommentRepository();
        public async override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                var taskUpdate = PornRep.UpdateViews(Id);  ////vyresit nacitani z databaze, ted se nactou zbytecne oba
                Video = await PornRep.GetVideoById(Id);
            }
            else if (Comments.IsRefreshRequired)
            {
                Comments.OnLoadingData = option => ComRep.GetCommentsByVideoId(option, Id);
            }
            
            await base.PreRender();
        }
        public override Task Init()
        {
            Id = Convert.ToInt32(Context.Parameters["Id"]);
            return base.Init(); 
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
    }
}

