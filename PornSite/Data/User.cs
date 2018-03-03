using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PornSite.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Admin { get; set; } = false;
        public virtual ICollection<Comment>Comments { get; set; }
    }
}