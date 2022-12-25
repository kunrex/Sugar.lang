using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Interfaces;

namespace Sugar.Language.Semantics.Services.Structure
{
    internal struct SearchResult<ReturnType> where ReturnType : IActionTreeNode
    {
        private readonly ReturnType result;
        public ReturnType Result { get => result; }

        public ActionNodeEnum ResultEnum { get => result.ActionNodeType; }

        private readonly Node remaining;
        public Node Remaining { get => remaining; }

        public SearchResult(ReturnType _result)
        {
            result = _result;
            remaining = null;
        }

        public SearchResult(ReturnType _result, Node _remaining)
        {
            result = _result;
            remaining = _remaining;
        }
    }
}
