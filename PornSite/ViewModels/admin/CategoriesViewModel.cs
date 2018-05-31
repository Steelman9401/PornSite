using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Runtime.Filters;
using DotVVM.Framework.ViewModel;
using PornSite.DTO;
using PornSite.Repositories.admin;

namespace PornSite.ViewModels.admin
{
    [Authorize()]
    public class CategoriesViewModel : AdminMasterPageViewModel
    {
        public GridViewDataSet<CategoryDTO> Categories { get; set; } = new GridViewDataSet<CategoryDTO>()
        {
            PagingOptions = { PageSize = 8 },
            RowEditOptions = new RowEditOptions() { PrimaryKeyPropertyName = "Id" }
        };
        public string ErrorMessage { get; set; } = null;
        public bool Success { get; set; }

        public async override Task PreRender()
        {
            AdminRepository AdminRep = new AdminRepository();
            Categories.OnLoadingData = option => AdminRep.GetAllCategories(option, CurrentUser);
            await base.PreRender();
        }

        public void Edit(CategoryDTO category)
        {
            Categories.RowEditOptions.EditRowId = category.Id;
        }

        public async Task Update(CategoryDTO category)
        {
            AdminRepository AdminRep = new AdminRepository();
            if (Validate(category))
            {
                await AdminRep.UpdateCategoryAsync(category);
            }
            Categories.RowEditOptions.EditRowId = null;

            // uncomment this line - it's here only for the sample to work without database
            //Customers.RequestRefresh(forceRefresh: true);
        }

        public void CancelEdit()
        {
            Categories.RowEditOptions.EditRowId = null;

            // uncomment this line - it's here only for the sample to work without database
            //Customers.RequestRefresh(forceRefresh: true);
        }
        private bool Validate(CategoryDTO category)
        {
            ValidationService ValService = new ValidationService();
            if (string.IsNullOrWhiteSpace(category.Name) || string.IsNullOrWhiteSpace(category.Name_en))
            {
                Success = false;
                ErrorMessage = "Nazev kategorie nemuze byt prazdny.";
                return false;
            }
            else if (ValService.CategoryExists(category))
            {
                ErrorMessage = "Kategorie s temito nazvy jiz existuje.";
                Success = false;
                return false;
            }
            else
            {
                Success = true;
                ErrorMessage = null;
                return true;
            }
        }
    }
}

