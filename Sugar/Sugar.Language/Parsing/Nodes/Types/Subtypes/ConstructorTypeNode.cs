using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Types.Enums;

namespace Sugar.Language.Parsing.Nodes.Types.Subtypes
{
    internal sealed class ConstructorTypeNode : TypeNode
    {
        public Node ConstructorReturnType { get => Children[0]; }
        public override TypeNodeEnum Type => TypeNodeEnum.Constructor;

        public ConstructorTypeNode(Node _returnType)
        {
            Children = new List<Node>() { _returnType };
        }

        public override string ToString() => $"Constructor Type Node";
    }
}
