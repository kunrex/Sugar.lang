using System;
using Sugar.Language.Lexing;

using Sugar.Language.Parsing.Parser;

using Sugar.Language.Exceptions;

using Sugar.Language.Semantics.Analysis;
using Sugar.Language.Semantics.ActionTrees.Namespaces;

namespace Sugar.Language.Compilers
{
    internal sealed class FileCompiler : CompilerBase
    {
        public FileCompiler()
        {
            Lexer.CreateInstance();
            Parser.CreateInstance();
        }

        public bool Compile(string[] source, string[] wrappers)
        {
            try
            {
                var collection = CreateSyntaxTree(source);
                if (collection.Length == 0)
                {
                    Console.WriteLine("No Output");
                    return true;
                }

                var wrapperCollection = CreateSyntaxTree(wrappers);
                if(!collection.Valid)
                {
                    collection.PrintExceptions();
                    return false;
                }

                var fileTree = collection.SyntaxTree;
                var wrapperTree = wrapperCollection.SyntaxTree;

                SemanticAnalyser.CreateInstance();
                SemanticAnalyser.Instance.Analyse(wrapperTree, fileTree);

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
                treeCollection.Add(ReadFile(file));

            return treeCollection;
        }
    }
}
