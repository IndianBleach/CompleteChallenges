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
using Project.Models.SolutionEngines;
using System.Text;

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
        public IActionResult Solve(string solution, int? testId)
        {            
            if (testId != null)
            {
                //need fix
                var obj = _ctx.Tests
                    .Include(x => x.ProgLanguage)
                    .Include(x => x.Challenge)
                    .FirstOrDefault(x => x.Id == testId);

                var challenge = _challengeService.GetChallengeById(obj.Challenge.Id);

                //need fix
                SolutionResult solResolt = (obj.ProgLanguage.Name) switch
                {
                    "Python" => solResolt = new PythonSolutionEngine().BuildResult(User.Identity.Name, obj.TestContent, solution),
                    "Csharp" => solResolt = new CsharpSolutionEngine().BuildResult(User.Identity.Name, obj.TestContent, solution),
                    _ => new SolutionResult { CanUserSubmitSolution = false, ResultContent = "solution engine error"}
                };

                ViewBag.Res = solResolt.ResultContent;
                ViewBag.OldSolution = solution;
                ViewBag.LangName = obj.ProgLanguage.Name;

                return View("Solve", challenge);

                /*
                if (obj.ProgLanguage.Name == "Python")
                {
                    var engine = new PythonSolutionEngine();
                    var res = engine.BuildResult(User.Identity.Name, obj.TestContent, solution);
                    ViewBag.Res = res.ResultContent;
                    ViewBag.OldSolution = solution;
                    ViewBag.LangName = obj.ProgLanguage.Name;

                    return View("Solve", challenge);
                }
                else if (obj.ProgLanguage.Name == "Csharp")
                {
                    var engine = new CsharpSolutionEngine();
                    var res = engine.BuildResult(User.Identity.Name, obj.TestContent, solution);
                    ViewBag.Res = res.ResultContent;
                    ViewBag.OldSolution = solution;
                    ViewBag.LangName = obj.ProgLanguage.Name;

                    return View("Solve", challenge);
                }
                */
            }            
            return RedirectToAction("index");
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
        public IActionResult Solve(int? challenge, string lang)
        {
            Challenge findChallenge = _challengeService.GetChallengeById(challenge);

            if (findChallenge != null)
            {
                ViewBag.LangName = lang;               

                return View("solve", findChallenge);
            }

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
