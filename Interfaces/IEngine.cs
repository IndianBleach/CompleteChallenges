using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace Project.Interfaces
{
    public interface IEngine
    {
        public string ExecutedContent { get; set; }
        public bool CanUserSubmitSolution { get; set; }
        public StringBuilder ExecutedSolutionResult { get; set; }
        public void BuildSolutionResult(string testContent, string solutionContent);
    }
}
