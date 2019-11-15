using Blog.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Blog.Infrastructure
{
    public class AppIdentityDbContext: IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext() : base("name=BlogDb") { }

        /*
        static AppIdentityDbContext()
        {
            Database.SetInitializer<AppIdentityDbContext>(new DbInit());
        }
        */

        public static AppIdentityDbContext Create()
        {
            return new AppIdentityDbContext();
        }

        public DbSet<Post> Posts { get; set; }
    }

    public class DbInit : DropCreateDatabaseAlways<AppIdentityDbContext>
    {
        protected override void Seed(AppIdentityDbContext context)
        {
            base.Seed(context);
        }
    }
}