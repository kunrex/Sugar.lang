using System;

namespace Sugar.Language.Parsing.Nodes.Loops
{
    internal abstract class LoopNode : Node
    {
        public abstract Node Body { get; }
        public abstract Node Condition { get; }

        public override Node AddChild(Node _node) => throw new NotImplementedException();
    }
}
