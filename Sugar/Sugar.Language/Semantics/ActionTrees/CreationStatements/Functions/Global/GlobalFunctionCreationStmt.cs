using System;

using Sugar.Language.Parsing.Nodes.Functions.Declarations;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global
{
    internal abstract class GlobalFunctionCreationStmt<Base, Parent> : FunctionCreationStmt<Base, Parent> where Base : BaseFunctionDeclarationNode where Parent : IActionTreeNode
    {
        public GlobalFunctionCreationStmt(DataType _creationType, Describer _describer, FunctionArguments _arguments, Base _baseNode) : base(
            _creationType,
            _describer,
            DescriberEnum.Static | DescriberEnum.AccessModifiers | DescriberEnum.InheritanceModifiers | DescriberEnum.Override,
            _arguments,
            _baseNode)
        {

        }
    }
}
