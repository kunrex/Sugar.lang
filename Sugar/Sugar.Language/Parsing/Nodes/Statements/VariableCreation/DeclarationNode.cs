﻿using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Functions.Properties;

namespace Sugar.Language.Parsing.Nodes.Statements.VariableCreation
{
    internal class DeclarationNode : VariableCreationNode
    {
        public override NodeType NodeType => NodeType.Declaration;

        private readonly Node name;
        public override Node Name { get => name; }

        public DeclarationNode(Node _describer, Node _type, Node _name) : base(_describer, _type, _name)
        {
            switch (_name.NodeType)
            {
                case NodeType.PropertyGet:
                case NodeType.PropertySet:
                case NodeType.PropertyGetSet:
                    name = ((PropertyNode)_name).Name;
                    break;
                default:
                    name = _name;
                    break;
            }
        }

        public override string ToString() => $"Declaration Node";
    }
}
