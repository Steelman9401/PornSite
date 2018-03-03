using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Storage;
using DotVVM.Framework.ViewModel;

namespace PornSite.ViewModels
{
    public class UploadTestViewModel : MasterPageViewModel
    {
        public UploadedFilesCollection UploadedVideos { get; set; } = new UploadedFilesCollection();
        
        public void SaveVideo()
        {
            var storage = Context.Configuration.ServiceLocator.GetService<IUploadedFileStorage>();
            foreach (var file in UploadedVideos.Files)
            {
                var uploadPath = GetUploadPath();
                    var targetPath = Path.Combine(uploadPath, file.FileName);
                storage.SaveAs(file.FileId, targetPath);
                storage.DeleteFile(file.FileId);
            }
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
