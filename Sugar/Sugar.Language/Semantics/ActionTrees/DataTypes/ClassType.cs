using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.UDDataTypes;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes.Casts;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global.Conversion;

namespace Sugar.Language.Semantics.ActionTrees.DataTypes
{
    internal class ClassType : DataTypeWrapper<ClassNode>, IGeneralContainer
    {
        public override DataTypeEnum TypeEnum { get => DataTypeEnum.Class; }

        public ClassType(IdentifierNode _name, List<ImportNode> _imports, ClassNode _skeleton) : base(_name, _imports,
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

        public IVariableContainer AddDeclaration(GlobalVariableDeclarationStmt declaration) => AddGlobalMember<ClassType, IVariableContainer>(MemberEnum.Variable, declaration, this);

        public IPropertyContainer AddDeclaration(PropertyCreationStmt declaration) => AddGlobalMember<ClassType, IPropertyContainer>(MemberEnum.Property, declaration, this);

        public IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt> AddDeclaration(MethodDeclarationStmt declaration) => AddGlobalMember<ClassType, IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>>(MemberEnum.Function, declaration, this);

        public IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt> AddDeclaration(VoidDeclarationStmt declaration) => AddGlobalMember<ClassType, IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>>(MemberEnum.Void, declaration, this);

        public IConstructorContainer AddDeclaration(ConstructorDeclarationStmt declaration) => AddGlobalMember<ClassType, IConstructorContainer>(MemberEnum.Constructor, declaration, this);

        public IOperatorContainer AddDeclaration(OperatorOverloadDeclarationStmt declaration) => AddGlobalMember<ClassType, IOperatorContainer>(MemberEnum.OperaterOverload, declaration, this);

        public IIndexerContainer AddDeclaration(IndexerCreationStmt declaration) => AddGlobalMember<ClassType, IIndexerContainer>(MemberEnum.Indexer, declaration, this);

        public ICastContainer<ImplicitCastDeclarationStmt, IImplicitContainer> AddDeclaration(ImplicitCastDeclarationStmt declaration) => AddGlobalMember<ClassType, IImplicitContainer>(MemberEnum.ImplicitCast, declaration, this);

        public ICastContainer<ExplicitCastDeclarationStmt, IExplicitContainer> AddDeclaration(ExplicitCastDeclarationStmt declaration) => AddGlobalMember<ClassType, IExplicitContainer>(MemberEnum.ExplicitCast, declaration, this);

        public GlobalVariableDeclarationStmt TryFindVariableCreation(IdentifierNode identifier) => globalMemberCollection.GetCreationStatement<GlobalVariableDeclarationStmt, IVariableContainer>(MemberEnum.Variable, identifier.Value);

        public PropertyCreationStmt TryFindPropertyCreation(IdentifierNode identifier) => globalMemberCollection.GetCreationStatement<PropertyCreationStmt, IPropertyContainer>(MemberEnum.Property, identifier.Value);

        public VoidDeclarationStmt TryFindMethodDeclaration(IdentifierNode identifier) => globalMemberCollection.GetCreationStatement<VoidDeclarationStmt, IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>>(MemberEnum.Void, identifier.Value);

        public MethodDeclarationStmt TryFindFunctionDeclaration(IdentifierNode identifier) => globalMemberCollection.GetCreationStatement<MethodDeclarationStmt, IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>>(MemberEnum.Function, identifier.Value);

        public ConstructorDeclarationStmt TryFindConstructorDeclaration(IdentifierNode identifier) => globalMemberCollection.GetCreationStatement<ConstructorDeclarationStmt,IConstructorContainer>(MemberEnum.Constructor, identifier.Value);

        public OperatorOverloadDeclarationStmt TryFindOperatorOverloadDeclaration(IdentifierNode identifier) => globalMemberCollection.GetCreationStatement<OperatorOverloadDeclarationStmt, IOperatorContainer>(MemberEnum.OperaterOverload, identifier.Value);

        public ImplicitCastDeclarationStmt TryFindImplicitCastDeclaration(IdentifierNode identifier) => globalMemberCollection.GetCreationStatement<ImplicitCastDeclarationStmt, IImplicitContainer>(MemberEnum.ImplicitCast, identifier.Value);

        public ExplicitCastDeclarationStmt TryFindExplicitCastDeclaration(IdentifierNode identifier) => globalMemberCollection.GetCreationStatement<ExplicitCastDeclarationStmt, IExplicitContainer>(MemberEnum.ExplicitCast, identifier.Value);

        public IndexerCreationStmt TryFindIndexerCreationStatement(IdentifierNode identifier) => globalMemberCollection.GetCreationStatement<IndexerCreationStmt, IIndexerContainer>(MemberEnum.Indexer, identifier.Value);

        public override bool IsDuplicate(IdentifierNode identifier) => globalMemberCollection.IsDuplicateCreationStatement(identifier.Value);

        public bool IsDuplicateIndexer(DataType dataType) => globalMemberCollection.GetIndexerStatement(dataType) != null;

        public bool IsDuplicateImplicitCast(DataType dataType) => globalMemberCollection.GetImplicitDeclaration(dataType) != null;

        public bool IsDuplicateExplicitCast(DataType dataType) => globalMemberCollection.GetExplicitDeclaration(dataType) != null;

        public override string ToString() => $"Class Node [{name.Value}]";
    }
}
