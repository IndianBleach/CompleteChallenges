using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Models;
using Project.Models.ValidModels;
using Microsoft.EntityFrameworkCore;

namespace Project.Services
{
    public class ChallengeService
    {
        private CsharpBuilder _csBuilder { get; set; }
        private PythonBuilder _pyBuilder { get; set; }
        private ApplicationContext _ctx;

        public ChallengeService(ApplicationContext ctx, CsharpBuilder csharpBuilder, PythonBuilder pythonBuilder)
        {
            _ctx = ctx;
            _csBuilder = csharpBuilder;
            _pyBuilder = pythonBuilder;
        }

        public Challenge GetChallengeById(int? id)
        {
            if (id != null)
            {
                Challenge findChallenge = _ctx.Challenges
                    .Include(x => x.Author)
                    .Include(x => x.Solutions)
                    .Include(x => x.Tags)
                    .Include(x => x.Level)
                    .Include(x => x.Tests)
                    .ThenInclude(x => x.ProgLanguage)
                    .Include(x => x.Likes)
                    .Include(x => x.UsersWhoUnlocked)
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
                .Include(x => x.Level)
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
            #endregion

            #region BUILD CHALLENGE TESTS  
            List<Test> challengeTests = new List<Test>();
            if (includeCsharp)
            {
                challengeTests.Add(
                    _csBuilder.BuildTest(
                    newChallenge,
                    _ctx.ProgramLanguages.FirstOrDefault(x =>
                        x.Name == "C#"),
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
