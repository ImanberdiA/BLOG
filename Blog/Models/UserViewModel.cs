using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    #region Модель для регистрации пользователя
    public class UserRegistrationModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        
        [Required]
        public string RoleName { get; set; }
    }
    #endregion

    // Создание пользователя
    public class CreateUserModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    // Модель для аутентификации
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }

    #region Модели для редактирование роли
    public class RoleEditModel
    {
        public AppRole Role { get; set; }

        public IEnumerable<AppUser> Members { get; set; }

        public IEnumerable<AppUser> NonMembers { get; set; }
    }

    public class RoleModificationModel
    {
        [Required]
        public string RoleName { get; set; }
        
        public string[] IdsToAdd { get; set; }

        public string[] IdsToRemove { get; set; }
    }

    #endregion
}