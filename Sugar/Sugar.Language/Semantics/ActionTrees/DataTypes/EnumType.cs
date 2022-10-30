using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.UDDataTypes;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation;

namespace Sugar.Language.Semantics.ActionTrees.DataTypes
{
    internal sealed class EnumType : DataTypeWrapper<EnumNode>, IVariableContainer
    {
        public override DataTypeEnum TypeEnum { get => DataTypeEnum.Enum; }

        public EnumType(IdentifierNode _name, List<ImportNode> _imports, EnumNode _skeleton) : base(_name, _imports, MemberEnum.Variable, _skeleton)
        {

        }

        public IVariableContainer AddDeclaration(VariableCreationStmt declaration) => AddGlobalMember<EnumType>(MemberEnum.Variable, declaration);

        public VariableCreationStmt TryFindVariableCreation(IdentifierNode identifier) => globalMemberCollection.GetCreationStatement<VariableCreationStmt, IVariableContainer>(MemberEnum.Variable, identifier.Value);

        public override bool IsDuplicate(IdentifierNode identifier) => globalMemberCollection.IsDuplicateCreationStatement(identifier.Value);

        public override string ToString() => $"Enum Node [{name.Value}]";
    }
}
