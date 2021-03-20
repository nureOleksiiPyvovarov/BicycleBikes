using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BikeStore.Models;
using BikeStore.Models.EfModels;
using BikeStore.Models.Repos;
using BikeStore.Models.Repos.Interfaces;
using BikeStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BikeStore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly UserManager<UserIdentityModel> _userManager;
        private readonly IOrdersRepo _ordersRepo;
        private readonly ShoppingCart _cart;
        private readonly IMapper _mapper;

        public OrdersController(UserManager<UserIdentityModel> userManager,
                                IOrdersRepo _ordersRepo, IProductsRepo _productsRepo,
                                ShoppingCart cart, IMapper mapper)
        {
            this._userManager = userManager;
            this._ordersRepo = _ordersRepo;
            _cart = cart;
            _mapper = mapper;
        }

        [HttpGet("Orders/Get")]
        [Authorize(Roles = "Admin")]
        public IActionResult Get()
        {
            var orders = _ordersRepo.GetOrdersList().Select(o => _mapper.Map<OrderViewModel>(o));
            return View(orders);
        }

        [HttpPost("Orders/CompleteOrder/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult CompleteOrder(int id)
        {
            var order = _ordersRepo.GetOrderById(id);
            if (order == null)
            {
                return View("StatusCodes/OrderNotFound");
            }

            order.IsCompleted = true;
            if (!_ordersRepo.SaveChanges())
            {
                return BadRequest();
            }

            return RedirectToAction("Get", "Orders");
        }

        [HttpPost("Orders/DeleteOrder/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteOrder(int id)
        {
            var order = _ordersRepo.GetOrderById(id);
            if (order == null)
            {
                return View("StatusCodes/OrderNotFound");
            }

            _ordersRepo.DeleteOrder(id);
            return RedirectToAction("Get", "Orders");
        }
        [HttpPost("Orders/Checkout/{id}")]
        [Authorize]
        public async Task<IActionResult> Checkout(int Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null)
            {
                return View("StatusCodes/UserNotFound");
            }

            var order = new Order {User = user, UserId = user.Id};
            var items = _cart.GetShoppingCartItems();
            _cart.Items = items;
            if (_cart.Items.Count == 0)
            {
                return View("EmptyCard");
            }
            _ordersRepo.AddOrder(order);
            _cart.ClearCart();
            return View("CheckoutComplete");
        }
    }
}