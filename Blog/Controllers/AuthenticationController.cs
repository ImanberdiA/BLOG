using Blog.Infrastructure;
using Blog.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class AuthenticationController: Controller
    {

        #region Войти в систему
        public ActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string ReturnUrl)
        {
            AppUser user = await UserManager.FindAsync(model.UserName, model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Пароль с таким именем или паролем не существует");
            }
            else
            {
                ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

                AuthenticationManager.SignOut();
                AuthenticationManager.SignIn(new AuthenticationProperties {
                    IsPersistent = true
                }, ident);

                return Redirect(ReturnUrl);
            }

            return View(model);
        }
        #endregion

        #region Выйти из системы
        [Authorize]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
    }
}