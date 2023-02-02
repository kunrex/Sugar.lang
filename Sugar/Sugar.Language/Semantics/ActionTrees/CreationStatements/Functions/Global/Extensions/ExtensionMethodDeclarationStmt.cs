using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global.Extensions
{
    internal sealed class ExtensionMethodDeclarationStmt : GlobalMethodCreationStmt<IExtensionFunctionContainer>, IMethodCreation, IExtension
    {
        private readonly DataType parentType;
        public DataType ParentType { get => parentType; }

        private readonly IdentifierNode identifier;

        public override ActionNodeEnum ActionNodeType { get => ActionNodeEnum.ExtensionFunction; }

        public ExtensionMethodDeclarationStmt(DataType _creationType, IdentifierNode _name, Describer _describer, FunctionDeclArgs _arguments, Node _nodeBody, DataType _parentType) : base(
           _creationType,
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
