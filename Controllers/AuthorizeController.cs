using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Models.ValidModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Project.Controllers
{
    public class AuthorizeController : Controller
    {
        private ApplicationContext _ctx;

        public AuthorizeController(ApplicationContext ctx)
        {
            _ctx = ctx;
        }

        #region :POST ENDPOINTS
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLogin logUser)
        {
            if (ModelState.IsValid)
            {
                User existUser = await _ctx.Users
                    .Include(x => x.Role)
                    .FirstOrDefaultAsync(x =>
                    x.Username == logUser.Username && x.Password == x.Password);

                if (existUser != null)
                {
                    await GoAuthenticate(existUser);

                    return RedirectToAction("Index", "Home");
                }
                else ModelState.AddModelError("", "Пользователь не найден");
            }
            return View(logUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegister regUser)
        {
            if (ModelState.IsValid)
            {
                User isUserAlreadyExist = await _ctx.Users.FirstOrDefaultAsync(x => x.Username == regUser.Username);

                if (isUserAlreadyExist == null)
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
                        UnlockedChallenges = new List<Challenge>(),
                        Role = await _ctx.UserRoles.FirstOrDefaultAsync(x =>
                            x.Name == "user")
                    };

                    _ctx.Users.Add(buildUser);
                    await _ctx.SaveChangesAsync();

                    await GoAuthenticate(buildUser);

                    return RedirectToAction("index", "home");
                }
                else ModelState.AddModelError("", "Такой логин уже существует");
            }
            return View(regUser);
        }

        public async Task GoAuthenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "app_authorize_cokie", 
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType
                );

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        #endregion

        #region :GET ENDPOINTS
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        #endregion
    }
}
