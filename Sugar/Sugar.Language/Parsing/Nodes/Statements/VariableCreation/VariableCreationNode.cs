using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Interfaces.Creation;

namespace Sugar.Language.Parsing.Nodes.Statements.VariableCreation
{
    internal abstract class VariableCreationNode : StatementNode, ICreationNode_Type, ICreationNode_Name
    {
        public Node Describer { get => Children[0]; }

        public Node Type { get => Children[1]; }
        public virtual Node Name { get => Children[2]; }

        public virtual NodeType CreationType { get => Name.NodeType; }

        public VariableCreationNode(Node _describer, Node _type, Node _name)
        {
            Children = new List<Node>() { _describer, _type, _name };
        }
    }
}
