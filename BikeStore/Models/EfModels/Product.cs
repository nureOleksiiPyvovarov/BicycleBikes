using BikeStore.Models.EfModels;
using System;
using System.Collections.Generic;

namespace BikeStore
{
    public partial class Product
    {
        public Product()
        { }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public short ModelYear { get; set; }
        public decimal ListPrice { get; set; }
        public string ProductPhoto { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Category Category { get; set; }
    }
}
