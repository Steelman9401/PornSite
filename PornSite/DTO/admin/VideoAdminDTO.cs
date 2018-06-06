using PornSite.Repositories.admin;
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
        public string Title { get; set; }
        public string Title_en { get; set; }
        public string Img { get; set; }
        public bool CategoryAdded { get; set; }
        public string ErrorMessage { get; set; } = "";
        public IEnumerable<string> DatabaseCategories { get; set; } = new List<string>();
        public bool Success { get; set; }
        public bool AllowMain { get; set; } = true;
        public bool ShowCategoryOption { get; set; }
        public bool CheckBoxEnabled { get; set; } = true;
        public CategoryDTO NewCategory { get; set; } = new CategoryDTO();
        public string Url { get; set; }
        public bool LoadError { get; set; }
        public bool IsCustom { get; set; }
        public bool IsEdited { get; set; }
        public string Duration { get; set; }
        public bool HD { get; set; }
        public string Description { get; set; }
        public string Preview { get; set; }
        public IEnumerable<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();

        public void AddCategory()
        {
            List<CategoryDTO> newCat = new List<CategoryDTO>();
            CategoryDTO cat = new CategoryDTO();
            try
            {
                newCat = Categories.ToList();
            }
            catch
            {
                List<CategoryDTO> list = new List<CategoryDTO>();
                Categories = list;

            }
            newCat.Add(cat);
            Categories = newCat;
        }
        public void RemoveCategory(CategoryDTO cat)
        {
            var newCat = Categories.ToList();
            newCat.Remove(cat);
            Categories = newCat;
        }
        public async Task AddVideo(string username)
        {
            AdminRepository AdminRep = new AdminRepository();
            await AdminRep.AddVideoAsync(this, username);
            Success = true;
        }
        public async Task<bool> AddCategoryToDatabase(string username)
        {
            if (ValidateCategory())
            {
                AdminRepository AdminRep = new AdminRepository();
                await AdminRep.AddCategoryAsync(NewCategory, username);
                CategoryAdded = true;
                ErrorMessage = "";
                return true;
            }
            else
                CategoryAdded = false;
            return false;
        }
        public async Task UpdateVideo()
        {
            AdminRepository AdminRep = new AdminRepository();
            await AdminRep.UpdateVideoAsync(this);
            Success = true;
        }

        public static class LinqExtras // Or whatever
        {
            public static bool ContainsAllItems<T>(IEnumerable<T> a, IEnumerable<T> b)
            {
                return !b.Except(a).Any();
            }
        }
        private IEnumerable<string> CheckCategories()
        {
            return Categories.Where(p => !DatabaseCategories.Any(p2 => p2 == p.Name)).ToList().Select(o => o.Name);
        }
        private bool ValidateCategory()
        {
            AdminRepository AdminRep = new AdminRepository();
            if (string.IsNullOrWhiteSpace(NewCategory.Name) || string.IsNullOrWhiteSpace(NewCategory.Name_en))
            {
                ErrorMessage = "Název kategorie nemůže být prázdný.";
                return false;
            }
            else if (AdminRep.CategoryExists(NewCategory))
            {
                ErrorMessage = "Kategorie již existuje.";
                return false;
            }
            else
            {
                return true;
            }
        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errormsg = new List<ValidationResult>();
            AdminRepository AdminRep = new AdminRepository();
            if (string.IsNullOrEmpty(Title))
            {
                Success = false;
                errormsg.Add(new ValidationResult("Je třeba zadat český název.", new[] { nameof(Title) }));
            }
            if (string.IsNullOrEmpty(Title_en))
            {
                Success = false;
                errormsg.Add(new ValidationResult("Je třeba zadat anglický název.", new[] { nameof(Title_en) }));
            }
            if (Categories.ToList().Count < 3 && !AllowLessCategories())
            {
                Success = false;
                errormsg.Add(new ValidationResult("Je třeba mít zadané alespoň 3 kategorie.", new[] { nameof(Categories) }));
            }
            if (Categories.GroupBy(x => x.Name)
                        .Where(group => group.Count() > 1).Count() != 0)
            {
                Success = false;
                errormsg.Add(new ValidationResult("Kategorie se nesmí jmenovat stejně.", new[] { nameof(Categories) }));
            }
            if (Categories.Where(x => x.Name == null).Count() != 0)
            {
                errormsg.Add(new ValidationResult("Název kategorie nesmí být prázdný.", new[] { nameof(Title) }));
            }
            if (!IsEdited && AdminRep.VideoExists(Url))
            {
                Success = false;
                errormsg.Add(new ValidationResult("Video již existuje.", new[] { nameof(Categories) }));
            }
            return errormsg;
        }
        private bool AllowLessCategories()
        {
            if (Categories.Any(x => x.Name == "Gay" || x.Name == "Hentai"))
                return true;
            return false;
        }
    }
}