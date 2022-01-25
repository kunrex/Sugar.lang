using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Values.Generics
{
    internal abstract class BaseGenericNode : Node
    {
        public override NodeType NodeType => NodeType.Generic;
    }
}
