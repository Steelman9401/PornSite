using DotVVM.Framework.Controls;
using PornSite.Data;
using PornSite.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PornSite.DTO
{
    public class VideoDetailDTO
    {
        public int Id { get; set; }
        public string Img { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public int Views { get; set; }
        public IEnumerable<CategoryDTO> Categories{ get; set; }
        public IEnumerable<VideoListDTO> SuggestedVideos { get; set; }
        public GridViewDataSet<CommentDTO> Comments { get; set; } = new GridViewDataSet<CommentDTO>()
        {
            PagingOptions = { PageSize = 3 }
        };

        public async Task LoadSuggestedVideos()
        {
            PornRepository rep = new PornRepository();
            SuggestedVideos = await rep.GetSuggestedVideos(Categories.Select(x => x.Id).ToList(),this.Id);
        }
    }
}