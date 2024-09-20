using System;

using Sugar.Language.Exceptions;

using Sugar.Language.Lexing;

using Sugar.Language.Parsing.Parser;

using Sugar.Language.Analysis;
using Sugar.Language.Analysis.ProjectStructure.ProjectNodes;

namespace Sugar.Language.Compilers
{
    internal sealed class FileCompiler : CompilerBase
    {
        public FileCompiler()
        {
            Lexer.CreateInstance();
            Parser.CreateInstance();
            SemanticAnalyser.CreateInstance();
        }

        public bool Compile(string[] source, string[] wrappers)
        {
            try
            {
                SugarFileCollection collection = CreateSyntaxTree(source);
                if (collection.Length == 0)
                {
                    Console.WriteLine("No Output");
                    return true;
                }

                /*SugarFileCollection wrapperTree = CreateSyntaxTree(wrappers);
                if(!wrapperTree.Valid)
                {
                    wrapperTree.PrintExceptions();
                    return false;
                }*/
                
                collection.SyntaxTree[1].Print();
        
                /*ProjectTree tree = SemanticAnalyser.Instance.WithTrees(null, collection.SyntaxTree).Analyse();
                tree.Print("", true);*/

                return true;
            }
            catch (CompileException e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                return false;
            }
        }

        private SugarFileCollection CreateSyntaxTree(string[] files)
        {
            var treeCollection = new SugarFileCollection();

            foreach (var file in files)
            {
                var p = ReadFile(file);
                if (p.ExceptionCount > 0)
                    p.PrintExceptions();

                treeCollection.Add(p);
            }

            return treeCollection;
        }
    }
}
