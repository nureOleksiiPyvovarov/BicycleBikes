using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeStore.Models.Repos;
using Microsoft.AspNetCore.Mvc;

namespace BikeStore.Components
{
    public class CategoryMenu:ViewComponent
    {
        private readonly ICategoriesRepo _repo;

        public CategoryMenu(ICategoriesRepo repo)
        {
            _repo = repo;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _repo.GetCategoriesForCategoryMenu().OrderBy(c => c.CategoryName);
            return View(categories);
        }
    }
}
