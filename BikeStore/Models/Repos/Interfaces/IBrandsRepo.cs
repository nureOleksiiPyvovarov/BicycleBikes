using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeStore.Models.EfModels;

namespace BikeStore.Models.Repos.Interfaces
{
    public interface IBrandsRepo
    {
        IEnumerable<Brand> GetBrands();
        Brand GetBrand(int id);
        Brand GetBrand(string brandName);
        void AddBrand(Brand brand);
    }
}
