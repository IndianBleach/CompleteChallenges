using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.SolutionEngines
{
    public class PythonSolutionEngine : SolutionEngineBase
    {
        public SolutionResult BuildResult(string authorUsername, string test, string solution)
        {
            return base.BuildSolutionResult(authorUsername, test, _handlerContent, solution);
        }

        //fix
        private StringBuilder _handlerContent
        {
            get
            {
                using (FileStream fstream = File.OpenRead(@"./SolutionEngines/Python/py-engine.py"))
                {
                    byte[] array = new byte[fstream.Length];
                    fstream.Read(array, 0, array.Length);

                    string textFromFile = System.Text.Encoding.Default.GetString(array);
                    var builder = new StringBuilder();
                    builder.Append(textFromFile);
                    return builder;
                }
            }
        }    
    }
}
