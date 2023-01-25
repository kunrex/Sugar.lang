using System;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global.Conversion
{
    internal abstract class CastDeclarationStmt<Parent> : GlobalFunctionCreationStmt<Parent> where Parent : IActionTreeNode
    {
        public DataType To { get => CreationType; }
        public DataType From { get => FunctionArguments[0].CreationType; }

        public CastDeclarationStmt(DataType _creationType, Describer _describer, FunctionArguments _arguments, Node _nodeBody) : base(
            _creationType,
            _creationType.Name,
            _describer,
            _arguments,
            _nodeBody)
        {
           
        }
    }
}
