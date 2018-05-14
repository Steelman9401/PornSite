using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using PornSite.DTO;
using PornSite.Repositories;

namespace PornSite.ViewModels
{
    public class CategoriesViewModel : MasterPageViewModel
    {
        public IEnumerable<CategoryDTO> Categories { get; set; }
        public async override Task PreRender()
        {
            PornRepository PornRep = new PornRepository();
            Categories = await PornRep.GetAllCategoriesAsync();
            await base.PreRender();
        }
    }
}
