using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.Controls;
using DotVVM.Framework.ViewModel;
using PornSite.DTO;
using PornSite.Repositories;

namespace PornSite.ViewModels
{
    public class ViewsViewModel : MasterPageViewModel
    {
        public GridViewDataSet<VideoDTO> Videos { get; set; }
        public PornRepository rep { get; set; } = new PornRepository();

        public override Task Init()
        {
            Videos = GridViewDataSet.Create(gridViewDataSetLoadDelegate: rep.GetAllVideosByViews, pageSize: 4);
            return base.Init();
        }
    }
}

