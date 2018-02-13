using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PornSite.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<VideoDTO> Videos { get; set; }
    }
}