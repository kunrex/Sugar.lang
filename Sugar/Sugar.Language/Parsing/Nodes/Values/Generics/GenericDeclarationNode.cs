using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Values.Generics
{
    internal class GenericDeclarationNode : NodeCollection<GenericVariableNode>
    {
        public override ParseNodeType NodeType { get => ParseNodeType.GenericDeclarataion; }

        public GenericDeclarationNode() : base()
        {
            
        }

        public GenericDeclarationNode AddChild(GenericVariableNode node)
        {
            Add(node);

            return this;
        }

        public override string ToString() => $"Generic Declaration Node";
    }
}
