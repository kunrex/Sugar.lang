using System;

using Sugar.Language.Parsing.Nodes;

namespace Sugar.Language.Exceptions.Analysis.Project
{
    internal sealed class InvalidProjectStatementException : CompileException
    {
        public InvalidProjectStatementException(ParseNode node) : base("Files can directly contain only type and namespace definitions", 0)
        {

        }
    }
}
