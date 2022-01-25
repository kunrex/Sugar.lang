using System;

namespace Sugar.Language.Parsing.Nodes.Interfaces.Creation
{
    internal interface ICreationNode_Type : ICreationNode
    {
        public Node Type { get; }
    }
}
