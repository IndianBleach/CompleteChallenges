using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class UserRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public ICollection<Solution> MySolutions { get; set; }
        public ICollection<Challenge> MyChallenges { get; set; }
        public ICollection<Challenge> UnlockedChallenges { get; set; }
        public int RoleId { get; set; }
        public UserRole Role { get; set; }
        public DateTime DateCreated { get; set; }
    }
    //END USER

    public class ProgramLanguage
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Test> Tests { get; set; }
        public ICollection<Solution> Solutions { get; set; }
    }

    public abstract class Test
    {
        public int Id { get; set; }
        public Challenge Challenge { get; set; }
        public User Author { get; set; }
        public ProgramLanguage ProgLanguage { get; set; }
        public string TestContent { get; set; }
        public DateTime DateCreated { get; set; }
        public Test(User author, ProgramLanguage lang, string solutionContent)
        {
            Author = author;
            ProgLanguage = lang;
            TestContent = solutionContent;
        }
    }

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
    }

    public abstract class Builder
    {
        abstract public Solution BuildSolution(User author, ProgramLanguage lang, string solutionContent);
        abstract public Test BuildTest(User author, ProgramLanguage lang, string testContent);
    }


    #region C# MODELS
    public class CsharpBuilder : Builder
    {
        public override Solution BuildSolution(User author, ProgramLanguage lang, string solutionContent)
        {
            return new CsharpSolution(author, lang, solutionContent);
        }

        public override Test BuildTest(User author, ProgramLanguage lang, string testContent)
        {
            return new CsharpTest(author, lang, testContent);
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
        public CsharpTest(User author, ProgramLanguage lang, string testContent)
            : base(author, lang, testContent)
        {
        }
    }
    #endregion

    #region PYTHON MODELS
    public class PythonBuilder : Builder
    {
        public override Solution BuildSolution(User author, ProgramLanguage lang, string solutionContent)
        {
            return new PythonSolution(author, lang, solutionContent);
        }

        public override Test BuildTest(User author, ProgramLanguage lang, string testContent)
        {
            return new PythonTest(author, lang, testContent);
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
        public PythonTest(User author, ProgramLanguage lang, string testContent)
            : base(author, lang, testContent)
        {
        }
    }
    #endregion


    //CHALLENGE
    public class Challenge
    { 
        public int Id { get; set; }
        public User Author { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Solution> Solutions { get; set; }
        public ICollection<Test> Tests { get; set; }
        public ICollection<User> UsersWhoUnlocked { get; set; }
        public DateTime DateCreated { get; set; }
    }

}
