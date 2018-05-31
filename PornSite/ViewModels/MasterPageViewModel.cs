using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
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
        public string currentCulture { get; set; }
        public override Task Init()
        {
            if (!Context.IsPostBack)
            {
                if (HttpContext.Current.Request.Cookies["Language"] == null)
                {
                    HttpCookie cookie = new HttpCookie("Language");
                    try
                    {
                        cookie.Value = HttpContext.Current.Request.UserLanguages[0];
                    }
                    catch
                    {
                        cookie.Value = "en-US";
                    }
                    if (cookie.Value.Contains("cs"))
                    {
                        cookie.Value = "cs-CZ";
                    }
                    else
                    {
                        cookie.Value = "en-US";
                    }
                    currentCulture = cookie.Value;
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentCulture);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(currentCulture);
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
                else
                {
                    currentCulture = HttpContext.Current.Request.Cookies["Language"].Value;
                    try
                    {
                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentCulture);
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo(currentCulture);
                    }
                    catch
                    {
                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                    }
                }
            }
            return base.Init();
        }
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
        public void SwitchToCzech()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("cz-CS");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("cz-CZ");
            HttpContext.Current.Response.Cookies["Language"].Value = "cz-CZ";

        }
        public void SwitchToEnglish()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            HttpContext.Current.Response.Cookies["Language"].Value = "en-US";

        }
    }
}
