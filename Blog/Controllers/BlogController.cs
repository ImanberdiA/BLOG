﻿using Blog.Infrastructure;
using Blog.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class BlogController : Controller
    {
        AppIdentityDbContext db = new AppIdentityDbContext();

        // GET: Blog
        public ActionResult Index()
        {
            IEnumerable<AppUser> users = UserManager.Users.ToList();
            return View(users);
        }

        #region Создание поста
        public ActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreatePost(Post post)
        {
            AppUser cur_user = await UserManager.FindByNameAsync(HttpContext.User.Identity.Name);
            cur_user.Posts.Add(post);
            await UserManager.UpdateAsync(cur_user);
            
            return RedirectToAction("Index");
        }
        #endregion

        #region Удаление поста
        public async Task<ActionResult> DeletePost(string id)
        {
            Post post = db.Posts.FirstOrDefault(p => p.Id.ToString() == id);

            if (post != null)
            {
                db.Posts.Remove(post);
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
        #endregion

        public AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
    }
}