using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.ViewModel;
using Microsoft.AspNet.Identity;
using PornSite.Repositories.admin;

namespace PornSite.ViewModels.admin
{
    public class AdminViewModel : DotvvmViewModelBase
    {
        [Required(ErrorMessage = "Uživatelské jméno nemùže být prázdné!")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Heslo nemùže být prázdné!")]
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
        public async Task SignIn()
        {
            ValidationService ValRep = new ValidationService();
            if (await ValRep.UserExist(Username, Password))
            {
                var identity = CreateIdentity();
                Context.GetAuthentication().SignIn(identity);
                string test = Context.GetOwinContext().Authentication.User.Identity.Name;
                Context.RedirectToUrl("Spider");
            }
            else
            {
                ErrorMessage = "Nesprávné údaje!";
            }
        }

        private ClaimsIdentity CreateIdentity()
        {
            var identity = new ClaimsIdentity(
                new[]
                {
                new Claim(ClaimTypes.Name, Username),
                new Claim(ClaimTypes.NameIdentifier,Username),
                new Claim(ClaimTypes.Role, "administrator"),
                },
                DefaultAuthenticationTypes.ApplicationCookie);
            return identity;

        }
    }
}

