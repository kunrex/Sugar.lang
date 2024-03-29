﻿using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Interfaces.Creation;

namespace Sugar.Language.Parsing.Nodes.Statements.VariableCreation
{
    internal class InitializeNode : DeclarationNode, ICreationNode_Value
    {
        public Node Value { get => Children[3]; }

        public InitializeNode(Node _describer, Node _type, Node _name, Node _to) : base(_describer, _type, _name)
        {
            Children = new List<Node>() { _describer, _type, _name, _to };

            nodeType = NodeType.PropertyDeclaration;
        }

        public override string ToString() => $"Initialize Node";
    }
}
