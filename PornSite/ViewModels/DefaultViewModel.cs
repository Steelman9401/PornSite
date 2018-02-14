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
        public bool LoadDataCheck { get; set; } = true;
        public DefaultViewModel()
		{
            
		}
        public override Task PreRender()
        {
            return base.PreRender();
        }

        public override Task Init()
        {   if (LoadDataCheck)
            {
                Videos = GridViewDataSet.Create(gridViewDataSetLoadDelegate: rep.GetAllVideos, pageSize: 4);
            }
            else
            {
                LoadDataCheck = true;
            }
            return base.Init();
        }

        public void DeleteAll()
        {
            Videos = null;
            LoadDataCheck = false;
        }
        public void LoadVideos()
        {
            Videos = GridViewDataSet.Create(gridViewDataSetLoadDelegate: rep.GetAllVideos, pageSize: 4);
        }

    }
}
