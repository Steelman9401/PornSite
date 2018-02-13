﻿using System;
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
        public IEnumerable<VideoDTO> Videos { get; set; }
        public IEnumerable<VideoDTO> VideosByCat { get; set; }

		public DefaultViewModel()
		{
            
		}
        public override Task PreRender()
        {
            PornRepository rep = new PornRepository();
            // VideosByCat = rep.GetVideosByCategory(185);
            Videos = rep.GetAllVideos();
            return base.PreRender();
        }

    }
}
