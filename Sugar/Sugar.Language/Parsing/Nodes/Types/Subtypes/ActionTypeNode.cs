using System;

using Sugar.Language.Parsing.Nodes.Types.Enums;

namespace Sugar.Language.Parsing.Nodes.Types.Subtypes
{
    internal sealed class ActionTypeNode : VoidNode
    {
        public override TypeNodeEnum Type { get => TypeNodeEnum.Action; }

        public ActionTypeNode()
        {

        }

        public override string ToString() => $"Action Type Node";
    }
}
