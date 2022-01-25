using System;

using Sugar.Language.Parsing.Nodes.Types.Enums;

namespace Sugar.Language.Parsing.Nodes.Types.Subtypes
{
    internal sealed class VoidNode : TypeNode
    {
        public override TypeNodeEnum Type => TypeNodeEnum.Void;

        public VoidNode()
        {
            
        }

        public override string ToString() => $"Void Node";
    }
}
