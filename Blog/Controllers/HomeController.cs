using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        // ЗДЕСЬ ВЫВОДИТЬ ВСЕ СТАТЬИ С НАЗВАНИЕМ И НЕПОЛНЫМ ТЕКСТОМ(КНОПОЧКУ ЧИТАТЬ ДАЛЬШЕ)
        public ActionResult Index()
        {
            return View();
        }


    }
}