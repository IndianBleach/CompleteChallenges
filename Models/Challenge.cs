using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Challenge
    {
        public int Id { get; set; }
        public User Author { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Solution> Solutions { get; set; }
        public ICollection<Test> Tests { get; set; }
        public ICollection<User> UsersWhoUnlocked { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public DateTime DateCreated { get; set; }

        public Challenge()
        {
            Solutions = new List<Solution>();
            Tests = new List<Test>();
            UsersWhoUnlocked = new List<User>();
            Tags = new List<Tag>();            
        }
    }
}