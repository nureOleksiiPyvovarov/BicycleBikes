using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeStore.Models.EfModels;
using Microsoft.AspNetCore.Mvc;

namespace BikeStore.ViewModels
{
    public class BikeCardViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public short ModelYear { get; set; }
        public decimal ListPrice { get; set; }
        public string ProductPhoto { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
    }
}
