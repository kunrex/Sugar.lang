using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.TryCatchFinally.Blocks
{
    internal sealed class CatchBlockArgumentsNode : Node
    {
        public override NodeType NodeType => NodeType.Catch;

        public CatchBlockArgumentsNode()
        {
            
        }

        public CatchBlockArgumentsNode(List<Node> _arguments)
        {
            Children = _arguments;
        }

        public override string ToString() => $"Catch Block Arguments Node";
    }
}

