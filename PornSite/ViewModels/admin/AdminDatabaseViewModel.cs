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
        public AdminRepository AdminRep { get; set; } = new AdminRepository();
        public GridViewDataSet<VideoAdminDTO> Videos { get; set; } = new GridViewDataSet<VideoAdminDTO>()
        {
            PagingOptions = { PageSize = 20 }
        };
        public VideoAdminDTO Video { get; set; }
        public bool ModalSwitch { get; set; } = false;
        public override Task PreRender()
        {
            Videos.OnLoadingDataAsync = option => AdminRep.GetAllVideosAdmin(option);
            return base.PreRender();
        }
        public async Task EditVideo(VideoAdminDTO vid)
        {            
            ModalSwitch = true;
            Video = await AdminRep.GetVideoById(vid.Id);
        }
        public void CloseVideo()
        {
            ModalSwitch = false;
        }
    }
}

