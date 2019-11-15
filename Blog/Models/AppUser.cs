using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class AppUser: IdentityUser
    {
        public virtual ICollection<Post> Posts { get; set; }

        public AppUser()
        {
            Posts = new List<Post>();
        }
    }
}