using System;
using System.Collections.Generic;

namespace Sugar.Language.Parsing.Nodes.TryCatchFinally.Blocks
{
    internal abstract class BlockNode : Node
    {
        public virtual Node Body { get => Children[0]; }

        public BlockNode(Node _body)
        {
            Children = new List<Node>() { _body };
        }
    }
}
