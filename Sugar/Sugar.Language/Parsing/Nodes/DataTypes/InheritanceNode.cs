using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Types;

namespace Sugar.Language.Parsing.Nodes.DataTypes
{
    internal sealed class InheritanceNode : NodeCollection<TypeNode>
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Inheritance; }

        public InheritanceNode() : base()
        {
           
        }

        public InheritanceNode AddChild(TypeNode _node)
        {
            Add(_node);

            return this;
        }

        public override string ToString() => $"Inheritance Node";
    }
}
