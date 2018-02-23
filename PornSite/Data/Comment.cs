using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PornSite.Data
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public virtual User User { get; set; } = new User();
        public virtual Video Video { get; set; } = new Video();
    }
}