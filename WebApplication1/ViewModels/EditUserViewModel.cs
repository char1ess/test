using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.CustomUtil;

namespace WebApplication1.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }
        public string Id { get; set; }
        [Display(Name = "城市")]
        public string City { get; set; }
        [Required]
        [Display(Name ="用户名")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "邮箱")]
        [VailEmailDomain(allowDomain: "qq.com", ErrorMessage = "仅支持用QQ邮箱注册！")]
        public string Email { get; set; }
        public IList<string> Claims { get; set; }
        public IList<string> Roles { get; set; }
    }
}
