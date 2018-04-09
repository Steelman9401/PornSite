using PornSite.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PornSite.DTO
{
    public class VideoAdminDTO:IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "• Je třeba zadat český název")]
        public string Title { get; set; }
        [Required(ErrorMessage = "• Je třeba zadat anglický název")]
        public string Title_en { get; set; }
        public string Img { get; set; }
        public IEnumerable<string> DatabaseCategories { get; set; } = new List<string>();
        public bool Success { get; set; }
        public bool ShowCategoryOption { get; set; }
        public string Url { get; set; }
        public bool IsCustom { get; set; }
        public string Duration { get; set; }
        public bool HD { get; set; }
        public string Description { get; set; }
        public string Preview { get; set; }
        public IEnumerable<string> Categories { get; set; } = new List<string>();

        public void AddCategory()
        {
            List<string> newCat = new List<string>();
            string cat = "";
            try
            {
                newCat = Categories.ToList();
            }
            catch
            {
                List<string> list = new List<string>();
                Categories = list;
                
            }
            newCat.Add(cat);
            Categories = newCat;
        }
        public void RemoveCategory(string cat)
        {
            var newCat = Categories.ToList();
            newCat.Remove(cat);
            Categories = newCat;
        }
        public async Task AddVideo()
        {
            AdminRepository AdminRep = new AdminRepository();
            await AdminRep.AddPorn(this);
            Success = true;
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
      
        public static class LinqExtras // Or whatever
        {
            public static bool ContainsAllItems<T>(IEnumerable<T> a, IEnumerable<T> b)
            {
                return !b.Except(a).Any();
            }
        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            AdminRepository AdminRep = new AdminRepository();
            List<ValidationResult> errormsg = new List<ValidationResult>();
            if (Categories.ToList().Count < 3)
            {
                errormsg.Add(new ValidationResult("• Je třeba mít zadané alespoň 3 kategorie.", new[] { nameof(Categories) }));
            }
            if (Categories.GroupBy(x => x)
                        .Where(group => group.Count() > 1).Count() != 0)
            {
                errormsg.Add(new ValidationResult("• Kategorie se nesmí jmenovat stejně.", new[] { nameof(Categories) }));
            }

            if (!LinqExtras.ContainsAllItems(DatabaseCategories, Categories))
            {
                errormsg.Add(new ValidationResult("• Kategorie není v databázi.", new[] { nameof(Categories) }));
            }
            if (AdminRep.VideoExists(Url))
            {
                errormsg.Add(new ValidationResult("• Video již existuje.", new[] { nameof(Categories) }));
            }
            return errormsg;
        }
    }
}