using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Types;

namespace Sugar.Language.Parsing.Nodes.Values.Generics
{
    internal class GenericCallNode : NodeCollection<TypeNode>
    {
        public override ParseNodeType NodeType { get => ParseNodeType.GenericCall; } 

        public GenericCallNode() : base()
        {
            
        }

        public GenericCallNode AddChild(TypeNode node)
        {
            Add(node);

            return this;
        }

        public override string ToString() => "Generic Call Node";
    }
}
