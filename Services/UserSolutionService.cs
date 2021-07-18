using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Models.SolutionEngines;

namespace Project.Services
{
    public class UserSolutionService
    {
        private ApplicationContext _ctx;
        private ChallengeService _challengeService;

        //todo- create service after solution engines
        public UserSolutionService(ApplicationContext ctx, ChallengeService challengeServ)
        {
            _ctx = ctx;
            _challengeService = challengeServ;
        }

        public SolutionResult GetSolutionResult(string solution, int testId, string username)
        {
            if (!string.IsNullOrWhiteSpace(solution))
            {
                var obj = _ctx.Tests
                    .Include(x => x.ProgLanguage)
                    .Include(x => x.Challenge)
                    .FirstOrDefault(x => x.Id == testId);

                var challenge = _challengeService.GetChallengeById(obj.Challenge.Id);

                //need fix
                SolutionResult solResult = (obj.ProgLanguage.Name) switch
                {
                    "Python" => solResult = new PythonSolutionEngine().BuildResult(username, obj.TestContent, solution),
                    "Csharp" => solResult = new CsharpSolutionEngine().BuildResult(username, obj.TestContent, solution),
                    _ => new SolutionResult { CanUserSubmitSolution = false, ResultContent = "solution engine error" }
                };

                return solResult;
            }
            else
                return new SolutionResult { CanUserSubmitSolution = false, ResultContent = "solution engine error" };
        }
    }
}
