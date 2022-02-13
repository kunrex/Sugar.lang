using System;
using System.Collections.Generic;
using System.Linq;

using Sugar.Language.Lexing;
using Sugar.Language.Parsing;
using Sugar.Language.Tokens;
using Sugar.Language.Exceptions;
using Sugar.Language.Parsing.Parser;

using Sugar.Language.Semantics.Analysis;

namespace Sugar.Language
{
    public sealed class Compiler
    {
        private readonly string sourceCode;
        public string SourceCode { get => sourceCode; }

        private List<Token> Tokens { get; set; }
        private SyntaxTree SyntaxTree { get; set; }

        public Compiler(string _sourceCode)
        {
            sourceCode = _sourceCode;

            Console.WriteLine("_____Source_____");
            Console.WriteLine(sourceCode);
        }

        public bool Lex()
        {
            Lexer lexer = new Lexer(sourceCode);
            Tokens = lexer.Lex();

            Console.WriteLine("\n_____Lexed_____");
            Console.WriteLine(string.Join('\n', Tokens));

            return true;
        }

        public bool Parse()
        {
            SyntaxTree = new Parser(Tokens).Parse();

            Console.WriteLine("\n_____Parsed_____");

            if (SyntaxTree == null)
            {
                Console.WriteLine("Source Code Empty");
                return false;
            }
            else
                SyntaxTree.BaseNode.Print("", true);

            SyntaxTree.ParentNodes();
            return true;
        }

        public void Compile()
        {
            try
            {
                Lex();

                var parseResult = Parse();
                if (!parseResult)
                    return;

                var analyser = new SemanticAnalyser(SyntaxTree);

                var analysisResult = analyser.Analyse();
                Console.WriteLine(analysisResult);
            }
            catch(CompileException e)
            {
                Console.WriteLine($"Exception: {e.Message}");
            }
        }
    }
}
