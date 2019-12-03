using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Blog.Infrastructure;
using Blog.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Host.SystemWeb;

namespace Blog.Controllers
{
    [Authorize(Roles = "Administrators, Moderators")]
    //[Authorize]
    public class AdminController: Controller
    {
        public ActionResult Index()
        {
            return View(UserManager.Users);
        }

        #region Создание пользователя
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser { UserName = model.Name, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResultToModelState(result);
                }
            }

            return View(model);            
        }
        #endregion

        #region Удаление пользователя
        public async Task<ActionResult> Delete(string id)
        {
            AppUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", result.Errors);
                }
            }

            return View("Error", new string[] { "Пользователь не найден" });
        }
        #endregion

        #region Редактирование пользователя
        public async Task<ActionResult> Edit(string id)
        {
            AppUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return View("Error", new string[] { "Пользователь не найден" });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, string email, string UserName, string password)
        {
            AppUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Email = email;
                user.UserName = UserName;
                IdentityResult validaUserData = await UserManager.UserValidator.ValidateAsync(user);

                if (!validaUserData.Succeeded)
                {
                    AddErrorsFromResultToModelState(validaUserData);
                }

                IdentityResult validPass = null;
                if (password != string.Empty)
                {
                    validPass = await UserManager.PasswordValidator.ValidateAsync(password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = UserManager.PasswordHasher.HashPassword(password);
                    }
                    else
                    {
                        AddErrorsFromResultToModelState(validPass);
                    }
                }

                if ((validaUserData.Succeeded && validPass == null) || (validaUserData.Succeeded && password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            else
            {
                return View("Error", new string[] { "Пользователь не найден" });
            }

            return View(user);
        }
        #endregion

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