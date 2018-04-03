using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PornSite.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Kategorie nemuze byt prazdna")]
        public string Name { get; set; }
    }
}