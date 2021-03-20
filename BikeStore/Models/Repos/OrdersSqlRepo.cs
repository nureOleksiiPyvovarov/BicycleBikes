using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeStore.Models.EfModels;
using BikeStore.Models.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BikeStore.Models.Repos
{
    public class OrdersSqlRepo : IOrdersRepo
    {
        private readonly BikeStoresContext _context;
        private readonly ShoppingCart _cart;

        public OrdersSqlRepo(BikeStoresContext context, ShoppingCart cart)
        {
            _context = context;
            _cart = cart;
        }
        public void AddOrder(Order order)
        {
            if (order == null)
            {
                throw new ArgumentException("Order doesn't exist");
            }

            if (_cart.Items == null)
            {
                throw new ArgumentException("Order doesn't contain any product");
            }
            var shoppingCartItems = _cart.Items;
            order.CreatedAt = DateTime.Now;
            order.IsCompleted = false;
            _context.Orders.Add(order);
            foreach (var shoppingCartItem in shoppingCartItems)
            {
                var orderProduct = new OrderProduct()
                {
                    OrderId = order.Id,
                    Order = order,
                    ProductId = shoppingCartItem.Product.ProductId,
                    Product = shoppingCartItem.Product,
                    Price = (double)shoppingCartItem.Product.ListPrice,
                    Total = shoppingCartItem.Amount
                };

                _context.OrderProducts.Add(orderProduct);
            }
            SaveChanges();
        }

        public Order GetOrderById(int id)
        {
            var order = _context.Orders.Include(o => o.Products).Include(o => o.User).FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                throw new ArgumentException("Order doesn't exist");
            }

            return order;
        }

        public List<Order> GetOrdersList() => _context.Orders.Include(o => o.User).Include(o => o.Products)
            .ThenInclude(p => p.Product)
            .ToList();

        public void DeleteOrder(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                throw new ArgumentException("Order doesn't exist");
            }
            _context.Orders.Remove(order);
            SaveChanges();
        }
        public bool SaveChanges() => _context.SaveChanges() > 0;
    }
}
