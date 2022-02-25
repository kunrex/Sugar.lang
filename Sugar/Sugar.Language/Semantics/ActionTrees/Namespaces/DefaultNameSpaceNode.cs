using System;

namespace Sugar.Language.Semantics.ActionTrees.Namespaces
{
    internal sealed class DefaultNameSpaceNode : BaseNameSpaceNode
    {
        public DefaultNameSpaceNode() : base()
        {

        }

        public override string ToString() => $"Default Name Space";

        protected override void PrintChildren(string indent)
        {
            for (int i = 0; i < dataTypes.Count; i++)
                dataTypes[i].Print(indent, i == dataTypes.Count - 1);
        }
    }
}
