using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeStore.Models;
using BikeStore.Models.Repos;
using BikeStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ShoppingCart _shoppingCart;
        private readonly IProductsRepo _productsRepo;

        public ShoppingCartController(ShoppingCart shoppingCart, IProductsRepo productsRepo)
        {
            _shoppingCart = shoppingCart;
            _productsRepo = productsRepo;
        }
        [Authorize]
        [HttpGet("ShoppingCart/Index")]
        public IActionResult Index()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.Items = items;

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };
            return View(shoppingCartViewModel);
        }
        [HttpPost("ShoppingCart/AddToShoppingCart/{id}")]
        [Authorize]
        public IActionResult AddToShoppingCart(int id)
        {
            var selectedProduct = _productsRepo.GetProductById(id);
            if (selectedProduct != null)
            {
                _shoppingCart.AddToCart(selectedProduct, 1);
            }
            return RedirectToAction("Index");
        }
        [HttpPost("ShoppingCart/RemoveFromShoppingCart/{id}")]
        [Authorize]
        public IActionResult RemoveFromShoppingCart(int id)
        {
            var selectedProduct = _productsRepo.GetProductById(id);
            if (selectedProduct != null)
            {
                _shoppingCart.RemoveFromCart(selectedProduct);
            }
            else
            {
                return View("StatusCodes/BikeNotFound");
            }
            return RedirectToAction("Index");
        }

        public static void ClearCart()
        {
            
        }
    }
}