using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    #region C# MODELS
    public class CsharpBuilder : Builder
    {
        public override Solution BuildSolution(User author, ProgramLanguage lang, string solutionContent)
        {
            return new CsharpSolution(author, lang, solutionContent);
        }

        public override Test BuildTest(Challenge challenge, ProgramLanguage lang, string testContent)
        {
            return new CsharpTest(challenge, lang, testContent);
        }
    }

    public class CsharpSolution : Solution
    {
        public CsharpSolution(User author, ProgramLanguage lang, string solutionContent)
            : base(author, lang, solutionContent)
        {
        }
    }

    public class CsharpTest : Test
    {
        public CsharpTest(Challenge challenge, ProgramLanguage lang, string testContent)
            : base(challenge, lang, testContent)
        {
        }

        public CsharpTest()
        { 
        }
    }
    #endregion
}