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
using Project.Services;

namespace Project.Controllers
{
    public class AuthorizeController : Controller
    {
        private ApplicationContext _ctx;
        private AuthorizeService _authService;

        public AuthorizeController(ApplicationContext ctx, AuthorizeService authServ)
        {
            _ctx = ctx;
            _authService = authServ;
        }

        #region :POST ENDPOINTS
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("login", "authorize");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLogin logUser)
        {
            if (ModelState.IsValid)
            {
                User existUser = _authService.GetUserByData(logUser);

                if (existUser != null)
                {
                    await _authService.GoAuthenticate(existUser, HttpContext);

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
                User isUserAlreadyExist = _authService.GetUserByUsername(regUser.Username);

                if (isUserAlreadyExist == null)
                {
                    User newUser = _authService.RegisterUser(regUser);

                    await _authService.GoAuthenticate(newUser, HttpContext);

                    return RedirectToAction("index", "home");
                }
                else ModelState.AddModelError("", "Такой логин уже существует");
            }
            return View(regUser);
        }

        #endregion

        #region :GET ENDPOINTS
        [HttpGet]
        public IActionResult Login()
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
