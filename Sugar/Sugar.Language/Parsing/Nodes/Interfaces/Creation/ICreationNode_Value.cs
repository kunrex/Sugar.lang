using System;

namespace Sugar.Language.Parsing.Nodes.Interfaces.Creation
{
    internal interface ICreationNode_Value : ICreationNode
    {
        public ParseNodeCollection Value { get; }
    }
}
