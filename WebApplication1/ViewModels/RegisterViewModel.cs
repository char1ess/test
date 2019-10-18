using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.CustomUtil;

namespace WebApplication1.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "邮箱地址")]
        [Required(ErrorMessage ="邮箱不可为空！")]
        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller: "account")]
        [VailEmailDomain(allowDomain:"qq.com",ErrorMessage ="仅支持用QQ邮箱注册！")]
        public string  Email { get; set; }
        [Display(Name = "密码")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name ="确认密码")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="两次密码不一致，请重新输入")]
        public string ConfirmPassword { get; set; }
    }
}
