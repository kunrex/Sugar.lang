using System;
using System.Collections.Generic;

using Sugar.Language.Lexing;
using Sugar.Language.Exceptions;
using Sugar.Language.Parsing.Parser;

using Sugar.Language.Semantics.Analysis;
using Sugar.Language.Parsing;

namespace Sugar.Language
{
    public sealed class Compiler
    {
        private readonly string[] sourceFiles;

        public Compiler(string source)
        {
            sourceFiles = new string[] { source };
        }

        public Compiler(List<string> source)
        {
            sourceFiles = source.ToArray();
        }

        public bool Compile()
        {
            try
            {
                var treeCollection = new SyntaxTreeCollection();
                foreach(var source in sourceFiles)
                {
                    var tokens = new Lexer(source).Lex();

                    var tree = new Parser(tokens).Parse();
                    if (tree == null)
                    {
                        Console.WriteLine("Source Code Empty");
                        continue;
                    }

                    treeCollection.Add(tree);
                    tree.BaseNode.Print("", true);
                }

                if(treeCollection.Count == 0)
                {
                    Console.WriteLine("No Output");
                    return true;
                }    

                /*Console.WriteLine("\n_____Analysed_____");

                var analysisResult = new SemanticAnalyser(treeCollection).Analyse();

                Console.WriteLine(analysisResult);*/
                return true;
            }
            catch(CompileException e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                return false;
            }
        }
    }
}
