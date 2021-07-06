using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class UserRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
        public UserRole() =>            
            Users = new List<User>();       
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Score { get; set; }
        public ICollection<ChallengeLike> ChallengeLikes { get; set; }
        public ICollection<Solution> MySolutions { get; set; }
        public ICollection<Challenge> MyChallenges { get; set; }
        public ICollection<Challenge> UnlockedChallenges { get; set; }
        public ICollection<Discuss> MyDiscusses { get; set; }
        public ICollection<Reply> Replies { get; set; }
        public ICollection<UserEvent> Events { get; set; }
        public int RoleId { get; set; }
        public UserRole Role { get; set; }
        public DateTime DateCreated { get; set; }

        public User()
        {
            MySolutions = new List<Solution>();
            MyChallenges = new List<Challenge>();
            UnlockedChallenges = new List<Challenge>();
            MyDiscusses = new List<Discuss>();
            Replies = new List<Reply>();
            Events = new List<UserEvent>();
        }
    }
}
