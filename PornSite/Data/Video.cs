using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PornSite.Data
{
    public class Video
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Title_en { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public int Views { get; set; }
        public string Duration { get; set; }
        public bool HD { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Img { get; set; }
        public string Preview { get; set; }
        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
        public virtual ICollection<Comment> Comments { get; set; }
    }
}