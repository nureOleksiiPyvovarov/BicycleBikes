using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BikeStore.Models;
using BikeStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;

namespace BikeStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<UserIdentityModel> _userManager;
        private readonly SignInManager<UserIdentityModel> _signInManager;
        private readonly IMapper _mapper;

        public AccountController(UserManager<UserIdentityModel> _userManager, SignInManager<UserIdentityModel> _signInManager, IMapper _mapper)
        {
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._mapper = _mapper;
        }
        [HttpGet("Account/Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Account/Register")]
        public async Task<IActionResult> Register(UserRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<UserIdentityModel>(model);
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("GetList", "Users");
                    }
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    await _userManager.AddToRoleAsync(user, "Customer");
                    ModelState.AddModelError("", user.ToString());
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

            return View(model);
        }

        [HttpPost("Account/LogOut")]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        [HttpGet("Account/LogIn")]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost("Account/LogIn")]
        public async Task<IActionResult> LogIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(String.Empty, "Login attempt is invalid.\nCheck your email and password");
            }

            return View();
        }
        [Authorize]
        [HttpGet("Account/ManageProfile")]
        public async Task<IActionResult> ManageProfile()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var model = _mapper.Map<UserManageViewModel>(user);
            return View(model);
        }
        [Authorize]
        [HttpPost("Account/ManageProfile/{id?}")]
        public async Task<IActionResult> ManageProfile(UserManageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id.ToString());
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.UserName = model.UserName;
                var newPassword = _userManager.PasswordHasher.HashPassword(user, model.Password);
                user.PasswordHash = newPassword;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("index", "home");
                foreach(var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }
            return View(model);
        }
    }
}