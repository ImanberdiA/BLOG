using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Blog.Infrastructure
{
    public class MyPasswordValidator: PasswordValidator
    {
        public override async Task<IdentityResult> ValidateAsync(string pass)
        {
            IdentityResult result = await base.ValidateAsync(pass);

            if (pass.Equals("123456") || pass.Equals("qwerty"))
            {
                var errors = result.Errors.ToList();
                errors.Add("Пароль не должен быть 123 или qwerty. Придумайте более сложный пример");
                result = new IdentityResult(errors);
            }

            return result;
        }
    }
}