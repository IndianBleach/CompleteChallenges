using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models.ValidModels
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Поле обязательно")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Поле обязательно")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}