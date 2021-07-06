using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Models.ValidModels;

namespace Project.Controllers
{
    public class ChallengeController : Controller
    {
        private ApplicationContext _ctx;

        public ChallengeController(ApplicationContext ctx)
        {
            _ctx = ctx;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateChellange challengeModel)
        {
            if (ModelState.IsValid)
            {
                //build-log
                CsharpBuilder includeCsharpTest = challengeModel.CsharpTest != null ? new CsharpBuilder() : null;
                PythonBuilder includePythonTest = challengeModel.PythonTest != null ? new PythonBuilder() : null;

                //build challenge tag list
                List<Tag> tagList = new List<Tag>();
                foreach (var tagName in challengeModel.Tags)
                {
                    tagList.Add(_ctx.Tags.FirstOrDefault(x =>
                        x.Name == tagName));
                }

                // +challenge
                Challenge newChallenge = new Challenge
                {
                    Author = await _ctx.Users.FirstOrDefaultAsync(x =>
                        x.Username == User.Identity.Name),
                    Description = challengeModel.Description,
                    Name = challengeModel.Name,
                    Score = 0,
                    DateCreated = DateTime.Now,
                    Level = _ctx.ChallengeLevels.FirstOrDefault(x =>
                        x.Name == challengeModel.Level),
                    Likes = new List<ChallengeLike>(),
                    Solutions = new List<Solution>(),
                    Tags = tagList,
                    UsersWhoUnlocked = new List<User>(),
                    Tests = new List<Test>()
                };

                // +set tests to challenge                                               
                List<Test> challengeTests = new List<Test>();
                if (includeCsharpTest != null)
                {
                    challengeTests.Add(
                        includeCsharpTest.BuildTest(
                        newChallenge,
                        await _ctx.ProgramLanguages.FirstOrDefaultAsync(x =>
                            x.Name == "C#"),
                        challengeModel.CsharpTest
                        )
                    );
                }

                if (includePythonTest != null)
                {
                    challengeTests.Add(
                        includePythonTest.BuildTest(
                        newChallenge,
                        await _ctx.ProgramLanguages.FirstOrDefaultAsync(x =>
                            x.Name == "Python"),
                        challengeModel.PythonTest
                        )
                    );
                }
                //render response
                await _ctx.Tests.AddRangeAsync(challengeTests);

                await _ctx.SaveChangesAsync();
            }
            return RedirectToAction("index");
        }


        public async Task<IActionResult> Solve(int? challenge)
        {
            if (challenge != null)
            {
                Challenge findChallenge = await _ctx.Challenges
                    .Include(x => x.Author)
                    .Include(x => x.Solutions)
                    .Include(x => x.Tags)
                    .Include(x => x.Level)
                    .Include(x => x.Tests)
                    .ThenInclude(x => x.ProgLanguage)
                    .Include(x => x.Likes)
                    .Include(x => x.UsersWhoUnlocked)
                    .FirstOrDefaultAsync(x => x.Id == challenge);

                if (findChallenge != null)
                {
                    return View("solve", findChallenge);
                }
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
        public async Task<IActionResult> Index()
        {
            List<Challenge> challenges = await _ctx.Challenges
                .Include(x => x.Author)
                    .Include(x => x.Solutions)
                    .Include(x => x.Tags)
                    .Include(x => x.Level)
                    .Include(x => x.Tests)
                    .ThenInclude(x => x.ProgLanguage)
                    .Include(x => x.Likes)
                    .Include(x => x.UsersWhoUnlocked)
                    .ToListAsync();

            return View(challenges);
        }
    }
}
