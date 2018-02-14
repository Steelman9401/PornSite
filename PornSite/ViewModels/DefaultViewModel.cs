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

namespace PornSite.ViewModels
{
    public class DefaultViewModel : MasterPageViewModel
    {
        public IEnumerable<VideoDTO> Videos { get; set; }
        public IEnumerable<VideoDTO> VideosByCat { get; set; }
        public PornRepository rep { get; set; } = new PornRepository();
        public DefaultViewModel()
		{
            
		}
        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                // VideosByCat = rep.GetVideosByCategory(185);
                Videos = rep.GetAllVideos();
            }
            return base.PreRender();
        }

        public void DeleteAll()
        {
            Videos = null;
        }
        public void LoadVideos()
        {
            Videos = rep.GetAllVideos();
        }

    }
}
