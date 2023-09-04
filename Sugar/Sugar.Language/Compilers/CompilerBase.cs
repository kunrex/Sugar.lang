using System;

using Sugar.Language.Lexing;

using Sugar.Language.Parsing.Parser;

namespace Sugar.Language.Compilers
{
    internal abstract class CompilerBase
    {
        public CompilerBase()
        {

        }

        protected internal SugarFile ReadFile(string source)
        {
            var file = new SugarFile(source);

            Lexer.Instance.Lex(file);
            Parser.Instance.Parse(file);

            return file;
        }
    }
}
