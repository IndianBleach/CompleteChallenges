using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Project.Models.SolutionEngines;

namespace Project.Interfaces
{
    public interface IEngine
    {
        public SolutionResult BuildSolutionResult(string authorUsername, string test, StringBuilder validatationContent, string solution);
    }

    public interface ISolutionResult
    {
        public string ResultContent { get; set; }
        public bool CanUserSubmitSolution { get; set; }
    }
}
