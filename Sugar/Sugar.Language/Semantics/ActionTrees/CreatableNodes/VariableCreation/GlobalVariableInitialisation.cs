using System;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Expressions;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;

namespace Sugar.Language.Semantics.ActionTrees.CreatableNodes.VariableCreation
{
    internal sealed class GlobalVariableInitialisation : GlobalVariableDeclaration
    {
        private readonly ExpressionNode value;

        public GlobalVariableInitialisation(DataType _creationType, IdentifierNode _creationName, Describer _describer, ExpressionNode _value) : base(
            _creationType,
            _creationName,
            _describer)
        {
            value = _value;
        }
    }
}
