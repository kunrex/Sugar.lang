using System;

namespace Sugar.Language.Parsing.Nodes.Interfaces.Creation
{
    internal interface ICreationNode_Name : ICreationNode
    {
       public Node Name { get; }
    }
}
