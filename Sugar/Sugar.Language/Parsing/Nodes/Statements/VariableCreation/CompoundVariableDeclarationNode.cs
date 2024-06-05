using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Statements.VariableCreation
{
    internal sealed class CompoundVariableDeclarationNode : NodeCollection<DeclarationNode>
    {
        public override ParseNodeType NodeType { get => ParseNodeType.CompoundDeclaration; }

        public CompoundVariableDeclarationNode(List<DeclarationNode> _declarations) : base(_declarations)
        {
           
        }

        public override string ToString() => $"Compound Variable Declaration Node";
    }
}
