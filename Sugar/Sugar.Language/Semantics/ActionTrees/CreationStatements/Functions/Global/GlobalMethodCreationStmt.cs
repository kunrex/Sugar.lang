using System;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global
{
    internal abstract class GlobalMethodCreationStmt<Parent> : MethodCreationStmt<Parent> where Parent : IActionTreeNode
    {
        public GlobalMethodCreationStmt(DataType _creationType, string _name, Describer _describer, FunctionDeclArgs _arguments, Node _nodeBody) : base(
            _creationType,
            _name,
            _describer,
            DescriberEnum.Static | DescriberEnum.AccessModifiers | DescriberEnum.InheritanceModifiers | DescriberEnum.Override,
            _arguments,
            _nodeBody)
        {

        }
    }
}
