using System;

using Sugar.Language.Compilers;

using Sugar.Language.Lexing;
using Sugar.Language.Parsing.Parser;

namespace Sugar.Language
{
    public sealed class UnitTestCompiler : ICompiler
    {
        private readonly string source;

        public UnitTestCompiler(string _source)
        {
            source = _source;

            if (Lexer.Instance == null)
                Lexer.CreateInstance();

            if (Parser.Instance == null)
                Parser.CreateInstance();
        }

        public bool Initialise()
        {
            if(Lexer.Instance == null)
                Lexer.CreateInstance();

            if (Parser.Instance == null)
                Parser.CreateInstance();

            return true;
        }

        public bool Compile()
        {
            var compiler = new SimpleCompiler(source);

            return compiler.LexParse();
        }
    }
}
