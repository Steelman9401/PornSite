using DotVVM.Framework.Controls;
using PornSite.Repositories.admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PornSite.DTO
{
    public class VideoListAdminDTO
    {
        public int Id { get; set; }
        public string Img { get; set; }
        public string Title { get; set; }
        public string Preview { get; set; }
        public string Duration { get; set; }
        public bool HD { get; set; }
        public string Title_en { get; set; }
        public string Url { get; set; }

        public async Task RemoveVideo(GridViewDataSet<VideoListAdminDTO> Videos)
        {
            AdminRepository AdminRep = new AdminRepository();
            var deletedVideo = Videos.Items.Where(x => x.Id == Id).First();
            Videos.Items.Remove(deletedVideo);
            await AdminRep.RemoveVideoAsync(this.Id);
        }
    }
}