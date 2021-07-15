using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Services;

namespace Project.Controllers
{
    public class LeaderboardController : Controller
    {
        private ApplicationContext _ctx;
        public LeaderboardController(ApplicationContext ctx)
        {
            _ctx = ctx;
        }

        public IActionResult Index()
        {
            User[] getAllUsers = _ctx.Users.OrderBy(x => x.Score).ToArray<User>();
            List<User> buildLeaderboard = new List<User>();
            int endOf = getAllUsers.Length >= 50 ? 50 : getAllUsers.Length;
            for (int i = 0; i < endOf; i++)
                buildLeaderboard.Add(getAllUsers[i]);

            return View(buildLeaderboard);
        }
    }
}
