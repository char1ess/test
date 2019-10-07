using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)//注入UserManager和SingInManager服务
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser//通过IdentityUser实例来存储传过来的表单内容
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await userManager.CreateAsync(user,model.Password);//通过userManager来异步创建用户
                if (result.Succeeded)//成功
                {
                    await signInManager.SignInAsync(user,isPersistent:false);//创建用户，和是否记住登陆状态
                    RedirectToAction("index", "home");
                }
                foreach (var error in result.Errors)//失败
                {
                    ModelState.AddModelError(string.Empty,error.Description);//添加失败信息到ModelState中
                }
            }
            return View(model);//验证错误保留填写内容
        }
    }
}
