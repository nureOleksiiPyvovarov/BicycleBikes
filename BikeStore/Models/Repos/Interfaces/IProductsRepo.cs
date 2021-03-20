using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStore.Models.Repos
{
    public interface IProductsRepo
    {
        Product GetProductById(int id);
        IEnumerable<Product> GetProducts(string searchString = null);
        IEnumerable<Product> GetProductsByCategory(string category);
        Product CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
        bool SaveChanges();
    }
}
