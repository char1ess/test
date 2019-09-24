using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class StudentCreatViewModel
    {
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Required(ErrorMessage = "请输入名字"), Display(Name = "名字"), MaxLength(50, ErrorMessage = "名字长度不可以超过50个字符")]
        public string Name { get; set; }
        [Required(ErrorMessage = "请选择编号"), Display(Name = "编号")]
        public NumberEnum? Number { get; set; }
        [Display(Name = "图片")]
        public List<IFormFile> Photos { get; set; }
    }
}
