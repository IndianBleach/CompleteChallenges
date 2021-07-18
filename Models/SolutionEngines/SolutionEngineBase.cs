using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BaselineTypeDiscovery;
using IronPython.Hosting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Scripting.Hosting;
using Project.Interfaces;
using System.Runtime;
using System.Runtime.Loader;
using System.Reflection.Emit;
using System.Threading;

namespace Project.Models.SolutionEngines
{
    public class CustomAssemblyLoadContext2 : AssemblyLoadContext
    {
        public CustomAssemblyLoadContext2() : base(isCollectible: true) { }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            return null;
        }
    }

    public class SolutionEngineBase
    {
        private protected SolutionResult BuildPythonSolutionResult(string authorUsername, string test, string validatationContent, string solution)
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

        private protected SolutionResult BuildCsharpSolutionResult(string authorUsername, string test, string solution)
        {
            string filePath = $"./UserSolutions/exe-by-{authorUsername}.txt";
            string mainCompilledClassName = $"{authorUsername}SolutionMain";
            string authorSignClass = $"public class {mainCompilledClassName} {{\n ";


            #region BUILD FILE TEXT CONTENT

            /* build plan
                1. Decor {
                2. Solution & author namespace
                3. Tests
                4. Validate
                5. }
             */
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    stream.Close();

                    using (StreamWriter sw = new StreamWriter(filePath, false))
                    {
                        string includedSources = File.ReadAllText($"./SolutionEngines/Csharp/SolutionWrapper.txt");
                        sw.WriteLine(includedSources);

                        sw.WriteLine(authorSignClass);

                        sw.WriteLine("public List<object> TestSolution(){\ntry{\n");
                        sw.WriteLine(solution);

                        sw.WriteLine(test);

                        string solutionValidMiddleware = File.ReadAllText($"./SolutionEngines/Csharp/SolutionValidMiddleware.txt");
                        sw.WriteLine(solutionValidMiddleware);

                        string testMethodEnd = @"}
                        catch (Exception exp)
                        {
                            return new List<object> { exp.Message, false };
                        }
                        }";
                        sw.WriteLine(testMethodEnd);

                        sw.WriteLine("\t}");

                        sw.Close();
                    }
                }
                #endregion

                var compilation = CSharpCompilation.Create($"SolutionBy{authorUsername}")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(
                    MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location))
                .AddSyntaxTrees(CSharpSyntaxTree.ParseText(
                    System.IO.File.ReadAllText(filePath)));

                #region BUILD MEMORY ASSEMBLY BY SOLUTION AUTHOR
                List<object> res;
                using (var stream = new MemoryStream())
                {
                    var result = compilation.Emit(stream);
                    if (!result.Success)
                    {
                        throw new InvalidOperationException();
                    }
                    var assembly = Assembly.Load(stream.GetBuffer());

                    var instance = assembly.CreateInstance(mainCompilledClassName);

                    res = (List<object>)assembly.GetType(mainCompilledClassName).GetMethod("TestSolution").Invoke(instance, null);

                    stream.Dispose();
                    stream.Close();
                }
                #endregion

                // result content | can user submit solution
                var resultPart = res.ToArray();

                StringBuilder resContent = new StringBuilder();
                foreach (var item in (List<string>)resultPart[0])
                {
                    resContent.Append(item);
                }

                return new SolutionResult { CanUserSubmitSolution = (bool)resultPart[1], ResultContent = resContent.ToString() };
            }
            catch (Exception exp)
            {
                return new SolutionResult { CanUserSubmitSolution = false, ResultContent = "some engine solution error" };
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
