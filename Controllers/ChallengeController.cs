using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Controllers
{
    public class ChallengeController : Controller
    {
        private ApplicationContext _ctx;

        public ChallengeController(ApplicationContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IActionResult> Solve(int? challenge)
        {
            if (challenge != null)
            {
                Challenge findChallenge = await _ctx.Challenges
                    .Include(x => x.Author)
                    .Include(x => x.Solutions)
                    .Include(x => x.Tags)
                    .Include(x => x.Tests)
                    .Include(x => x.Likes)
                    .Include(x => x.UsersWhoUnlocked)
                    .FirstOrDefaultAsync(x => x.Id == challenge);

                if (findChallenge != null)
                {
                    return View("solve", findChallenge);
                }
            }

            return RedirectToAction("index");
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
