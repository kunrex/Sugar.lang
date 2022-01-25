using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Values
{
    internal sealed class DefaultValueNode : Node
    {
        public override NodeType NodeType => NodeType.Default;

        public DefaultValueNode()
        {

        }

        public override string ToString() => $"Default Value Node";
    }
}
