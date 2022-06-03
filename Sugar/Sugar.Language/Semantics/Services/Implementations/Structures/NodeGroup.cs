using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Statements;

namespace Sugar.Language.Semantics.Services.Implementations.Structures
{
    internal struct NodeGroup
    {
        public Node Value { get; private set; }
        public ImportNode ImportNode { get; private set; }

        public NodeGroup(Node _value, ImportNode _importNode)
        {
            Value = _value;
            ImportNode = _importNode;
        }
    }
}
