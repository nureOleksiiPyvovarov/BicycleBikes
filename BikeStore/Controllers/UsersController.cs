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

namespace BikeStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly UserManager<UserIdentityModel> _userManager;
        private readonly IMapper _mapper;

        public UsersController(RoleManager<IdentityRole<int>> _roleManager, UserManager<UserIdentityModel> _userManager, IMapper _mapper)
        {
            this._roleManager = _roleManager;
            this._userManager = _userManager;
            this._mapper = _mapper;
        }
        [HttpGet("Users/GetList")]
        public IActionResult GetList()
        {
            var users = _userManager.Users.Select(u => _mapper.Map<UsersListItemModel>(u));
            return View(users);
        }
        [HttpGet("Users/Update")]
        public async Task<IActionResult> Update(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return View("StatusCodes/UserNotFound", id);
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new UpdateUserViewModel()
            {
                Id = user.Id,
                Name = user.UserName,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles
            };

            return View(model);
        }
        [HttpPost("Users/Delete/{id?}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return View("StatusCodes/UserNotFound", id);
            }
            else
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("GetList");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("GetList");
            }
        }

        [HttpGet("Users/ManageUserRoles/{id?}")]
        public async Task<IActionResult> ManageUserRoles(int id)
        {
            ViewBag.userId = id;
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return View("StatusCodes/UserNotFound");
            }

            var model = new List<ManageUserRolesItem>();
            foreach (var role in _roleManager.Roles.ToList())
            {
                var manageUserRolesItem = new ManageUserRolesItem()
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    manageUserRolesItem.IsSelected = true;
                }
                else
                {
                    manageUserRolesItem.IsSelected = false;
                }
                model.Add(manageUserRolesItem);
            }

            return View(model);
        }
        [HttpPost("Users/ManageUserRoles/{userId?}")]
        public async Task<IActionResult> ManageUserRoles(int userId, List<ManageUserRolesItem> model)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return View("StatusCodes/UserNotFound");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            result = await _userManager.AddToRolesAsync(user,
                model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction("Update", new { Id = userId });
        }

    }
}