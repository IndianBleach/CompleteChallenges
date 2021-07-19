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
        private UserSolutionService _solutionService;

        public ChallengeController(ApplicationContext ctx, ChallengeService challengeService, UserActivityService activityServ, UserSolutionService solServ)
        {
            _ctx = ctx;
            _challengeService = challengeService;
            _activityService = activityServ;
            _solutionService = solServ;
        }

        [Authorize]
        public IActionResult Unlock(int? challenge, string lang)
        { 
            if (challenge != null)
            {
                Challenge getChallenge = _challengeService.GetChallenge(challenge);

                if (!_challengeService.IsUserUnlocked(getChallenge.Id, User.Identity.Name))
                {
                    bool isUpdated = _challengeService.TryUpdateScore(getChallenge.Id, User.Identity.Name, true);
                    if (isUpdated)
                    {
                        _challengeService.AddUnlockUser(getChallenge, User.Identity.Name);

                        ViewBag.Unlocked = true;
                    }
                    else ViewBag.ErrorUserScore = true;

                    ViewBag.LangName = lang;
                    ViewBag.Unlocked = isUpdated;

                    return View("solve", getChallenge);
                }
                else
                {
                    ViewBag.Section = "solutions";
                    ViewBag.LangName = lang;
                    ViewBag.Unlocked = true;
                }
            }
            return RedirectToAction("index");
        }

        #region POST ENDPOINTS
        [Authorize]
        [HttpPost]
        public IActionResult AddReport(int? challenge, string lang, string reportContent)
        {
            if (challenge != null)
            {
                Challenge updatedChallenge = _challengeService.AddChallengeReport(
                    challenge, User.Identity.Name, reportContent);

                ViewBag.LangName = lang;
                ViewBag.Unlocked = true;
                ViewBag.Section = "comments";

                return View("solve", updatedChallenge);
            }
            return RedirectToAction("index");
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddComment(int? challengeId, string commentContent, string lang)
        {
            if (challengeId != null)
            {
                Challenge updatedChallenge = _challengeService.AddChallengeComment(
                    (int)challengeId, commentContent, User.Identity.Name); 

                ViewBag.Section = "solutions";
                ViewBag.LangName = lang;
                ViewBag.Unlocked = true;

                return View("solve", updatedChallenge);
            }
            return RedirectToAction("index");
        }

        [HttpPost]
        [Authorize]
        public IActionResult Submit(string solution, int? testId)
        {
            if (testId != null)
            {                
                SolutionResult result = _solutionService.GetSolutionResult(solution, (int)testId, User.Identity.Name);
                if (result.CanUserSubmitSolution == true)
                {
                    User user = _ctx.Users.FirstOrDefault(x => x.Username == User.Identity.Name);

                    var obj = _ctx.Tests
                    .Include(x => x.ProgLanguage)
                    .Include(x => x.Challenge)
                    .FirstOrDefault(x => x.Id == testId);

                    Challenge challenge = _challengeService.GetChallengeById(obj.Challenge.Id);

                    _challengeService.SubmitSolution(challenge, user, solution, obj.ProgLanguage);

                    ViewBag.Section = "solutions";
                    ViewBag.LangName = obj.ProgLanguage.Name;
                    ViewBag.Unlocked = true;

                    return View("solve", challenge);
                }
            }
            return RedirectToAction("index");
        }


        [Authorize]
        [HttpPost]
        public IActionResult Solve(string solution, int? testId)
        {
            if (testId != null)
            {
                SolutionResult result = _solutionService.GetSolutionResult(solution, (int)testId, User.Identity.Name);

                //need fix
                var obj = _ctx.Tests
                    .Include(x => x.ProgLanguage)
                    .Include(x => x.Challenge)
                    .FirstOrDefault(x => x.Id == testId);

                ViewBag.Res = result.ResultContent;
                ViewBag.OldSolution = solution;
                ViewBag.SolutionStatus = result.CanUserSubmitSolution;
                ViewBag.LangName = obj.ProgLanguage.Name;

                Challenge challenge = _challengeService.GetChallengeById(obj.Challenge.Id);

                return View("Solve", challenge);
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

                await _activityService.AddActivity(User.Identity.Name, $"публ. {challengeModel.Name}");
            }
            return RedirectToAction("index");
        }
        #endregion

        #region GET ENDPOINTS
        [HttpGet]
        public IActionResult Solve(int? challenge, string lang, string? section)
        {
            Challenge findChallenge = _challengeService.GetChallengeById(challenge);

            if (section == null)
                section = "solutions";        

            if (findChallenge != null)
            {
                ViewBag.Section = section;

                ViewBag.LangName = lang;

                var checkUserUnlock = findChallenge.UsersWhoUnlocked.FirstOrDefault(x => x.Username == User.Identity.Name);
                if (checkUserUnlock != null)
                    ViewBag.Unlocked = true;

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
