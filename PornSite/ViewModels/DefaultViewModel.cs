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
        public int LoadSwitch { get; set; } = 1;
        public DefaultViewModel()
		{
            
		}
        public override Task PreRender()
        {
            LoadSwitch = 1;
            return base.PreRender();
        }

        public override Task Init()
        {   
            Videos = GridViewDataSet.Create(gridViewDataSetLoadDelegate: rep.GetAllVideos, pageSize: 20);
            return base.Init();
        }

    }
}
