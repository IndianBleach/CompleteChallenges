using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Project.Interfaces;

namespace Project.Models.SolutionEngines
{
    public class CsharpSolutionEngine : SolutionEngineBase
    {
        public SolutionResult BuildResult(string authorUsername, string test, string solution)
        {
            

            return base.BuildCsharpSolutionResult(authorUsername, test, solution);
        }

        //fix
        private string _handlerContent
        {
            get
            {
                using (FileStream fstream = File.OpenRead(@"./SolutionEngines/Csharp/cs-engine.cs"))
                {
                    byte[] array = new byte[fstream.Length];
                    fstream.Read(array, 0, array.Length);

                    string textFromFile = System.Text.Encoding.Default.GetString(array);
                    return textFromFile;
                }
            }
        }
    }
}
