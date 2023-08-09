using System;

namespace Sugar.Language.Compilers
{
    internal sealed class SimpleCompiler : CompilerBase
    {
        private readonly string source;
        public string Source
        {
            get
            {
                return source;
            }
        }

        public SimpleCompiler(string _source)
        {
            source = _source;
        }

        private SugarFile CreateFile()
        {
            return ReadFile(source);
        }

        public bool LexParse() => CreateFile().Exceptions.Count == 0;

        public bool Analyse()
        {
            var file = CreateFile();

            //analyse file
            return file.Exceptions.Count == 0;
        }
    }
}
