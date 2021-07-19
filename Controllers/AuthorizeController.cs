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
        private AuthorizeService _authService;

        public AuthorizeController(AuthorizeService authServ)
        {
            _authService = authServ;
        }

        #region :POST ENDPOINTS
        public async Task<IActionResult> Logout()
        {
            await _authService.SignOutAsync(HttpContext);
            return RedirectToAction("login", "authorize");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLogin logUser)
        {
            if (ModelState.IsValid)
            {
                User existUser = _authService.GetUserByLoginForm(logUser);
                if (existUser != null)
                {
                    await _authService.SignInAsync(existUser, HttpContext);

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
                if (!_authService.IsUserAlreadyExist(regUser.Username))
                {
                    User newUser = _authService.Register(regUser);

                    await _authService.SignInAsync(newUser, HttpContext);

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
