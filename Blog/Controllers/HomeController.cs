using Blog.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        AppIdentityDbContext db = new AppIdentityDbContext();

        // ЗДЕСЬ ВЫВОДИТЬ ВСЕ СТАТЬИ С НАЗВАНИЕМ И НЕПОЛНЫМ ТЕКСТОМ(КНОПОЧКУ ЧИТАТЬ ДАЛЬШЕ)
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }



    }
}