﻿using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Local
{
    internal sealed class LocalVoidDeclarationStmt : VoidCreationStmt<LocalMethodCreationStmt, LocalVoidDeclarationStmt>
    {
        public override CreationTypeEnum CreationEnumType { get => CreationTypeEnum.LocalFunction; }

        public LocalVoidDeclarationStmt(IdentifierNode _name, Describer _describer, FunctionArguments arguments, Node _nodeBody) : base(
            _name.Value,
            _describer,
            DescriberEnum.None,
            arguments,
            _nodeBody)
        {

        }

        public override string ToString() => "Local Void Declaration Node";
    }
}
