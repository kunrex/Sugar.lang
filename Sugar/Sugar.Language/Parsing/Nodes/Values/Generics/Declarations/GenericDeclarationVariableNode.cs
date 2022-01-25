using System;
using System.Collections.Generic;

namespace Sugar.Language.Parsing.Nodes.Values.Generics.Declarations
{
    internal sealed class GenericDeclarationVariableNode : BaseGenericNode
    {
        public Node Variable { get => Children[0]; }
        public Node Enforcement { get => ChildCount == 1 ? null : Children[1]; }

        public GenericDeclarationVariableNode(Node _variable)
        {
            Children = new List<Node>() { _variable };
        }

        public GenericDeclarationVariableNode(Node _variable, Node _enforcement)
        {
            Children = new List<Node>() { _variable, _enforcement };
        }

        public override string ToString() => $"Generic Declaration Type Node";
    }
}
