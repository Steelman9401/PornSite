using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using PornSite.DTO;
using PornSite.Repositories;

namespace PornSite.ViewModels
{
    public class MasterPageViewModel : DotvvmViewModelBase
    {
        public string Search { get; set; }
        public bool ShowLogin { get; set; } = false;
        public int Year { get; set; } = 2018;
        public string Username { get; set; }
        public string Password { get; set; }
        public UserDTO LoggedUser { get; set; }
        public async Task DoSearch()
        {
            if (!string.IsNullOrEmpty(Search))
            {
                Context.RedirectToRoute("search", new { text = new string(Search.ToArray()) });
            }
        }
        public void ShowLoginModal()
        {
            ShowLogin = true;
        }
        public async Task SignIn()
        {
            UserRepository rep = new UserRepository();
            LoggedUser = await rep.UserLogin(Username, Password);
            if(LoggedUser!=null)
            {
               
            }
            int a = 5;
        }
    }
}
