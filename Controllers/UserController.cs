using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Microsoft.EntityFrameworkCore;
using Project.Services;

namespace Project.Controllers
{
    //[Route("user")]
    public class UserController : Controller
    {
        private ApplicationContext _ctx;
        private UserActivityService _activityService;
        
        public UserController(ApplicationContext ctx, UserActivityService activityService)
        {
            _ctx = ctx;
            _activityService = activityService;
        }


        [Authorize]
        public async Task<IActionResult> Me()
        {
            User tryFindUser = await _ctx.Users
                .Include(x => x.Events)
                .FirstOrDefaultAsync(x =>
                x.Username == User.Identity.Name);

            ViewBag.SelfUserProfile = true;

            return View("pactivity", tryFindUser);    
        }

        [Route("{user}/{section?}")]
        public async Task<IActionResult> Profile(string user, string? section)
        {
            //NEED FIX
            User tryFindUser = await _ctx.Users
                .Include(x => x.Events)
                .Include(x => x.MyDiscusses)
                .Include(x => x.MySolutions)
                .Include(x => x.MyChallenges)
                .ThenInclude(x => x.Tests)
                .Include(x => x.MyChallenges)
                .ThenInclude(x => x.Level)
                .Include(x => x.MyChallenges)
                .ThenInclude(x => x.Tests)
                .ThenInclude(x => x.ProgLanguage)
                .FirstOrDefaultAsync(x =>
                x.Username == user);

            if (tryFindUser != null)
            {
                if (tryFindUser.Username == User.Identity.Name)
                    ViewBag.SelfUserProfile = true;

                return (section) switch
                {
                    "solutions" => View("psolutions", tryFindUser),
                    "creations" => View("pcreations", tryFindUser),
                    "discusses" => View("pdiscusses", tryFindUser),
                    _ => View("pactivity", tryFindUser)
                };
            }
            return RedirectToAction("index", "home");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
