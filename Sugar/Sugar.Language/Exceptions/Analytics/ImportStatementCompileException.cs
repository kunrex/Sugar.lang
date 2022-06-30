using System;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.Expressions.Associative;

namespace Sugar.Language.Exceptions.Analytics
{
    internal abstract class ImportStatementCompileException : CompileException
    {
        public ImportStatementCompileException(string exception, int index) : base(exception, index)
        {

        }

        protected static int GetIndex(ImportNode importNode) => importNode.Name.NodeType switch
        {
            NodeType.Dot => ((IdentifierNode)((DotExpression)importNode.Name).LHS).Token.Index,
            _ => ((IdentifierNode)importNode.Name).Token.Index
        };
    }
}
