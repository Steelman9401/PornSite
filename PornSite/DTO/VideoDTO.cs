using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PornSite.DTO
{
    public class VideoDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Img { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public int Views { get; set; }
        public string Preview { get; set; }
        public int Index { get; set; }
        public IEnumerable<CategoryDTO> Categories { get; set; }
    }
}