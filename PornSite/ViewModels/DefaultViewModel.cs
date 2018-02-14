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

namespace PornSite.ViewModels
{
    public class DefaultViewModel : MasterPageViewModel
    {
        public GridViewDataSet<VideoDTO> Videos { get; set; }       
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
                //rep.GetAllVideos(Videos);
            }
            return base.PreRender();
        }

        public override Task Init()
        {
            Videos = GridViewDataSet.Create(gridViewDataSetLoadDelegate: rep.GetAllVideos, pageSize: 4);
            return base.Init();
        }

        public void DeleteAll()
        {
            //Videos = new GridViewDataSet<VideoDTO>();
        }
        public void LoadVideos()
        {
            //rep.GetAllVideos(Videos);
        }

    }
}
