using System;
using Sugar.Language.Lexing;

using Sugar.Language.Parsing;
using Sugar.Language.Parsing.Parser;

using Sugar.Language.Exceptions;

using Sugar.Language.Semantics.ActionTrees.Namespaces;

namespace Sugar.Language.Compilers
{
    internal sealed class FileCompiler : CompilerBase
    {
        private DefaultNameSpaceNode defaultNameSpace;
        private CreatedNameSpaceCollectionNode createdNameSpaces;

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
                if (collection.Count == 0)
                {
                    Console.WriteLine("No Output");
                    return true;
                }

                Console.WriteLine("Theres something here!");

                //analyso capricio

                return true;
            }
            catch (CompileException e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                return false;
            }
        }

        private SyntaxTreeCollection CreateSyntaxTree(string[] files)
        {
            var treeCollection = new SyntaxTreeCollection();

            foreach (var file in files)
            {
                var sugarFile = ReadFile(file);

                if (sugarFile.Exceptions.Count > 0)
                    PrintErrors(sugarFile);

                treeCollection.Add(sugarFile.SyntaxTree);
            }

            return treeCollection;
        }

    }
}
