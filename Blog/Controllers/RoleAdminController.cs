using Blog.Infrastructure;
using Blog.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class RoleAdminController : Controller
    {
        // GET: RoleAdmin
        public ActionResult Index()
        {
            return View(RoleManager.Roles);
        }

        #region Создание роли
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create([Required]string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await RoleManager.CreateAsync(new AppRole(name));

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }

            return View(name);
        }
        #endregion

        #region Удаление роли
        public async Task<ActionResult> Delete(string id)
        {
            AppRole role = await RoleManager.FindByIdAsync(id);

            if (role != null)
            {
                IdentityResult result = await RoleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", result);
                }
            }
            else
            {
                return View("Error", new string[] { "Роль не найдена" });
            }
        }
        #endregion

        #region Редактирование роли
        public async Task<ActionResult> Edit([Required]string id)
        {
            if (ModelState.IsValid)
            {
                AppRole role = await RoleManager.FindByIdAsync(id);

                if (role != null)
                {
                    string[] memberIds = role.Users.Select(x => x.UserId).ToArray();

                    IEnumerable<AppUser> members = UserManager.Users.Where(x => memberIds.Any(y => y == x.Id));

                    IEnumerable<AppUser> nonMembers = UserManager.Users.Except(members);

                    return View(new RoleEditModel {
                        Role = role,
                        Members = members,
                        NonMembers = nonMembers
                    });
                }
            }

            return View("Error", new string[] { "Роль не найдена" });
        }

        public async Task<ActionResult> Edit(RoleModificationModel model)
        {
            IdentityResult result;
            if (model != null && ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd)
                {
                    result = await UserManager.AddToRoleAsync(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }

                foreach (string userId in model.IdsToRemove)
                {
                    result = await UserManager.RemoveFromRoleAsync(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }

                return Redirect("Index");
            }

            return View("Error", new string[] { "Произошла ошибка" });
        }
        #endregion

        private AppRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppRoleManager>();
            }
        }

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}