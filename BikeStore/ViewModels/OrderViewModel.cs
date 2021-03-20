using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeStore.Models;
using BikeStore.Models.EfModels;

namespace BikeStore.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<OrderProduct> Products { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsCompleted { get; set; }
    }
}
