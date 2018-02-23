using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using PornSite.Repositories;

namespace PornSite.ViewModels
{
    public class MasterPageViewModel : DotvvmViewModelBase
    {
        public string Search { get; set; }
        public int Year { get; set; } = 2018;

        public async Task DoSearch()
        {
            if (!string.IsNullOrEmpty(Search))
            {
                Context.RedirectToRoute("search", new { text = new string(Search.ToArray()) });
            }
        }
    }
}
