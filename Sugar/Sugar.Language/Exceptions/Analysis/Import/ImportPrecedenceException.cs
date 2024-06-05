using System;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.Expressions.Associative;

namespace Sugar.Language.Exceptions.Analysis.Import
{
    internal sealed class ImportPrecedenceException : CompileException
    {
        public ImportPrecedenceException(ImportNode import) : base("Import statements must preceed namespace and type defintions", GetIndex(import))
        {

        }

        private static int GetIndex(ImportNode importNode) => importNode.BaseName.NodeType switch
        {
            ParseNodeType.Dot => ((IdentifierNode)((DotExpression)importNode.BaseName).LHS).Token.Index,
            ParseNodeType.Variable => ((IdentifierNode)importNode.BaseName).Token.Index,
            _ => -1
        };
    }
}
