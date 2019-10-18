using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            User = new List<string>();
        }
        [Required(ErrorMessage ="这是必填的")]
        [Display(Name ="角色ID")]
        public string Id { get; set; }
        [Display(Name = "角色名称")]
        public string RoleName { get; set; }
        public List<string> User { get; set; }
    }
}
