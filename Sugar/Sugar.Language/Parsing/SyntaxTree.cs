using System;

using Sugar.Language.Parsing.Nodes;

namespace Sugar.Language.Parsing
{
    internal sealed class SyntaxTree
    {
        private readonly Node baseNode;
        public Node BaseNode
        {
            get
            {
                return baseNode;
            }
        }

        public SyntaxTree(Node _baseNode)
        {
            baseNode = _baseNode;
        }

        public void Print() => baseNode.Print("", true);

        public void ParentNodes() => baseNode.SetParent();
    }
}
