using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStore.Models.EfModels
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserIdentityModel User { get; set; }
        public List<OrderProduct> Products { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsCompleted { get; set; }
    }
}
