using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using PornSite.DTO;
using PornSite.Repositories;

namespace PornSite.ViewModels
{
    public class VideoViewModel : MasterPageViewModel
    {
        public VideoDTO Video { get; set; }
        public int Id { get; set; }
        public string VideoSrc { get; set; } = "https://embed.redtube.com/?id=";
        public override Task PreRender()
        {
            PornRepository rep = new PornRepository();
            Video = rep.GetVideoById(Convert.ToInt32(Context.Parameters["Id"]));
            VideoSrc = VideoSrc + Video.Url;
            return base.PreRender();
        }
        public override Task Init()
        {
            Id = Convert.ToInt32(Context.Parameters["Id"]);
            return base.Init(); 
        }
    }
}

