using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Models.ValidModels;

namespace Project.Services
{
    public class AuthorizeService
    {
        private ApplicationContext _ctx;
        public AuthorizeService(ApplicationContext ctx)
        {
            _ctx = ctx;
        }

        public User RegisterUser(UserRegister regUser)
        {
            User buildUser = new Models.User
            {
                Username = regUser.Username,
                Email = regUser.Email,
                Password = regUser.Password,
                DateCreated = DateTime.Now,
                Events = new List<UserEvent>(),
                MyChallenges = new List<Challenge>(),
                MyDiscusses = new List<Discuss>(),
                MySolutions = new List<Solution>(),
                Replies = new List<Reply>(),
                Score = 0,
                ChallengeLikes = new List<ChallengeLike>(),
                UnlockedChallenges = new List<Challenge>(),
                Role = _ctx.UserRoles.FirstOrDefault(x =>
                    x.Name == "user")
            };

            _ctx.Users.Add(buildUser);
            _ctx.SaveChanges();

            return buildUser;
        }

        public User GetUserByData(UserLogin logUser)
        {

            User findUser = _ctx.Users
                .Include(x => x.Role)
                .FirstOrDefault(x =>
                    x.Username == logUser.Username && x.Password == logUser.Password);

            if (findUser != null) return findUser;

            return null;
        }

        public User GetUserByUsername(string username)
        {

            User findUser = _ctx.Users
                .FirstOrDefault(x =>
                    x.Username == username);

            if (findUser != null) return findUser;

            return null;
        }

        public void AddUserActivityEvent()
        { 
        
        }                
    }
}
