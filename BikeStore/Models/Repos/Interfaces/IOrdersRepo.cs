using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeStore.Models.EfModels;

namespace BikeStore.Models.Repos.Interfaces
{
    public interface IOrdersRepo
    {
        void AddOrder(Order order);
        Order GetOrderById(int id);
        List<Order> GetOrdersList();
        bool SaveChanges();
        void DeleteOrder(int orderId);
    }
}
