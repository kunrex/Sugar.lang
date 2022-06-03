using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Functions.Declarations;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions
{
    internal abstract class FunctionCreationStmt<Base, Parent> : CreationStatement<Parent> where Base : BaseFunctionDeclarationNode where Parent : IActionTreeNode
    {
        protected readonly Node body;
        protected readonly FunctionArguments arguments;

        public FunctionCreationStmt(DataType _creationType, Describer _describer, DescriberEnum _allowed, FunctionArguments _arguments, Base _baseNode) : base(
            _creationType,
            (IdentifierNode)_baseNode.Name,
            _describer,
            _allowed)
        {
            body = _baseNode.Body;
            arguments = _arguments;
        }
    }
}
