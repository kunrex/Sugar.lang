using System;

using Sugar.Language.Parsing.Nodes.Expressions;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements
{
    internal interface IInitialisable
    {
        public ExpressionNode Value { get; }
    }
}
