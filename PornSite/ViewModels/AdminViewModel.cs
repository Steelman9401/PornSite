using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Storage;
using DotVVM.Framework.ViewModel;
using HtmlAgilityPack;
using PornSite.Data;
using PornSite.DTO;
using PornSite.Repositories;
using PornSite.SupportClasses;

namespace PornSite.ViewModels
{
    public class AdminViewModel : MasterPageViewModel
    {
        public List<VideoDTO> Videos { get; set; } = new List<VideoDTO>();
        public bool EnableButton { get; set; } = false;
        public UploadedFilesCollection UploadedVideos { get; set; } = new UploadedFilesCollection();
        public ScrapperRepository scrapRep { get; set; } = new ScrapperRepository();
        public IEnumerable<string> Urls { get; set; }
        public VideoDTO Video { get; set; }
        public PornRepository PornRep { get; set; } = new PornRepository();
        public IEnumerable<string> Categories { get; set; }
        public IEnumerable<string> DatabaseCategories { get; set; }
        public List<WebCategory> WebCategories { get; set; } = new List<WebCategory>
        {
            new WebCategory() { Name = "Main", Url = "https://www.redtube.com"},
            new WebCategory() { Name = "Anal", Url = "https://www.redtube.com/redtube/anal"},
            new WebCategory() { Name = "Amateur", Url = "https://www.redtube.com/redtube/amateur"},
            new WebCategory() { Name = "Asian", Url = "https://www.redtube.com/redtube/asian"},
            new WebCategory() { Name = "Blonde", Url = "https://www.redtube.com/redtube/blonde"},
            new WebCategory() { Name = "Ebony", Url = "https://www.redtube.com/redtube/ebony"},
            new WebCategory() { Name = "Zrzky", Url = "https://www.redtube.com/redtube/redhead"},
            new WebCategory() { Name = "MILF", Url = "https://www.redtube.com/redtube/milf"},

        };
        public string SelectedWebCategory { get; set; } = "https://www.redtube.com";
        public bool ModalSwitch { get; set; }

        public async override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                DatabaseCategories = await PornRep.GetAllCategories();
                Urls = await PornRep.GetUrls();
                Videos = scrapRep.FillList(SelectedWebCategory, Urls);
            }

            await base.PreRender();
        }

        public void OpenModal(VideoDTO vid)
        {
            Video = vid;
            Categories = scrapRep.GetTags(vid);
            ModalSwitch = true;
        }
        public void CloseModal()
        {
            ModalSwitch = false;
        }

        public async Task AddVideo(VideoDTO vid)
        {
            var storage = Context.Configuration.ServiceLocator.GetService<IUploadedFileStorage>();
            var file = UploadedVideos.Files[0];
            var uploadPath = GetUploadPath();
            var targetPath = Path.Combine(uploadPath, file.FileId + ".mp4");
            storage.SaveAs(file.FileId, targetPath);
            storage.DeleteFile(file.FileId);
            vid.Url = "/PORN/" + file.FileId + ".mp4";
            await PornRep.AddPorn(vid, Categories);
            ModalSwitch = false;
            Videos.RemoveAll(x => x.Url == vid.Url);
            UploadedVideos.Clear();
        }
        public void ChangeCategoryList()
        {
            Videos = new List<VideoDTO>();
            Videos = scrapRep.FillList(SelectedWebCategory, Urls);
        }
        public void VideoUploaded()
        {
            EnableButton = true;
        }
        private string GetUploadPath()
        {
            var uploadPath = Path.Combine(Context.Configuration.ApplicationPhysicalPath, "PORN");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            return uploadPath;
        }
    }
}

