using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Statements.VariableCreation
{
    internal sealed class CompoundVariableDeclarationNode : StatementNode
    {
        public override NodeType NodeType => NodeType.CompoundDeclaration; 

        public CompoundVariableDeclarationNode(List<Node> _declarations)
        {
            Children = _declarations;
        }

        public override string ToString() => $"Compound Variable Declaration Node";
    }
}
