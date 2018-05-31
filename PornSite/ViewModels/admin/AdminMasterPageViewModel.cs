using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.ViewModel;

namespace PornSite.ViewModels.admin
{
    public class AdminMasterPageViewModel : DotvvmViewModelBase
    {
        public string CurrentUser
        {
            get { return Context.GetOwinContext().Authentication.User.Identity.Name; }
        }
        public override Task PreRender()
        {

            return base.PreRender();
        }

        public void SignOut()
        {
            Context.GetOwinContext().Authentication.SignOut();
            Context.RedirectToRoute("Default");
        }
    }
}

