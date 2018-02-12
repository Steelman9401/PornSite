using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Hosting;
using PornSite.DTO;
using PornSite.Repositories;

namespace PornSite.ViewModels
{
    public class DefaultViewModel : MasterPageViewModel
    {
        public List<VideoDTO> Videos { get; set; }
        public List<VideoDTO> VideosByCat { get; set; }
        public string Title { get; set;}

		public DefaultViewModel()
		{
			Title = "Hello from DotVVM!";
            PornRepository rep = new PornRepository();
            VideosByCat = rep.GetVideosByCategory(97);
		}
    }
}
