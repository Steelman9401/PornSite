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
    public class SearchViewModel : MasterPageViewModel
    {
        public GridViewDataSet<VideoDTO> Videos { get; set; }
        public PornRepository rep { get; set; } = new PornRepository();
        public string SearchQ { get; set; }
        public override Task Init()
        {
            SearchQ = Context.Parameters["text"].ToString();
            Videos = new GridViewDataSet<VideoDTO>()
            {
                PagingOptions = { PageSize = 20 }
            };
            return base.Init();
        }

        public override Task PreRender()
        {
            if (Videos.IsRefreshRequired || !Context.IsPostBack)
            {
                Videos.OnLoadingData = option => rep.GetSearchResult(option, SearchQ.ToLower());
            }
            return base.PreRender();
        }
    }
}

