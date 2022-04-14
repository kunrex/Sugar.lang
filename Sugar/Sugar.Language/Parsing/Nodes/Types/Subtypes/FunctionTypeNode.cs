using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Types.Enums;

namespace Sugar.Language.Parsing.Nodes.Types.Subtypes
{
    internal sealed class FunctionTypeNode : TypeNode
    {
        public override TypeNodeEnum Type => TypeNodeEnum.Function;

        public Node TypeNode { get => Children[0]; }
             
        public FunctionTypeNode(Node _type)
        {
            Children = new List<Node>() { _type };
        }

        public override string ToString() => $"Function Type Node";
    }
}
