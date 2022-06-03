using System;

using Sugar.Language.Parsing.Nodes.Functions.Declarations;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global.Conversion
{
    internal abstract class CastDeclarationStmt<SubType, Parent> : GlobalFunctionCreationStmt<SubType, Parent> where SubType : UnnamedFunctionDeclarationNode where Parent : IActionTreeNode
    {
        protected DataType to;
        protected DataType from;

        public CastDeclarationStmt(DataType _creationType, Describer _describer, FunctionArguments _arguments, SubType _baseNode) : base(
            _creationType,
            _describer,
            _arguments,
            _baseNode)
        {
            
        }
    }
}
