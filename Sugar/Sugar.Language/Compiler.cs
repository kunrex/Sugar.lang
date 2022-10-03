using System;
using System.Collections.Generic;

using Sugar.Language.Lexing;
using Sugar.Language.Exceptions;
using Sugar.Language.Parsing.Parser;

using Sugar.Language.Parsing;
using Sugar.Language.Semantics.Analysis;

namespace Sugar.Language
{
    public sealed class Compiler
    {
        private readonly string[] sourceFiles;
        private readonly string[] internalDataTypes;

        public Compiler(List<string> source, List<string> defaultDataTypes)
        {
            sourceFiles = source.ToArray();
            internalDataTypes = defaultDataTypes.ToArray();
        }

        public bool Compile()
        {
            try
            {
                var parsedFiles = CreateSyntaxTree(sourceFiles);
                if (parsedFiles.Count == 0)
                {
                    Console.WriteLine("No Output");
                    return true;
                }

                var defaultDataTypes = CreateSyntaxTree(internalDataTypes);

                Console.WriteLine("\n_____Analysed_____");

                var analysisResult = new SemanticAnalyser(defaultDataTypes, parsedFiles).Analyse();

                Console.WriteLine(analysisResult);
                return true;
            }
            catch(CompileException e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                return false;
            }
        }

        private SyntaxTreeCollection CreateSyntaxTree(string[] source)
        {
            var treeCollection = new SyntaxTreeCollection();
            foreach (var file in source)
            {
                var tree = new Parser(new Lexer(file).Lex()).Parse();

                if (tree == null)
                {
                    Console.WriteLine("Source Code Empty");
                    continue;
                }

                treeCollection.Add(tree);
            }

            return treeCollection;
        }
    }
}
