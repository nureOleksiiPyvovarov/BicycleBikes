using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeStore.Models;
using BikeStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BikeStore.Controllers
{
    [Authorize(Roles="Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly UserManager<UserIdentityModel> _userManager;

        public RolesController(RoleManager<IdentityRole<int>> _roleManager, UserManager<UserIdentityModel> _userManager)
        {
            this._roleManager = _roleManager;
            this._userManager = _userManager;
        }
        [HttpGet("Roles/Create")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost("Roles/Create")]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole<int> role = new IdentityRole<int>()
                {
                    Name = model.RoleName
                };
                IdentityResult res = await _roleManager.CreateAsync(role);
                if (res.Succeeded)
                {
                    return RedirectToAction("GetRoles", "Roles");
                }
                foreach (IdentityError error in res.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [HttpGet("Roles/GetRoles")]
        public IActionResult GetRoles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpGet("Roles/Update/{id?}")]
        public async Task<IActionResult> Update(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return View("StatusCodes/RoleNotFound", id);
            }
            var model = new UpdateRoleViewModel()
            {
                Id = role.Id,
                RoleName = role.Name,
            };
            foreach (var user in _userManager.Users.ToList())
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }
        [HttpPost("Roles/Update/{id?}")]
        public async Task<IActionResult> Update(UpdateRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id.ToString());
            if (role == null)
            {
                return View("StatusCodes/RoleNotFound", model.Id);
            }
            else
            {
                role.Name = model.RoleName;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("GetRoles");
                }

                foreach (var resultError in result.Errors)
                {
                    ModelState.AddModelError("", resultError.Description);
                }
            }

            return View(model);
        }
        [HttpGet("Roles/UpdateUsersInRole/{id?}")]
        public async Task<IActionResult> UpdateUsersInRole(int roleId)
        {
            ViewBag.roleId = roleId;

            var role = await _roleManager.FindByIdAsync(roleId.ToString());

            if (role == null)
            {
                return View("StatusCodes/RoleNotFound", roleId);
            }

            var model = new List<UpdateUserRoleViewModel>();

            foreach (var user in _userManager.Users.ToList())
            {
                var userRoleViewModel = new UpdateUserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);
        }
        [HttpPost("Roles/UpdateUsersInRole/{id?}")]
        public async Task<IActionResult> UpdateUsersInRole(List<UpdateUserRoleViewModel> model, int roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());

            if (role == null)
            {
                return View("StatusCodes/RoleNotFound", roleId);
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId.ToString());

                IdentityResult result = null;

                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("Update", new { Id = roleId });
                }
            }

            return RedirectToAction("Update", new { Id = roleId });
        }

        [HttpPost("Roles/Delete/{id?}")]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return View("StatusCodes/RoleNotFound", id);
            }
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("GetRoles");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return RedirectToAction("Update", id);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View("StatusCodes/AccessDenied");
        }
    }
}