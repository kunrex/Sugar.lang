using System;

using Sugar.Language.Parsing.Nodes;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.Value
{
    internal interface IValueNode
    {
        public ParseNode Value { get; }
    }
}

