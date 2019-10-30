using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };
                //如果创建同名的角色会报错
                IdentityResult result = await roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("listroles", "admin");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"角色ID为{id}的角色 不存在，请重试！";
                return View("~/Views/Error/NotFound.cshtml");
            }
            //存在此角色，将信息存到对应的EditRoleViewModel中
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };
            //获取所有的用户信息
            var users = userManager.Users.ToList();
            foreach (var user in users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))//用户是否包含这个角色名
                {
                    model.User.Add(user.UserName);//有的话添加到model中
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"角色ID为{model.Id}的角色 不存在，请重试！";
                return View("~/Views/Error/NotFound.cshtml");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("listroles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            //通过roleId来查询是否存在
            ViewBag.roleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"角色ID为{roleId}的角色 不存在，请重试！";
                return View("~/Views/Error/NotFound.cshtml");
            }

            var model = new List<UserRoleViewModel>();
            //循环所有用户
            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {//获取用户的id和username
                    UserId = user.Id,
                    UserName = user.UserName
                };//用户有该觉得则为选中
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);//将用户信息添加到集合中
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            //通过roleId来查询是否存在
            ViewBag.roleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"角色ID为{roleId}的角色 不存在，请重试！";
                return View("~/Views/Error/NotFound.cshtml");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);//查询用户
                var isInRole = await userManager.IsInRoleAsync(user, role.Name);//查询用户是否有该角色
                IdentityResult result = null;
                if (!isInRole && model[i].IsSelected)//用户没有这个角色，并选中则添加用户到角色中
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (isInRole && !model[i].IsSelected)//用户有这个角色，但并没有选中，则把用户从角色中移除
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else//被选中，用户已存在角色 不操作进行下次循环
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < model.Count - 1)//执行完2所有用户才算成功，否则需要继续循环
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("EditRole", new { id = roleId });
                    }
                }
            }
            return RedirectToAction("EditRole", new { id = roleId });
        }
    }
}
