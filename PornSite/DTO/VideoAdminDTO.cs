using PornSite.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PornSite.DTO
{
    public class VideoAdminDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Název musí být vyplněn.")]
        public string Title { get; set; }
        public string Img { get; set; }
        public bool Error { get; set; }
        public bool Success { get; set; }
        public string Url { get; set; }
        public string Duration { get; set; }
        public bool HD { get; set; }
        public string Description { get; set; }
        public string Preview { get; set; }
        public IEnumerable<CategoryDTO> Categories { get; set; }

        public void AddCategory()
        {
            CategoryDTO cat = new CategoryDTO();
            var newCat = Categories.ToList();
            newCat.Add(cat);
            Categories = newCat;
        }
        public void RemoveCategory(CategoryDTO cat)
        {
            var newCat = Categories.ToList();
            newCat.Remove(cat);
            Categories = newCat;
        }
        public async Task<bool> AddVideo()
        {
            AdminRepository AdminRep = new AdminRepository();
            if (Categories.ToList().Count > 2)
            {
                await AdminRep.AddPorn(this);
                Success = true;
                Error = false;
                return true;
            }
            else
            {
                Success = false;
                Error = true;
                return false;
            }
        }
        public async Task RemoveVideo()
        {
            AdminRepository AdminRep = new AdminRepository();
            await AdminRep.RemoveVideo(this.Id);
        }
        public async Task UpdateVideo()
        {
            AdminRepository AdminRep = new AdminRepository();
            await AdminRep.UpdateVideo(this);
        }
    }
}