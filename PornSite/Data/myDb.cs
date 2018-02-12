using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PornSite.Data
{
    public class myDb:DbContext
    {
        public DbSet<Video> Videos { get; set; }
        public DbSet<Category> Categories { get; set; }
        public myDb() : base("PornDb")
        {


        }
    }
}