using System;

using Sugar.Language.Parsing.Nodes;

namespace Sugar.Language.Parsing
{
    internal sealed class SyntaxTree
    {
        public Node BaseNode { get; private set; }

        public SyntaxTree()
        {

        }

        public SyntaxTree(Node _baseNode)
        {
            BaseNode = _baseNode;
        }

        public SyntaxTree AddChildToBaseNode(Node _node)
        {
            BaseNode.AddChild(_node);

            return this;
        }
    }
}
