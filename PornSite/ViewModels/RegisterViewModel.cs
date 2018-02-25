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
    public class RegisterViewModel : MasterPageViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool UserExistsError { get; set; } = false;
        public async Task AddUser()
        {
            UserRepository rep = new UserRepository();
            if (!rep.UserExists(Username))
            {
                await rep.AddUser(Username, Password);
            }
            else
            {
                UserExistsError = true;
            }
        }
    }
}

