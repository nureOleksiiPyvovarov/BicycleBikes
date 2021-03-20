using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeStore.Models.EfModels;
using BikeStore.Models.Repos.Interfaces;

namespace BikeStore.Models.Repos
{
    public class BrandsSqlRepo : IBrandsRepo
    {
        private readonly BikeStoresContext _context;

        public BrandsSqlRepo(BikeStoresContext context)
        {
            _context = context;
        }

        public void AddBrand(Brand brand)
        {
            if (brand == null)
                throw new ArgumentException();
            _context.Brands.Add(brand);
        }

        public Brand GetBrand(int id)
        {
            var brand = _context.Brands.FirstOrDefault(b => b.BrandId == id);
            if (brand == null)
                throw new ArgumentException("Invalid brand");
            return brand;
        }

        public Brand GetBrand(string brandName)
        {
            var brand = _context.Brands.FirstOrDefault(b => b.BrandName == brandName);
            if (brand == null)
                throw new ArgumentException("Invalid brand");
            return brand;
        }

        public IEnumerable<Brand> GetBrands() => _context.Brands.ToList();
    }
}
