using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global
{
    internal sealed class ExtensionMethodDeclarationStmt : MethodDeclarationStmt
    {
        private readonly DataType parentType;

        public ExtensionMethodDeclarationStmt(DataType _creationType, IdentifierNode _name, Describer _describer, FunctionArguments _arguments, Node _nodeBody, DataType _parentType) : base(
           _creationType,
           _name,
           _describer,
           _arguments,
           _nodeBody)
        {
            parentType = _parentType;
        }

        public override string ToString() => $"Extension Function Declaration Node [Extends: {parentType.Name}]";
    }
}
