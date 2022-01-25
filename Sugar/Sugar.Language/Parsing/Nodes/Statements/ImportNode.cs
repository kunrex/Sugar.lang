using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.UDDataTypes.Enums;

namespace Sugar.Language.Parsing.Nodes.Statements
{
    internal sealed class ImportNode : StatementNode
    {
        public UDDataType EntityType { get; private set; }
        public override NodeType NodeType => NodeType.Import;

        public ImportNode(UDDataType _entityType, Node _name)
        {
            EntityType = _entityType;
            Children = new List<Node>() { _name };
        }

        public override string ToString() => $"Import Statement Node [Type: {EntityType}]";
    }
}
