using Blog.Infrastructure;
using Blog.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    [Authorize(Roles ="Автор, Administrators, Moderators")]
    public class BlogController : Controller
    {
        AppIdentityDbContext db = new AppIdentityDbContext();

        #region Получить все посты пользователя
        public async Task<ActionResult> GetUserPosts()
        {
            ViewData["IsAuth"] = HttpContext.User.Identity.IsAuthenticated;
            AppUser user = await UserManager.FindByNameAsync(HttpContext.User.Identity.Name);
            return View(user);
        }
        #endregion

        #region Создание поста
        public ActionResult Create()
        {
            ViewData["IsAuth"] = HttpContext.User.Identity.IsAuthenticated;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Post post)
        {
            AppUser cur_user = await UserManager.FindByNameAsync(HttpContext.User.Identity.Name);
            cur_user.Posts.Add(post);
            await UserManager.UpdateAsync(cur_user);
            
            return RedirectToAction("GetUserPosts");
        }
        #endregion

        #region Удаление поста
        public async Task<ActionResult> Delete(string id)
        {
            Post post = db.Posts.FirstOrDefault(p => p.Id.ToString() == id);

            if (post != null)
            {
                db.Posts.Remove(post);
                await db.SaveChangesAsync();
            }

            return RedirectToAction("GetUserPosts");
        }
        #endregion

        #region Редактирование поста
        public ActionResult Edit(string id)
        {
            Post post;
            if (id != null)
            {
                post = db.Posts.FirstOrDefault(p => p.Id.ToString() == id);
            }
            else
            {
                return HttpNotFound();
            }            

            if (post != null)
            {
                ViewData["IsAuth"] = HttpContext.User.Identity.IsAuthenticated;
                return View(post);
            }
            else
            {
                return new HttpStatusCodeResult(404);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Post post)
        {
            db.Entry(post).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("GetUserPosts");
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