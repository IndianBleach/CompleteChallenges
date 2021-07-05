using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public abstract class Builder
    {
        abstract public Solution BuildSolution(User author, ProgramLanguage lang, string solutionContent);
        abstract public Test BuildTest(Challenge challenge, ProgramLanguage lang, string testContent);
    }
}