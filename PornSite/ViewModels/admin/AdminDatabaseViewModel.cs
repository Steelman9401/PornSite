using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.Controls;
using DotVVM.Framework.ViewModel;
using PornSite.DTO;
using PornSite.Repositories;

namespace PornSite.ViewModels.admin
{
    public class AdminDatabaseViewModel : PornSite.ViewModels.MasterPageViewModel
    {
        public GridViewDataSet<VideoDTO> Videos { get; set; }
        public PornRepository Rep { get; set; } = new PornRepository();
        public VideoDTO Video { get; set; }
        public bool ModalSwitch { get; set; } = false;
        public override Task Init()
        {
            Videos = GridViewDataSet.Create(gridViewDataSetLoadDelegate: Rep.GetAllVideosAdmin, pageSize: 20);
            return base.Init();
        }
        public void RemoveVideo(VideoDTO video)
        {
            Task.Run(() => this.Rep.RemoveVideo(video.Id));
            Videos.Items.Remove(video);
        }
        public void EditVideo(VideoDTO vid)
        {
            Video = vid;
            ModalSwitch = true;
        }
    }
}

