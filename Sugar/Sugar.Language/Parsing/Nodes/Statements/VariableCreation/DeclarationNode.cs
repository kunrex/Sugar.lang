using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Statements.VariableCreation
{
    internal class DeclarationNode : VariableCreationNode
    {
        public override NodeType NodeType => NodeType.Declaration;

        public DeclarationNode(Node _describer, Node _type, Node _name) : base(_describer, _type, _name)
        {

        }

        public override string ToString() => $"Declaration Node";
    }
}
