using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeStore.Models.EfModels;
using Microsoft.AspNetCore.Identity;

namespace BikeStore.Models
{
    public class UserIdentityModel:IdentityUser<int>
    {
        public List<Order> Orders { get; set; }
    }
}
