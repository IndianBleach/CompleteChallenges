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
        public async Task<IActionResult> MyProfile()
        {
            User findUser = await _ctx.Users.FirstOrDefaultAsync(x =>
            x.Username == User.Identity.Name);

            ViewBag.ActivityEvents = _activityService.GetUserActivityEvents(User.Identity.Name);

            ViewBag.SelfUserProfile = true;

            if (findUser != null) return View("profile", findUser);

            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> Profile(int? user)
        {
            User findUser;
            if (user != null)
            {                        
                findUser = await _ctx.Users.FirstOrDefaultAsync(x =>
                    x.Id == user);

                if (findUser.Username == User.Identity.Name)
                    return RedirectToAction("MyProfile");

                ViewBag.ActivityEvents = _activityService.GetUserActivityEvents(findUser.Username);

                if (findUser != null) return View(findUser);
                else
                {
                    //return self user profile if findUser or user == null
                    return RedirectToAction("MyProfile");
                }
            }
            return RedirectToAction("index", "home");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
