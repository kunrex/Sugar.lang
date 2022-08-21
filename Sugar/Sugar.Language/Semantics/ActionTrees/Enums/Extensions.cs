using System;
using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Semantics.ActionTrees.Enums
{
    internal static class Extensions
    {
        public static GlobalMemberEnum ToGlobalMemberEnum(this NodeType nodeType) => nodeType switch
        {
            NodeType.Indexer => GlobalMemberEnum.Indexer
        };

    }
}
