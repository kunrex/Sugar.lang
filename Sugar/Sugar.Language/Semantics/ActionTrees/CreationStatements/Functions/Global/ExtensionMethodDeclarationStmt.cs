using System;

using Sugar.Language.Parsing.Nodes.Functions.Declarations;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global
{
    internal sealed class ExtensionMethodDeclarationStmt : MethodDeclarationStmt
    {
        private readonly DataType parentType;

        public ExtensionMethodDeclarationStmt(DataType _creationType, Describer _describer, FunctionArguments _arguments, FunctionDeclarationNode _baseNode, DataType _parentType) : base(
           _creationType,
           _describer,
           _arguments,
           _baseNode)
        {
            parentType = _parentType;
        }

        public override string ToString() => $"Extension Function Declaration Node [Extends: {parentType.Name}]";
    }
}
