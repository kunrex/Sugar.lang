﻿using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Functions.Declarations.OperatorOverloading
{
    internal sealed class ExplicitCastDeclarationNode : UnnamedFunctionDeclarationNode
    {
        public override NodeType NodeType { get => NodeType.ExplicitDeclaration; }

        public ExplicitCastDeclarationNode(Node _describer, Node _returnType, Node _arguments, Node _body) : base(_describer, _returnType, _arguments, _body)
        {

        }

        public override string ToString() => $"Explicit Cast Declaration Node";
    }
}

