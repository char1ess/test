using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.CustomUtil
{
    public class VailEmailDomainAttribute:ValidationAttribute
    {
        private readonly string allowDomain;

        public VailEmailDomainAttribute(string allowDomain)
        {
            this.allowDomain = allowDomain;
        }

        public override bool IsValid(object value)
        {
            string[] strings = value.ToString().Split('@');
            return strings[1].ToUpper() == allowDomain.ToUpper();
        }
    }
}
