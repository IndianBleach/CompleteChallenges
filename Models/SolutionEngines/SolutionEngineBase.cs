using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Project.Interfaces;

namespace Project.Models.SolutionEngines
{
    public class SolutionEngineBase //: IEngine
    {
        private protected SolutionResult BuildSolutionResult(string authorUsername, string test, StringBuilder validatationContent, string solution)
        {
            string filePath = $"./UserSolutions/exe-by-{authorUsername}.py";
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    stream.Close();
                }

                using (StreamWriter sw = new StreamWriter(filePath, false))
                {
                    sw.WriteLine(solution);
                    sw.WriteLine(test);
                    sw.WriteLine(validatationContent);
                    sw.Close();
                }

                ScriptEngine engine = Python.CreateEngine();
                ScriptScope scope = engine.CreateScope();

                engine.ExecuteFile(filePath, scope);
                dynamic getExecuteResult = scope.GetVariable("getResult");
                dynamic response = getExecuteResult();

                StringBuilder resultStrBuilder = new StringBuilder();
                foreach (var item in response) resultStrBuilder.Append(item);

                SolutionResult solutionResult = new SolutionResult()
                {
                    CanUserSubmitSolution = scope.GetVariable("canUserSubmitSolution"),
                    ResultContent = resultStrBuilder.ToString(),
                };

                //System.IO.File.Delete(filePath);

                return solutionResult;
            }
            catch (Exception exp)
            {
                return new SolutionResult()
                {
                    CanUserSubmitSolution = false,
                    ResultContent = exp.Message
                };
            }

            finally
            {
                if (solution != null) System.IO.File.Delete(filePath);
            }            
        }

    }

    public struct SolutionResult : ISolutionResult
    { 
        public string ResultContent { get; set; }
        public bool CanUserSubmitSolution { get; set; }
    }
}
