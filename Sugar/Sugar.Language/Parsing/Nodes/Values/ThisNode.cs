﻿using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Values
{
    internal sealed class ThisNode : Node
    {
        public override NodeType NodeType => NodeType.This;

        public Node Reference { get => Children[0]; }

        public ThisNode(Node _reference)
        {
            Children = new List<Node>() { _reference };
        }

        public override string ToString() => $"This Node";
    }
}
