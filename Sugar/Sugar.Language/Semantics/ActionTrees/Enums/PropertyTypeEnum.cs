using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Semantics.ActionTrees.Enums
{
    public enum PropertyTypeEnum : byte
    {
        Get = NodeType.PropertyGet,
        Set = NodeType.PropertySet,
        GetSet = NodeType.PropertyGetSet
    }
}
