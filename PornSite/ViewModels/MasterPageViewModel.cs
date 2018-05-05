using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.ViewModel;
using Microsoft.AspNet.Identity;
using PornSite.DTO;
using PornSite.Repositories;

namespace PornSite.ViewModels
{
    public class MasterPageViewModel : DotvvmViewModelBase
    {
        public string Search { get; set; }
        public long TimeStamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        public override Task PreRender()
        {
            return base.PreRender();
        }
        public async Task DoSearch()
        {
            if (!string.IsNullOrEmpty(Search))
            {
                Context.RedirectToRoute("search", new { text = new string(Search.ToArray()) });
            }
        }
    }
}
