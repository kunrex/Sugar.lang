using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.UDDataTypes;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes.Casts;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global.Conversion;

namespace Sugar.Language.Semantics.ActionTrees.DataTypes
{
    internal sealed class InterfaceType : DataTypeWrapper<InterfaceNode>, IPropertyContainer, IFunctionContainer<MethodDeclarationStmt>, IOperatorContainer, IIndexerContainer, IImplicitContainer, IExplicitContainer
    {
        public override DataTypeEnum TypeEnum { get => DataTypeEnum.Interface; }

        public InterfaceType(IdentifierNode _name, List<ImportNode> _imports, InterfaceNode _skeleton) : base(_name,
                    _imports,
                    GlobalMemberEnum.Indexer |
                    GlobalMemberEnum.Variable |
                    GlobalMemberEnum.Property |
                    GlobalMemberEnum.Function |
                    GlobalMemberEnum.Constructor |
                    GlobalMemberEnum.ImplicitCast |
                    GlobalMemberEnum.Explicitcast |
                    GlobalMemberEnum.OperaterOverload,
                    _skeleton)
        {

        }

        public IPropertyContainer AddDeclaration(PropertyCreationStmt declaration) => AddGlobalMember<InterfaceType>(GlobalMemberEnum.Property, declaration);

        public IFunctionContainer<MethodDeclarationStmt> AddDeclaration(MethodDeclarationStmt declaration) => AddGlobalMember<InterfaceType>(GlobalMemberEnum.Function, declaration);

        public IOperatorContainer AddDeclaration(OperatorOverloadDeclarationStmt declaration) => AddGlobalMember<InterfaceType>(GlobalMemberEnum.OperaterOverload, declaration);

        public IIndexerContainer AddDeclaration(IndexerCreationStmt declaration) => AddGlobalMember<InterfaceType>(GlobalMemberEnum.Indexer, declaration);

        public ICastContainer<ImplicitCastDeclarationStmt, IImplicitContainer> AddDeclaration(ImplicitCastDeclarationStmt declaration) => AddGlobalMember<InterfaceType>(GlobalMemberEnum.ImplicitCast, declaration);

        public ICastContainer<ExplicitCastDeclarationStmt, IExplicitContainer> AddDeclaration(ExplicitCastDeclarationStmt declaration) => AddGlobalMember<InterfaceType>(GlobalMemberEnum.Explicitcast, declaration);

        public PropertyCreationStmt TryFindpropertyCreation(IdentifierNode identifier) => globalMemberCollection.GetCreationStatement<PropertyCreationStmt, IPropertyContainer>(GlobalMemberEnum.Property, identifier.Value);

        public MethodDeclarationStmt TryFindFunctionDeclaration(IdentifierNode identifier) => globalMemberCollection.GetCreationStatement<MethodDeclarationStmt, IFunctionContainer<MethodDeclarationStmt>>(GlobalMemberEnum.Function, identifier.Value);

        public OperatorOverloadDeclarationStmt TryFindOperatorOverloadDeclaration(IdentifierNode identifier) => globalMemberCollection.GetCreationStatement<OperatorOverloadDeclarationStmt, IOperatorContainer>(GlobalMemberEnum.OperaterOverload, identifier.Value);

        public ImplicitCastDeclarationStmt TryFindImplicitCastDeclaration(IdentifierNode identifier) => globalMemberCollection.GetCreationStatement<ImplicitCastDeclarationStmt, IImplicitContainer>(GlobalMemberEnum.ImplicitCast, identifier.Value);

        public ExplicitCastDeclarationStmt TryFindExplicitCastDeclaration(IdentifierNode identifier) => globalMemberCollection.GetCreationStatement<ExplicitCastDeclarationStmt, IExplicitContainer>(GlobalMemberEnum.Explicitcast, identifier.Value);

        public IndexerCreationStmt TryFindIndexerCreationStatement(IdentifierNode identifier) => globalMemberCollection.GetCreationStatement<IndexerCreationStmt, IIndexerContainer>(GlobalMemberEnum.Indexer, identifier.Value);

        public override bool IsDuplicateGlobalMember(IdentifierNode identifier) => globalMemberCollection.IsDuplicateCreationStatement(identifier.Value);

        public bool IsDuplicateIndexer(DataType dataType) => globalMemberCollection.GetIndexerStatement(dataType) != null;

        public override string ToString() => $"Interface Node [{name.Value}]";
    }
}
