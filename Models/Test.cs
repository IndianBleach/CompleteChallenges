using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public abstract class Test
    {
        public int Id { get; set; }
        public Challenge Challenge { get; set; }
        public ProgramLanguage ProgLanguage { get; set; }
        public string TestContent { get; set; }
        public DateTime DateCreated { get; set; }
        public Test(Challenge challenge, ProgramLanguage lang, string solutionContent)
        {
            Challenge = challenge;
            ProgLanguage = lang;
            TestContent = solutionContent;
        }
    }
}
