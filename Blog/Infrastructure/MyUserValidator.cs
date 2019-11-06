using Blog.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Blog.Infrastructure
{
    public class MyUserValidator: UserValidator<AppUser>
    {
        public MyUserValidator(AppUserManager manager) : base(manager) { }

        public override async Task<IdentityResult> ValidateAsync(AppUser user)
        {
            IdentityResult result = await base.ValidateAsync(user);

            if (user.Email.ToLower().Contains("yahoo.com"))
            {
                var errors = result.Errors.ToList();
                errors.Add("Почта Yahoo не поддерживается");
                result = new IdentityResult(errors);
            }

            return result;
        }
    }
}