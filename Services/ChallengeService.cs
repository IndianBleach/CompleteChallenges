using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Models;
using Project.Models.ValidModels;
using Microsoft.EntityFrameworkCore;
using Project.Models.SolutionEngines;

namespace Project.Services
{
    public class ChallengeService
    {
        private CsharpBuilder _csBuilder { get; set; }
        private PythonBuilder _pyBuilder { get; set; }
        private ApplicationContext _ctx;
        private UserSolutionService _solutionService;
        public ChallengeService(ApplicationContext ctx, CsharpBuilder csharpBuilder, PythonBuilder pythonBuilder, UserSolutionService solServ)
        {
            _ctx = ctx;
            _csBuilder = csharpBuilder;
            _pyBuilder = pythonBuilder;
        }

        #region SOLUTION SERVICE

        /*
        public SolutionResult TestSolution(string solution, Test test, string username)
        {
            SolutionResult result = _solutionService.GetSolutionResult(solution, test.Id, User.Identity.Name);
        }
        */

        public void SubmitSolution(Challenge challenge, User user, string solution, ProgramLanguage lang)
        {
            var checkAlreadySolved = user.MySolutions.FirstOrDefault(x => 
            x.Challenge.Id == challenge.Id && x.ProgLanguage.Name == lang.Name);

            bool checkChallengeAuthor = challenge.Author == user ? true : false;

            if (checkAlreadySolved == null && checkChallengeAuthor == false)
            {
                challenge.UsersWhoUnlocked.Add(user);
                user.Score += challenge.Level.Score;                
            }

            if (checkAlreadySolved == null)
            {
                _ctx.Solutions.Add(new Solution
                {
                    SolutionContent = solution,
                    Author = user,
                    Challenge = challenge,
                    DateCreated = DateTime.Now,
                    ProgLanguage = lang
                });
            }

            _ctx.SaveChanges();
        }
        #endregion

        public Challenge AddChallengeComment(int challengeId, string content, string authorUsername)
        {
            Challenge challenge = GetChallenge(challengeId);

            _ctx.ChallengeComments.Add(new ChallengeComment()
            {
                Author = _ctx.Users.FirstOrDefault(x =>
                    x.Username == authorUsername),
                Content = content,
                CreatedDateStr = DateTime.Now.ToShortDateString(),
                OnChallenge = challenge
            });

            _ctx.SaveChanges();

            return challenge;
        }

        public Challenge AddChallengeReport(int? challengeId, string authorUsername, string reportContent)
        {
            if (challengeId != null)
            {
                Challenge getChallenge = GetChallenge(challengeId);

                if (getChallenge != null)
                {
                    _ctx.ChallengeReports.Add(new ChallengeReport()
                    {
                        Content = reportContent,
                        Author = _ctx.Users.FirstOrDefault(x => x.Username == authorUsername),
                        OnChallenge = getChallenge
                    });

                    _ctx.SaveChanges();

                    return getChallenge;
                }               
            }
            return null;
        }

        public Challenge GetChallenge(int? id)
        {
            if (id != null)
            {
                Challenge getChallenge = _ctx.Challenges
                    .Include(x => x.Author)
                    .Include(x => x.Solutions)
                    .Include(x => x.Tags)
                    .Include(x => x.Comments)
                    .ThenInclude(x => x.Author)
                    .Include(x => x.Level)
                    .Include(x => x.Tests)
                    .ThenInclude(x => x.ProgLanguage)
                    .Include(x => x.Likes)
                    .Include(x => x.UsersWhoUnlocked)
                    .Include(x => x.Reports)
                    .FirstOrDefault(x => x.Id == id);

                return getChallenge;
            }
            else return null;
        }

        public bool IsUserUnlocked(int challengeId, string username)
        {
            Challenge getChallenge = _ctx.Challenges
                .Include(x => x.UsersWhoUnlocked)
                .FirstOrDefault(x => x.Id == challengeId);

            bool isExist = getChallenge.UsersWhoUnlocked.Contains(
                _ctx.Users.FirstOrDefault(x => x.Username == username));

            return isExist;
        }

