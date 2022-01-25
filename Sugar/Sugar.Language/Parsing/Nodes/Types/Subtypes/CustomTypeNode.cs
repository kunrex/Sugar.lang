using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Types.Enums;

namespace Sugar.Language.Parsing.Nodes.Types.Subtypes
{
    internal sealed class CustomTypeNode : TypeNode
    {
        public Node CustomType { get => Children[0]; }
        public override TypeNodeEnum Type => TypeNodeEnum.BuiltIn;

        public CustomTypeNode(Node _type)
        {
            Children = new List<Node>() { _type };
        }

        public override string ToString() => $"Custom Type Node";
    }
}
