using DotVVM.Framework.Controls;
using PornSite.Data;
using PornSite.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PornSite.DTO
{
    public class VideoDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Img { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public int Views { get; set; }
        public string Preview { get; set; }
        public int Index { get; set; }
        public IEnumerable<CategoryDTO> Categories { get; set; }
        public GridViewDataSet<CommentDTO> Comments { get; set; } = new GridViewDataSet<CommentDTO>()
        {
            PagingOptions = { PageSize = 3 }
        };
        public IEnumerable<VideoDTO> SuggestedVideos { get; set; }
        public void GetComments()
        {
            CommentRepository ComRep = new CommentRepository();
            Comments.OnLoadingData = option => ComRep.GetCommentsByVideoId(option, this.Id);
        }

        public async Task GetSuggestedVideos()
        {
            PornRepository rep = new PornRepository();
            SuggestedVideos = await rep.GetSuggestedVideos(Categories.Select(x=>x.Id).ToList());
        }

        public async Task AddComment(string ComText, int UserId, string username)
        {
            CommentRepository ComRep = new CommentRepository();
            CommentDTO comment = new CommentDTO();
            comment.Text = ComText;
            comment.User_Id = UserId;
            comment.Video_Id = this.Id;
            comment.Username = username;
            await ComRep.AddComment(comment);
            Comments.Items.Insert(0, comment);
        }
    }
}