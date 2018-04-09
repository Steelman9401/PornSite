using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PornSite.DTO
{
    public class VideoListAdminDTO
    {
        public int Id { get; set; }
        public string Img { get; set; }
        public string Title { get; set; }
        public string Preview { get; set; }
        public string Duration { get; set; }
        public bool HD { get; set; }
        public string Title_en { get; set; }
        public string Url { get; set; }
    }
}