using System;
using System.Text;
using System.Collections.Generic;

using Sugar.Language.Tokens;
using Sugar.Language.Parsing;
using Sugar.Language.Exceptions;

namespace Sugar.Language
{
    internal sealed class SugarFile  
    {
        private readonly StringBuilder builder;
        public string Source { get { return builder.ToString(); } }

        private readonly List<CompileException> compileExceptions;
        public int ExceptionCount { get => compileExceptions.Count; }

        private SyntaxTree syntaxTree;
        public SyntaxTree SyntaxTree
        {
            get
            {
                return syntaxTree;
            }
        }

        private TokenCollection tokenCollection;

        public bool isInitialised
        {
            get
            {
                return tokenCollection != null && syntaxTree != null;
            }
        }

        public int TokenCount
        {
            get
            {
                if (tokenCollection == null)
                    return 0;

                return tokenCollection.Length;
            }
        }

        public Token this[int index]
        {
            get => tokenCollection[index];
        }

        public SugarFile(string _source)
        {
            builder = new StringBuilder(_source);

            compileExceptions = new List<CompileException>();
        }

        public SugarFile WithTokens(TokenCollection tokens)
        {
            if (tokenCollection == null)
                tokenCollection = tokens;

            return this;
        }

        public SugarFile WithSyntaxTree(SyntaxTree tree)
        {
            if (syntaxTree == null)
                syntaxTree = tree;

            return this;

        }

        public SugarFile PushException(CompileException exception)
        {
            compileExceptions.Add(exception);

            return this;
        }

        public void PrintExceptions()
        {
            foreach (var exception in compileExceptions)
                Console.WriteLine(exception);
        }

        public void PrintTree() => syntaxTree.BaseNode.ToString();
    }
}
