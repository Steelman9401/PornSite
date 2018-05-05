using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PornSite.DTO
{
    public class VideoListDTO
    {
        public int Id { get; set; }
        public string Img { get; set; }
        public string Title { get; set; }
        public string Preview { get; set; }
        public string Duration { get; set; } 
        public int Views { get; set; }
        public bool HD { get; set; }
        public long TimeStamp { get; set; }
    }
}