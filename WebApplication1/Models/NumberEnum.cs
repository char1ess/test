using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    /// <summary>
    /// 数量枚举
    /// </summary>
    public enum NumberEnum
    {
        [Display(Name = "无")]
        None,
        [Display(Name = "一")]
        One,
        [Display(Name = "二")]
        Two,
        [Display(Name = "三")]
        Three
    }
}
