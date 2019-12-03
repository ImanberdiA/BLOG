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
        #region Личный кабинет пользователя
        public async Task<ActionResult> PersonalAccount()
        {
            string name = HttpContext.User.Identity.Name;

            AppUser currentUser = await UserManager.FindByNameAsync(name);

            if (currentUser != null)
            {
                string roleNames = null;
                foreach (var role in currentUser.Roles)
                {
                    roleNames += string.Join(", ", RoleManager.Roles.Where(r => r.Id == role.RoleId).Select(r => r.Name));
                    roleNames += "; ";
                    //roleNames = RoleManager.Roles.Where(r => r.Id == role.RoleId).Select(r => r.Name);
                }

                ViewBag.RoleNames = roleNames;
                ViewData["IsAuth"] = HttpContext.User.Identity.IsAuthenticated;
                return View(currentUser);
            }
            else
            {
                return View("Error", new string[] { "Пользователь не существует" });
            }
        }
        #endregion

        #region Войти в систему
        public ActionResult Login(string ReturnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View("Error", new string[] { "В доступе отказано" });
            }

            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Login")]
        public async Task<ActionResult> LoginPost(LoginModel model, string ReturnUrl)
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

                if (ReturnUrl != string.Empty)
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                   return RedirectToAction("Index", "Home", null);
                }
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

        #region Регистрация пользователей
        public ActionResult Register()
        {
            if(HttpContext.User.Identity.IsAuthenticated)
            {
                return View("Error", new string[] { "В доступе отказано" });
            }

            IEnumerable<AppRole> Roles = RoleManager.Roles.Where(r => (r.Name != "Administrators") && (r.Name != "Moderators"));
            SelectList roles = new SelectList(Roles, "Name", "Name");
            ViewData["Roles"] = roles;
            return View();
        }
        
        [HttpPost]
        public async Task<ActionResult> Register(UserRegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser { UserName = model.Name, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    result = await UserManager.AddToRoleAsync(user.Id, model.RoleName);
                    if (result.Succeeded)
                    {
                        await LoginPost(new LoginModel { UserName = model.Name, Password = model.Password }, "/Home/Index");
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        AddErrorsFromResultToModelState(result);
                    }
                }
                else
                {
                    AddErrorsFromResultToModelState(result);
                }
            }

            IEnumerable<AppRole> Roles = RoleManager.Roles.Where(r => (r.Name != "Administrators") && (r.Name != "Moderators"));
            SelectList roles = new SelectList(Roles, "Name", "Name");
            ViewData["Roles"] = roles;

            return View(model);
        }
        #endregion
        
        private AppRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppRoleManager>();
            }
        }

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

        private void AddErrorsFromResultToModelState(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}