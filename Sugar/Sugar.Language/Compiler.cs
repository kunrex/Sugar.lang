using System;
using System.Collections.Generic;
using System.Linq;

using Sugar.Language.Lexing;
using Sugar.Language.Parsing;
using Sugar.Language.Tokens;
using Sugar.Language.Parsing.Parser;
using Sugar.Language.Exceptions;

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
        }

        public bool Compile()
        {
            try
            {
                Console.WriteLine("_____Source_____");
                Console.WriteLine(sourceCode);

                Lexer lexer = new Lexer(sourceCode);
                Tokens = lexer.Lex();

                /*Console.WriteLine("\n_____Lexed_____");
                Console.WriteLine(string.Join('\n', Tokens.Select(x => $"[{x.Type}, {x.Value}]")));*/

                SyntaxTree = new Parser(Tokens).Parse();

                Console.WriteLine("\n_____Parsed_____");

                if (SyntaxTree == null)
                    Console.WriteLine("Source Code Empty");
                else
                    SyntaxTree.BaseNode.Print("", true);

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
