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
        public bool ShowLogin { get; set; } = false;
        public int Year { get; set; } = 2018;
        public string Username { get; set; }
        public string Password { get; set; }
        public string CurrentUser {
            get { return Context.GetOwinContext().Authentication.User.Identity.Name; }  
        }
        public string CurrentUserId { get; set; }
        public override Task PreRender()
        {

            return base.PreRender();
        }
        public async Task DoSearch()
        {
            if (!string.IsNullOrEmpty(Search))
            {
                Context.RedirectToRoute("Default", new { text = new string(Search.ToArray()) });
            }
        }
        public void ShowLoginModal()
        {
            ShowLogin = true;
        }
        public async Task SignIn()
        {
            UserRepository rep = new UserRepository();
            CurrentUserId = await rep.CheckUser(Username, Password);
            if (CurrentUserId!= "")
            {
                ShowLogin = false;
                var identity = CreateIdentity(Username,CurrentUserId);
                Context.GetAuthentication().SignIn(identity);
                Context.RedirectToUrl("");
            }
            else
            {
                //error
            }
        }

        private ClaimsIdentity CreateIdentity(string username, string Id)
        {
            if (Id.Substring(0, 1) == "A")
            {
                CurrentUserId = Id.Substring(1, Id.Length - 1);
                var identity = new ClaimsIdentity(
                    new[]
                    {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier,CurrentUserId),
                new Claim(ClaimTypes.Role, "administrator"),
                    },
                    DefaultAuthenticationTypes.ApplicationCookie);
                return identity;
            }
            else
            {
                var identity = new ClaimsIdentity(
                   new[]
                   {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier,Id),
                   },
                DefaultAuthenticationTypes.ApplicationCookie);
                return identity;
            }
        }
    }
}
