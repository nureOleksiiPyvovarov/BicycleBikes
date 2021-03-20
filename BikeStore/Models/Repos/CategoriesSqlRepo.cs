using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BikeStore.Models.Repos
{
    public class CategoriesSqlRepo : ICategoriesRepo
    {
        private readonly BikeStoresContext _context;
        public CategoriesSqlRepo(BikeStoresContext context)
        {
            _context = context;
        }

        public void AddCategory(Category category)
        {
            if (category == null)
                throw new ArgumentException();
            _context.Categories.Add(category);
        }

        public IEnumerable<Category> GetCategoriesForCategoryMenu() => _context.Categories.Include(c => c.Products).ToList();

        public IEnumerable<Category> GetCategories() => _context.Categories.ToList(); 

        public Category GetCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            if(category == null)
                throw new ArgumentException("Invalid category");
            return category;
        }

        public Category GetCategory(string categoryName)
        {
            var category = _context.Categories.Include(c => c.Products).FirstOrDefault(c => c.CategoryName == categoryName);
            if (category == null)
                throw new ArgumentException("Invalid category");
            return category;
        }
    }
}
