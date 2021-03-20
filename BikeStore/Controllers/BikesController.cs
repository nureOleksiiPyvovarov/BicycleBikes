using System;
using System.IO;
using System.Linq;
using AutoMapper;
using BikeStore.Models.Repos;
using BikeStore.Models.Repos.Interfaces;
using BikeStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLog;

namespace BikeStore.Controllers
{
    public class BikesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProductsRepo _productsRepo;
        private readonly ICategoriesRepo _categoriesRepo;
        private readonly IBrandsRepo _brandsRepo;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BikesController(IMapper _mapper, IProductsRepo _productsRepo, ICategoriesRepo _categoriesRepo, IBrandsRepo _brandsRepo, IWebHostEnvironment _hostEnvironment)
        {
            this._mapper = _mapper;
            this._productsRepo = _productsRepo;
            this._categoriesRepo = _categoriesRepo;
            this._brandsRepo = _brandsRepo;
            this._hostEnvironment = _hostEnvironment;
        }
        [Route("Bikes/Details/{id}")]
        public IActionResult Details(int id)
        {
            Product bike = null;
            try
            {
                bike = _productsRepo.GetProductById(id);
            }
            catch (ArgumentException)
            {
                return View("StatusCodes/BikeNotFound", id);
            }
            var bikeCard = _mapper.Map<BikeCardViewModel>(bike);
            return View(bikeCard);
        }
        [Route("Bikes/BikesByCategory/{category}")]
        public IActionResult BikesByCategory(string category)
        {
            var bikeCategory = _categoriesRepo.GetCategory(category);
            if (bikeCategory == null)
                return NotFound();
            var bikes = bikeCategory.Products.ToList();
            var bikesCard = bikes.Select(b => _mapper.Map<BikeCardViewModel>(b)).ToList();
            return View(bikesCard);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet(@"Bikes/Create")]
        public IActionResult Create()
        {
            ViewBag.Brands = new SelectList(_brandsRepo.GetBrands().ToList(), "BrandId", "BrandName");
            ViewBag.Categories = new SelectList(_categoriesRepo.GetCategories().ToList(), "CategoryId", "CategoryName");
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Bikes/Create")]
        public IActionResult Create(CreateBikeViewModel createBike)
        {
            if (ModelState.IsValid)
            {
                var bike = _mapper.Map<Product>(createBike);
                var fileName = GetPhotoPath(createBike);
                bike.ProductPhoto = fileName;
                _productsRepo.CreateProduct(bike);
                return RedirectToAction("Details", new { id = bike.ProductId });
            }
            ViewBag.Brands = new SelectList(_brandsRepo.GetBrands(), "BrandName", "BrandName");
            ViewBag.Categories = new SelectList(_categoriesRepo.GetCategories(), "CategoryName", "CategoryName");
            return View(createBike);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("Bikes/Update/{id}")]
        public IActionResult Update(int id)
        {
            Product bikeUpdate = null;
            try
            {
                bikeUpdate = _productsRepo.GetProductById(id);
            }
            catch (ArgumentException e)
            {
                return View("StatusCodes/BikeNotFound", id);
            }
            var bikeUpdateModel = _mapper.Map<UpdateBikeViewModel>(bikeUpdate);
            bikeUpdateModel.ExistingPhoto = bikeUpdate.ProductPhoto;
            ViewBag.Brands = new SelectList(_brandsRepo.GetBrands().ToList(), "BrandId", "BrandName");
            ViewBag.Categories = new SelectList(_categoriesRepo.GetCategories().ToList(), "CategoryId", "CategoryName");
            return View(bikeUpdateModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Bikes/Update/{id?}")]
        public IActionResult Update(int id, UpdateBikeViewModel model)
        {
            if (ModelState.IsValid)
            {
                string photoPath = String.Empty;
                if (model.ProductPhoto == null)
                    photoPath = model.ExistingPhoto;
                else
                {
                    photoPath = GetPhotoPath(model);
                }
                var bike = _mapper.Map<Product>(model);
                bike.ProductPhoto = photoPath;
                _productsRepo.UpdateProduct(bike);
                return RedirectToAction("Details", new { id = bike.ProductId });
            }
            ViewBag.Brands = new SelectList(_brandsRepo.GetBrands(), "BrandName", "BrandName");
            ViewBag.Categories = new SelectList(_categoriesRepo.GetCategories(), "CategoryName", "CategoryName");
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("Bikes/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            Product bikeDelete = null;
            try
            {
                bikeDelete = _productsRepo.GetProductById(id);
            }
            catch (ArgumentException e)
            {
                return View("StatusCodes/BikeNotFound", id);
            }
            var model = _mapper.Map<DeleteBikeViewModel>(bikeDelete);
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Bikes/Delete/{id}")]
        public IActionResult Delete(int id, DeleteBikeViewModel model)
        {
            if (ModelState.IsValid)
            {
                _productsRepo.DeleteProduct(model.ProductId);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
        private string GetPhotoPath(CreateBikeViewModel createBike)
        {
            if (createBike.ProductPhoto == null)
                return "~/img/noimage.jpg";
            var uniqueFileName = "~/img/" + Guid.NewGuid().ToString() + "_" + createBike.ProductPhoto.FileName;
            var rootPath = _hostEnvironment.WebRootPath;
            string replacedFileName = uniqueFileName.Substring(1).Replace('/', '\\');
            using (var stream = new FileStream(rootPath + replacedFileName, FileMode.Create))
            {
                createBike.ProductPhoto.CopyTo(stream);
            }

            return uniqueFileName;
        }
    }
}