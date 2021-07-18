using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Project.Models.ValidModels
{
    public class UserEdit
    {
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string GitHubLink { get; set; }
        public string About { get; set; }
        public string Email { get; set; }
        public IFormFile Avatar { get; set; }
    }
}
