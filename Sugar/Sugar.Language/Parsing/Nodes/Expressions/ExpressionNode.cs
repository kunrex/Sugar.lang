using System;

namespace Sugar.Language.Parsing.Nodes.Expressions
{
    internal abstract class ExpressionNode : Node
    {
        public override Node AddChild(Node _node) => throw new NotImplementedException();
    }
}
