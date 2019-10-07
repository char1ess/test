using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "邮箱地址")]
        [Required]
        [EmailAddress]
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
