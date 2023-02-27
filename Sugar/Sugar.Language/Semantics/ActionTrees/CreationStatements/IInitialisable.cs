using System;

using Sugar.Language.Parsing.Nodes;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements
{
    internal interface IInitialisableCreationStatement : ICreationStatement
    {
        public Node Value { get; }
    }
}
