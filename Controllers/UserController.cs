using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Models.ValidModels;
using Microsoft.EntityFrameworkCore;
using Project.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Project.Controllers
{
    //[Route("user")]
    public class UserController : Controller
    {
        private ApplicationContext _ctx;
        private UserActivityService _activityService;
        private IWebHostEnvironment _env;
        private AuthorizeService _authorizeService;
        
        public UserController(ApplicationContext ctx, UserActivityService activityService, IWebHostEnvironment env, AuthorizeService authService)
        {
            _env = env;
            _ctx = ctx;
            _activityService = activityService;
            _authorizeService = authService;
        }

        [Authorize]
        public async Task<IActionResult> Delete(string password)
        {
            User findUser = await _ctx.Users
                .Include(x => x.Events)
                .Include(x => x.Avatar)
                .Include(x => x.Reports)
                .Include(x => x.MyChallenges)
                .Include(x => x.Comments)
                .Include(x => x.MyDiscusses)
                .Include(x => x.MySolutions)
                .ThenInclude(x => x.Challenge)
                .Include(x => x.MyChallenges)
                .ThenInclude(x => x.Tests)
                .FirstOrDefaultAsync(x => x.Username == User.Identity.Name && x.Password == password);

            _ctx.Challenges.RemoveRange(_ctx.Challenges.Where(x => x.Author == findUser));

            if (findUser != null)
            {
                _ctx.Users.Remove(findUser);
            }

            await _ctx.SaveChangesAsync();

            await _authorizeService.SignOutAsync(HttpContext);

            return RedirectToAction("index", "home");
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(string username, string email, string oldPass, string newPass, string about, IFormFile avatar, string githubLink)
        {
            User tryFindUser = await _ctx.Users
                .Include(x => x.Role)
                .Include(x => x.Avatar)
                .FirstOrDefaultAsync(x =>
                x.Username == User.Identity.Name);

            tryFindUser.Username = string.IsNullOrWhiteSpace(username) ? tryFindUser.Username : username;
            tryFindUser.About = string.IsNullOrWhiteSpace(about) ? tryFindUser.About : about;
            tryFindUser.GitHubLink = string.IsNullOrWhiteSpace(githubLink) ? tryFindUser.GitHubLink : githubLink;
            tryFindUser.Email = string.IsNullOrWhiteSpace(email) ? tryFindUser.Email : email;

            if (oldPass == tryFindUser.Password)
            {
                tryFindUser.Password = string.IsNullOrWhiteSpace(newPass) ? tryFindUser.Password : newPass;
            }

            if (avatar != null)
            {                
                if (tryFindUser.Avatar != null && tryFindUser.Avatar.Name != "default_user_avatar221.jpg")
                    System.IO.File.Delete(_env.WebRootPath + $"/media/avatars/{tryFindUser.Avatar.Name}");

                string avatarName = $"{User.Identity.Name}{avatar.FileName}";

                using (FileStream fs = new FileStream(_env.WebRootPath + $"/media/avatars/{avatarName}", FileMode.Create))
                {
                    await avatar.CopyToAsync(fs);

                    Avatar newAvatar = new Avatar()
                    {
                        Name = avatarName,
                    };

                    tryFindUser.Avatar = newAvatar;

                    fs.Close();
                }
            }
            await _ctx.SaveChangesAsync();

            await _authorizeService.SignInAsync(tryFindUser, HttpContext);

            ViewBag.SelfUserProfile = true;

            return RedirectToAction("me");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            User tryFindUser = await _ctx.Users
                .Include(x => x.Avatar)
                .FirstOrDefaultAsync(x =>
                x.Username == User.Identity.Name);

            if (tryFindUser != null)
            {
                ViewBag.SelfUsername = tryFindUser.Username;
                ViewBag.SelfEmail = tryFindUser.Email;
                ViewBag.SelfAbout = tryFindUser.About;
                ViewBag.SelfGitHub = tryFindUser.GitHubLink;
                ViewBag.SelfUserProfile = true;

                return View("edit", tryFindUser);
            }

            return View("index", "home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Me()
        {
            User tryFindUser = await _ctx.Users
                .Include(x => x.Events)
                .Include(x => x.Avatar)
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
                x.Username == User.Identity.Name);

            ViewBag.SelfUserProfile = true;
            //return Content(User.Identity.Name);
            return View("pactivity", tryFindUser);    
        }

        [Route("{user}/{section?}")]
        public async Task<IActionResult> Profile(string user, string? section)
        {
            //NEED FIX
            User tryFindUser = await _ctx.Users
                .Include(x => x.Events)
                .Include(x => x.Avatar)
                .Include(x => x.MyDiscusses)
                .Include(x => x.MySolutions)
                .ThenInclude(x => x.Challenge)
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
