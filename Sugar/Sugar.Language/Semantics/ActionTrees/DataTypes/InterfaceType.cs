using System;
using System.Collections.Generic;

using Sugar.Language.Tokens.Operators;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.UDDataTypes;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Interfaces;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes.Casts;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global.Conversion;

namespace Sugar.Language.Semantics.ActionTrees.DataTypes
{
    internal sealed class InterfaceType : DataTypeWrapper<InterfaceNode>, IPropertyContainer, IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>, IOperatorContainer, IIndexerContainer, IImplicitContainer, IExplicitContainer
    {
        public override ActionNodeEnum ActionNodeType { get => ActionNodeEnum.Interface; }

        public InterfaceType(IdentifierNode _name, List<ImportNode> _imports, InterfaceNode _skeleton) : base(_name,
                    _imports,
                    MemberEnum.Indexer |
                    MemberEnum.Indexer |
                    MemberEnum.Variable |
                    MemberEnum.Property |
                    MemberEnum.Function |
                    MemberEnum.Constructor |
                    MemberEnum.ImplicitCast |
                    MemberEnum.ExplicitCast |
                    MemberEnum.OperaterOverload,
                    _skeleton)
        {

        }

        public IPropertyContainer AddDeclaration(PropertyCreationStmt declaration) => AddGlobalMember<InterfaceType, IPropertyContainer>(MemberEnum.Property, declaration, this);

        public IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt> AddDeclaration(MethodDeclarationStmt declaration) => AddGlobalMember<InterfaceType, IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>>(MemberEnum.Function, declaration, this);

        public IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt> AddDeclaration(VoidDeclarationStmt declaration) => AddGlobalMember<InterfaceType, IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>>(MemberEnum.Void, declaration, this);

        public IOperatorContainer AddDeclaration(OperatorOverloadDeclarationStmt declaration) => AddGlobalMember<InterfaceType, IOperatorContainer>(MemberEnum.OperaterOverload, declaration, this);

        public IIndexerContainer AddDeclaration(IndexerCreationStmt declaration) => AddGlobalMember<InterfaceType, IIndexerContainer>(MemberEnum.Indexer, declaration, this);

        public ICastContainer<ImplicitCastDeclarationStmt, IImplicitContainer> AddDeclaration(ImplicitCastDeclarationStmt declaration) => AddGlobalMember<InterfaceType, IImplicitContainer>(MemberEnum.ImplicitCast, declaration, this);

        public ICastContainer<ExplicitCastDeclarationStmt, IExplicitContainer> AddDeclaration(ExplicitCastDeclarationStmt declaration) => AddGlobalMember<InterfaceType, IExplicitContainer>(MemberEnum.ExplicitCast, declaration, this);

        public PropertyCreationStmt TryFindPropertyCreation(IdentifierNode identifier) => TryFindIdentifiedMember<PropertyCreationStmt, IPropertyContainer>(identifier, MemberEnum.Properties);

        public VoidDeclarationStmt TryFindVoidDeclaration(IdentifierNode identifier, IFunctionArguments arguments) => TryFindIdentifiedArgumentedMember<VoidDeclarationStmt, IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>>(identifier, arguments, MemberEnum.Void);

        public MethodDeclarationStmt TryFindMethodDeclaration(IdentifierNode identifier, IFunctionArguments arguments) => TryFindIdentifiedArgumentedMember<MethodDeclarationStmt, IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>>(identifier, arguments, MemberEnum.Function);

        public OperatorOverloadDeclarationStmt TryFindOperatorOverloadDeclaration(Operator op, DataType operhand) => TryFindOperatorOverload(op, operhand);

        public OperatorOverloadDeclarationStmt TryFindOperatorOverloadDeclaration(Operator op, DataType operhand1, DataType operhand2) => TryFindOperatorOverload(op, operhand1, operhand2);

        public ImplicitCastDeclarationStmt TryFindImplicitCastDeclaration(DataType external) => TryFindCast<ImplicitCastDeclarationStmt, IImplicitContainer>(external, MemberEnum.ImplicitCast);

        public ExplicitCastDeclarationStmt TryFindExplicitCastDeclaration(DataType external) => TryFindCast<ExplicitCastDeclarationStmt, IExplicitContainer>(external, MemberEnum.ExplicitCast);

        public IndexerCreationStmt TryFindIndexerCreationStatement(DataType external, IFunctionArguments arguments) => TryFindIndexer(external, arguments);

        public override string ToString() => $"Interface Node [{name.Value}]";
    }
}
