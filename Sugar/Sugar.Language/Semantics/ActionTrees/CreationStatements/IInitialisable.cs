using System;

using Sugar.Language.Parsing.Nodes;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements
{
    internal interface IInitialisable
    {
        public Node Value { get; }
    }
}
