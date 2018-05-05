using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PornSite.Data
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Name_en { get; set; }
        public virtual User User { get; set; }
        public string Img { get; set; }
        public virtual ICollection<Video> Videos { get; set; }
    }
}