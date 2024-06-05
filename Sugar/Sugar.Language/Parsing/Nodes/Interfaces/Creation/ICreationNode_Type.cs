using System;

using Sugar.Language.Parsing.Nodes.Types;

namespace Sugar.Language.Parsing.Nodes.Interfaces.Creation
{
    internal interface ICreationNode_Type : ICreationNode
    {
        public TypeNode Type { get; }
    }
}
