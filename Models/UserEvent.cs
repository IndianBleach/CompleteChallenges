using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class UserEvent
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Title { get; set; }
        public string DateCreatedStr { get; set; }
    }
}
