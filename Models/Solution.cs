using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Solution
    {
        public int Id { get; set; }
        public Challenge Challenge { get; set; }
        public User Author { get; set; }
        public string SolutionContent { get; set; }
        public ProgramLanguage ProgLanguage { get; set; }
        public DateTime DateCreated { get; set; }
        
        public Solution(User author, ProgramLanguage lang, string solutionContent)
        {
            Author = author;
            ProgLanguage = lang;
            SolutionContent = solutionContent;
        }

        public Solution()
        {             
        }
    }
}
