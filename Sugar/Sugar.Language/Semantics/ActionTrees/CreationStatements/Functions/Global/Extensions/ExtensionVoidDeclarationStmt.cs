using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global.Extensions
{
    internal sealed class ExtensionVoidDeclarationStmt : GlobalVoidDeclarationStmt<ExtensionMethodDeclarationStmt, ExtensionVoidDeclarationStmt>, IExtension
    {
        private readonly DataType parentType;
        public DataType ParentType { get => parentType; }

        private readonly IdentifierNode identifier;

        public override ActionNodeEnum ActionNodeType { get => ActionNodeEnum.ExtensionVoid; }

        public ExtensionVoidDeclarationStmt(IdentifierNode _name, Describer _describer, FunctionArguments _arguments, Node _nodeBody, DataType _parentType) : base(
           _name.Value,
           _describer,
           _arguments,
           _nodeBody)
        {
            identifier = _name;

            parentType = _parentType;
        }


        public override string ToString() => $"Extension Function Declaration Node [Extends: {parentType.Name}]";
    }
}
