using System;

using Sugar.Language.Parsing.Nodes.Values;

namespace Sugar.Language.Parsing.Nodes.Interfaces.Creation
{
    internal interface ICreationNode_Name : ICreationNode
    {
       public IdentifierNode Name { get; }
    }
}