        public bool TryUpdateScore(int challengeId, string username, bool minus)
        {
            Challenge getChallenge = _ctx.Challenges
                .Include(x => x.Level)
                .FirstOrDefault(x => x.Id == challengeId);

            User user = _ctx.Users
                .FirstOrDefault(x => x.Username == username);

            if (user.Score >= getChallenge.Level.Score)
            {
                user.Score = minus ? user.Score - getChallenge.Level.Score :
                user.Score + getChallenge.Level.Score;

                _ctx.SaveChanges();

                return true;
            }
            else return false;
        }

        public void AddUnlockUser(Challenge challenge, string username)
        {
            challenge.UsersWhoUnlocked.Add(_ctx.Users.FirstOrDefault(x =>
                x.Username == username));

            _ctx.SaveChanges();
        }

        public Challenge GetChallengeById(int? id)
        {
            if (id != null)
            {
                Challenge findChallenge = _ctx.Challenges
                    .Include(x => x.Author)
                    .Include(x => x.Solutions)
                    .Include(x => x.Tags)
                    .Include(x => x.Comments)
                    .ThenInclude(x => x.Author)
                    .Include(x => x.Level)
                    .Include(x => x.Tests)
                    .ThenInclude(x => x.ProgLanguage)
                    .Include(x => x.Likes)
                    .Include(x => x.UsersWhoUnlocked)
                    .Include(x => x.Reports)
                    .FirstOrDefault(x => x.Id == id);

                return findChallenge;
            }
            else return null;
        }

        public List<Challenge> GetAllChallenges()
        {
            List<Challenge> challenges = _ctx.Challenges
                .Include(x => x.Author)
                .Include(x => x.Solutions)
                .Include(x => x.Tags)
                .Include(x => x.Comments)
                .Include(x => x.Level)
                .Include(x => x.Reports)
                .Include(x => x.Tests)
                    .ThenInclude(x => x.ProgLanguage)
               .Include(x => x.Likes)
               .Include(x => x.UsersWhoUnlocked)
               .ToList();

            return challenges;
        }

        public async Task CreateChallenge(string authorUsername, CreateChellange challengeModel)
        {
            bool includeCsharp = challengeModel.CsharpTest != null ? true : false;
            bool includePython = challengeModel.PythonTest != null ? true : false;

            #region BUILD TAG LIST
            //build challenge tag list
            List<Tag> tagList = new List<Tag>();
            foreach (var tagName in challengeModel.Tags)
            {
                tagList.Add(_ctx.Tags.FirstOrDefault(x =>
                    x.Name == tagName));
            }
            #endregion

            #region BUILD EMPTY CHALLENGE MODEL
            Challenge newChallenge = new Challenge
            {
                Author = _ctx.Users.FirstOrDefault(x =>
                    x.Username == authorUsername),
                Description = challengeModel.Description,
                Name = challengeModel.Name,
                DateCreated = DateTime.Now,
                Level = _ctx.ChallengeLevels.FirstOrDefault(x =>
                    x.Name == challengeModel.Level),
                Likes = new List<ChallengeLike>(),
                Solutions = new List<Solution>(),
                Comments = new List<ChallengeComment>(),
                Reports = new List<ChallengeReport>(),
                Tags = tagList,
                UsersWhoUnlocked = new List<User>(),
                Tests = new List<Test>()
            };
            #endregion
            
            #region BUILD CHALLENGE TESTS  
            List<Test> challengeTests = new List<Test>();
            if (includeCsharp)
            {
                challengeTests.Add(
                    _csBuilder.BuildTest(
                    newChallenge,
                    _ctx.ProgramLanguages.FirstOrDefault(x =>
                        x.Name == "Csharp"),
                    challengeModel.CsharpTest
                    )
                );
            }
            

            if (includePython)
            {
                challengeTests.Add(
                    _pyBuilder.BuildTest(
                    newChallenge,
                    _ctx.ProgramLanguages.FirstOrDefault(x =>
                        x.Name == "Python"),
                    challengeModel.PythonTest
                    )
                );
            }
            #endregion

            await _ctx.Tests.AddRangeAsync(challengeTests);
            
            await _ctx.SaveChangesAsync();
        }
    }
}
