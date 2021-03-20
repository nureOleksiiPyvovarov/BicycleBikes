using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace BikeStore.Models.Repos
{
    public class ProductsSqlRepo : IProductsRepo
    {
        private readonly BikeStoresContext _context;
        public ProductsSqlRepo(BikeStoresContext context)
        {
            _context = context;
        }

        public Product CreateProduct(Product product)
        {
            if(product == null)
                throw new ArgumentException("Product is null");
            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public void DeleteProduct(int id)
        {
            var deletedProduct = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if(deletedProduct == null)
                throw new ArgumentException("Product with given id doesn't exist");
            _context.Products.Remove(deletedProduct);
            _context.SaveChanges();
        }

        public Product GetProductById(int id)
        {
            var product = _context.Products.Include(p => p.Category).Include(p => p.Brand)
                .FirstOrDefault(p => p.ProductId == id);
            if(product == null)
                throw new ArgumentException("Product with given id doesn't exist");
            return product;
        }

        public IEnumerable<Product> GetProducts(string searchString = null)
        {
            if (searchString == null)
            {
                var products = _context.Products.Include(p => p.Category).Include(p => p.Brand).ToList();
                return products;
            }
            var searchedProducts = _context.Products.Include(p => p.Category).Include(p => p.Brand).
                Where(b => b.ProductName.Contains(searchString));
            return searchedProducts;
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            if (_context.Categories.FirstOrDefault(c => c.CategoryName == category) == null)
                throw new ArgumentException("Invalid Category");
            return _context.Products.AsNoTracking().Include(p => p.Category).Include(p => p.Brand)
                .Where(p => p.Category.CategoryName == category);
        }

        public bool SaveChanges() => _context.SaveChanges() > 0;

        public void UpdateProduct(Product product)
        {
            var updatedProduct = _context.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
            if(updatedProduct == null)
                throw new ArgumentException("Product with given id doesn't exist");
            updatedProduct = product;
            _context.SaveChanges();
        }
    }
}
