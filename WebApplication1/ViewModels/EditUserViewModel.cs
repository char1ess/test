using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        [Display(Name = "城市")]
        public string City { get; set; }
        [Required]
        [Display(Name ="用户名")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "邮箱")]
        public string Email { get; set; }
        public IList<string> Claims { get; set; }
        public IList<string> Roles { get; set; }
    }
}
