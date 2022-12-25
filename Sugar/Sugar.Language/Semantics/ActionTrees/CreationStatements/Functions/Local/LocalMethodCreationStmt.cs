using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Local
{
    internal sealed class LocalMethodCreationStmt : FunctionCreationStmt<IFunctionContainer<LocalMethodCreationStmt, LocalVoidDeclarationStmt>>, ILocalFunction, IMethodCreation
    {
        public override ActionNodeEnum ActionNodeType { get => ActionNodeEnum.LocalFunction; }

        public LocalMethodCreationStmt(DataType _creationType, IdentifierNode _name, Describer _describer, FunctionArguments arguments, Node _nodeBody) : base(
            _creationType,
            _name.Value,
            _describer,
            DescriberEnum.None,
            arguments,
            _nodeBody)
        {

        }
      
        public override string ToString() => $"Local Function Declaration Node";
    }
}
