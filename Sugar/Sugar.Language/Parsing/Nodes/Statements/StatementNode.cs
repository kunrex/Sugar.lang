using System;

namespace Sugar.Language.Parsing.Nodes.Statements
{
    internal abstract class StatementNode : Node
    {
        public override Node AddChild(Node _node) => throw new NotImplementedException();
    }
}
