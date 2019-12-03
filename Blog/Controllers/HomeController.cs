using Blog.Infrastructure;
using Blog.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        AppIdentityDbContext db = new AppIdentityDbContext();

        // ЗДЕСЬ ВЫВОДИТЬ ВСЕ СТАТЬИ С НАЗВАНИЕМ И НЕПОЛНЫМ ТЕКСТОМ(КНОПОЧКУ ЧИТАТЬ ДАЛЬШЕ)
        public ActionResult Index()
        {
            ViewData["IsAuth"] = HttpContext.User.Identity.IsAuthenticated;
            return View(db.Posts.ToList());
        }

        public ActionResult GetArticle(string id)
        {
            ViewData["IsAuth"] = HttpContext.User.Identity.IsAuthenticated;
            Post post = db.Posts.FirstOrDefault(p => p.Id.ToString() == id);
            return View(post);
        }
    }
}