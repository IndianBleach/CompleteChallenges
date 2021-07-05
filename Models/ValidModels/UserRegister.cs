using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Project.Models.ValidModels
{
    public class UserRegister
    {
        [Required(ErrorMessage = "Поле обязательно")]
        [MinLength(3, ErrorMessage = "Минимум 3 символа")]
        [MaxLength(22, ErrorMessage = "Максимум 22 символа")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Поле обязательно")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Поле обязательно")]
        [DataType(DataType.Password)]
        [MinLength(3, ErrorMessage = "Минимум 3 символа")]
        [MaxLength(28, ErrorMessage = "Максимум 28 символов")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Поле обязательно")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; }
    }
}
