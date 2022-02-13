using System;

using Sugar.Language.Parsing.Nodes;

namespace Sugar.Language.Semantics.ActionTrees
{
    internal sealed class ActionTree
    {
        public ActionTreeNode BaseNode { get; private set; }

        public ActionTree()
        {

        }

        public ActionTree(ActionTreeNode _baseNode)
        {
            BaseNode = _baseNode;
        }

        public void ParentNodes() => BaseNode.SetParent();
    }
}
