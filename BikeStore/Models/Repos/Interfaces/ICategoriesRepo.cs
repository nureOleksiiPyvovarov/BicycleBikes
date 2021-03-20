using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStore.Models.Repos
{
    public interface ICategoriesRepo
    {
        IEnumerable<Category> GetCategories();
        Category GetCategory(int id);
        Category GetCategory(string categoryName);
        void AddCategory(Category category);
        IEnumerable<Category> GetCategoriesForCategoryMenu();
    }
}
