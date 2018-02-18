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
    public class VideoViewModel : MasterPageViewModel
    {
        public VideoDTO Video { get; set; }
        public int Id { get; set; }
        public IEnumerable<VideoDTO> SuggestedVideos { get; set; }
        public int LoadSwitch { get; set; } = 0;
        PornRepository rep = new PornRepository();
        public override Task PreRender()
        {
            Task.Run(() => this.UpdateViews());
            Video = rep.GetVideoById(Convert.ToInt32(Context.Parameters["Id"]));
            return base.PreRender();
        }
        private async Task UpdateViews()
        {
            await rep.UpdateViews(Id);
        }
        public override Task Init()
        {
            Id = Convert.ToInt32(Context.Parameters["Id"]);
            return base.Init(); 
        }        
        public void GetSuggestions()
        {
            List<int> Categories = Video.Categories.Select(x => x.Id).ToList();
            LoadSwitch = 1;
            System.Threading.Thread.Sleep(3000);
            PornRepository rep = new PornRepository();
            SuggestedVideos = rep.GetSuggestedVideos(Categories);
        }
    }
}

