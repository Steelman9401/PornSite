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
        public PornRepository rep { get; set; } = new PornRepository();
        public int Switch { get; set; } = 0;
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

        public void Test()
        {
            Switch = 1;
            Videos = new GridViewDataSet<VideoDTO>()
            {
                PagingOptions = { PageSize = 20 }
            };
        }

        public void Test2()
        {
            Switch = 0;
            Videos = new GridViewDataSet<VideoDTO>()
            {
                PagingOptions = { PageSize = 20 }
            };
        }

    }
}
