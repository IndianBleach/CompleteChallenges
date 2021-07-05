using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    #region PYTHON MODELS
    public class PythonBuilder : Builder
    {
        public override Solution BuildSolution(User author, ProgramLanguage lang, string solutionContent)
        {
            return new PythonSolution(author, lang, solutionContent);
        }

        public override Test BuildTest(Challenge challenge, ProgramLanguage lang, string testContent)
        {
            return new PythonTest(challenge, lang, testContent);
        }
    }

    public class PythonSolution : Solution
    {
        public PythonSolution(User author, ProgramLanguage lang, string solutionContent)
            : base(author, lang, solutionContent)
        {
        }
    }

    public class PythonTest : Test
    {
        public PythonTest(Challenge challenge, ProgramLanguage lang, string testContent)
            : base(challenge, lang, testContent)
        {
        }

        public PythonTest()
        { 
        }
    }
    #endregion
}