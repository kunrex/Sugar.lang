using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Types.Enums;

namespace Sugar.Language.Parsing.Nodes.Types.Subtypes
{
    internal sealed class ArrayTypeNode : TypeNode
    {
        public Node ArrayType { get => Children[0]; }
        public override TypeNodeEnum Type => TypeNodeEnum.Array;

        public ArrayTypeNode(Node _type)
        {
            Children = new List<Node>() { _type };
        }

        public override string ToString() => $"Array Type Node";
    }
}
