using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Project.Controllers
{    
    public class UserController : Controller
    {
        private ApplicationContext _ctx;
        
        public UserController(ApplicationContext ctx)
        {
            _ctx = ctx;
        }

        [Authorize]
        public async Task<IActionResult> MyProfile()
        {
            User findUser = await _ctx.Users.FirstOrDefaultAsync(x =>
            x.Username == User.Identity.Name);

            ViewBag.SelfUserProfile = true;

            if (findUser != null) return View("profile", findUser);

            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> Profile(int? user)
        {
            User findUser;
            if (user != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    int selfUserId = _ctx.Users.FirstOrDefault(x =>
                    x.Username == User.Identity.Name)
                    .Id;

                    if (user == selfUserId) return RedirectToAction("MyProfile");
                }
                
                findUser = await _ctx.Users.FirstOrDefaultAsync(x =>
                    x.Id == user);

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
