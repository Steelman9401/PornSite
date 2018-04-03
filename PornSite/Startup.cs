using System;
using System.Web.Hosting;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using DotVVM.Framework;
using DotVVM.Framework.Configuration;
using DotVVM.Framework.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(PornSite.Startup))]
namespace PornSite
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/"),       // don't use ~/login here - the ~ in URLs is a DotVVM feature, OWIN security doesn't know it
                Provider = new CookieAuthenticationProvider()
                {
                    OnApplyRedirect = e => DotvvmAuthenticationHelper.ApplyRedirectResponse(e.OwinContext, e.RedirectUri)
                }
            });
            var applicationPhysicalPath = HostingEnvironment.ApplicationPhysicalPath;

            ConfigureAuth(app);



            // use DotVVM
            var dotvvmConfiguration = app.UseDotVVM<DotvvmStartup>(applicationPhysicalPath, debug: IsInDebugMode(), options: options =>
            {
                options.AddDefaultTempStorages("temp");
                options.AddUploadedFileStorage("App_Data/Temp");
            });


            // use static files
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileSystem = new PhysicalFileSystem(applicationPhysicalPath)
            });
            app.Run(context =>
            {
                context.Response.Redirect("/");
                return Task.FromResult(0);
            });
        }
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Authentication/SignIn"),
                Provider = new CookieAuthenticationProvider()
                {
                    OnApplyRedirect = context =>
                    {
                        DotvvmAuthenticationHelper.ApplyRedirectResponse(context.OwinContext, context.RedirectUri);
                    }
                }
            });
        }

		private bool IsInDebugMode()
        {
#if !DEBUG
			return false;
#endif
            return true;
        }
    }
}
