using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Interfaces;
using Project.Models;
using Project.Models.ValidModels;
using Project.Services;

namespace Project.Controllers
{
    public class ChallengeController : Controller
    {
        private ApplicationContext _ctx;
        private ChallengeService _challengeService;
        private UserActivityService _activityService;

        public ChallengeController(ApplicationContext ctx, ChallengeService challengeService, UserActivityService activityServ)
        {
            _ctx = ctx;
            _challengeService = challengeService;
            _activityService = activityServ;
        }

        #region POST ENDPOINTS
        [Authorize]
        [HttpPost]
        public IActionResult Solve(string solutionLanguage, string solutionContent)
        {
            if ((solutionContent != null) && (solutionLanguage != null))
            { 
                
            }
            
            return RedirectToAction();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateChellange challengeModel)
        {
            if (ModelState.IsValid)
            {
                await _challengeService.CreateChallenge(User.Identity.Name, challengeModel);

                await _activityService.AddUserActivityEvent(User.Identity.Name, $"публ. {challengeModel.Name}");
            }
            return RedirectToAction("index");
        }
        #endregion

        #region GET ENDPOINTS
        [HttpGet]
        public IActionResult Solve(int? challenge)
        {
            Challenge findChallenge = _challengeService.GetChallengeById(challenge);

            if (findChallenge != null)
                return View("solve", findChallenge);

            return RedirectToAction("index");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Tags = _ctx.Tags.ToList();
            ViewBag.Levels = _ctx.ChallengeLevels.ToList();

            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_challengeService.GetAllChallenges());
        }
        #endregion
    }
}
