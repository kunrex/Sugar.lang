using System;
using System.Collections.Generic;

namespace Sugar.Language.Semantics.ActionTrees.NodeGroups
{
    internal sealed class ScopeNodeAT : ActionTreeNode
    {
        public ScopeNodeAT()
        {

        }

        public IEnumerable<ActionTreeNode> GetChildrenBefore(int index)
        {
            for (int i = 0; i < index; i++)
                yield return Children[i];
        }

        public IEnumerable<ActionTreeNode> GetChildrenBefore(ActionTreeNode node)
        {
            var index = Children.IndexOf(node);

            if (index == -1)
                yield return null;

            foreach (var child in GetChildrenBefore(index))
                yield return child;
        }

        public override string ToString() => $"Scope Node [Action Tree]";
    }
}
