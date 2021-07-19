using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Project.Services;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationContext _ctx;
        private ChallengeService _challengeService;

        public HomeController(ILogger<HomeController> logger, ApplicationContext ctx, ChallengeService challengeServ)
        {
            _logger = logger;
            _ctx = ctx;
            _challengeService = challengeServ;
        }

        public IActionResult Index()
        {
            var challenges = _challengeService.GetAllChallenges();

            return View(challenges);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
