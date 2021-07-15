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

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationContext _ctx;

        public HomeController(ILogger<HomeController> logger, ApplicationContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        public async Task<IActionResult> Index()
        {
            List<Challenge> challenges = await _ctx.Challenges
                .Include(x => x.Author)
                .Include(x => x.Tests)
                .ThenInclude(x => x.ProgLanguage)
                .Include(x => x.Likes)
                .Include(x => x.Tags)
                .Include(x => x.Solutions)
                .Include(x => x.Level)
                .ToListAsync();

            //viewbag discusses

            return View(challenges);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
