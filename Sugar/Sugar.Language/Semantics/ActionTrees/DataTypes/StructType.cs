using System;
using System.Collections.Generic;

using Sugar.Language.Tokens.Operators;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.UDDataTypes;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes.Casts;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global.Conversion;

namespace Sugar.Language.Semantics.ActionTrees.DataTypes
{
    internal sealed class StructType : DataTypeWrapper<StructNode>, IGeneralContainer
    {
        public override ActionNodeEnum ActionNodeType { get => ActionNodeEnum.Struct; }

        public StructType(IdentifierNode _name, List<ImportNode> _imports, StructNode _skeleton) : base(_name,
                    _imports,
                    MemberEnum.Void |
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

        public IVariableContainer AddDeclaration(GlobalVariableDeclarationStmt declaration) => AddGlobalMember<StructType, IVariableContainer>(MemberEnum.Variable, declaration, this);

        public IPropertyContainer AddDeclaration(PropertyCreationStmt declaration) => AddGlobalMember<StructType, IPropertyContainer>(MemberEnum.Property, declaration, this);

        public IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt> AddDeclaration(MethodDeclarationStmt declaration) => AddGlobalMember<StructType, IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>>(MemberEnum.Function, declaration, this);

        public IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt> AddDeclaration(VoidDeclarationStmt declaration) => AddGlobalMember<StructType, IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>>(MemberEnum.Void, declaration, this);

        public IConstructorContainer AddDeclaration(ConstructorDeclarationStmt declaration) => AddGlobalMember<StructType, IConstructorContainer>(MemberEnum.Constructor, declaration, this);

        public IOperatorContainer AddDeclaration(OperatorOverloadDeclarationStmt declaration) => AddGlobalMember<StructType, IOperatorContainer>(MemberEnum.OperaterOverload, declaration, this);

        public IIndexerContainer AddDeclaration(IndexerCreationStmt declaration) => AddGlobalMember<StructType, IIndexerContainer>(MemberEnum.Indexer, declaration, this);

        public ICastContainer<ImplicitCastDeclarationStmt, IImplicitContainer> AddDeclaration(ImplicitCastDeclarationStmt declaration) => AddGlobalMember<StructType, IImplicitContainer>(MemberEnum.ImplicitCast, declaration, this);

        public ICastContainer<ExplicitCastDeclarationStmt, IExplicitContainer> AddDeclaration(ExplicitCastDeclarationStmt declaration) => AddGlobalMember<StructType, IExplicitContainer>(MemberEnum.ExplicitCast, declaration, this);

        public GlobalVariableDeclarationStmt TryFindVariableCreation(IdentifierNode identifier) => TryFindIdentifiedMember<GlobalVariableDeclarationStmt, IVariableContainer>(identifier, MemberEnum.Variable);

        public PropertyCreationStmt TryFindPropertyCreation(IdentifierNode identifier) => TryFindIdentifiedMember<PropertyCreationStmt, IPropertyContainer>(identifier, MemberEnum.Properties);

        public VoidDeclarationStmt TryFindMethodDeclaration(IdentifierNode identifier, FunctionArguments arguments) => TryFindIdentifiedArgumentedMember<VoidDeclarationStmt, IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>>(identifier, arguments, MemberEnum.Void);

        public MethodDeclarationStmt TryFindFunctionDeclaration(IdentifierNode identifier, FunctionArguments arguments) => TryFindIdentifiedArgumentedMember<MethodDeclarationStmt, IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>>(identifier, arguments, MemberEnum.Function);

        public ConstructorDeclarationStmt TryFindConstructorDeclaration(IdentifierNode identifier, FunctionArguments arguments) => TryFindIdentifiedArgumentedMember<ConstructorDeclarationStmt, IConstructorContainer>(identifier, arguments, MemberEnum.Constructor);

        public OperatorOverloadDeclarationStmt TryFindOperatorOverloadDeclaration(Operator op, DataType operhand) => TryFindOperatorOverload(op, operhand);

        public OperatorOverloadDeclarationStmt TryFindOperatorOverloadDeclaration(Operator op, DataType operhand1, DataType operhand2) => TryFindOperatorOverload(op, operhand1, operhand2);

        public ImplicitCastDeclarationStmt TryFindImplicitCastDeclaration(DataType external) => TryFindCast<ImplicitCastDeclarationStmt, IImplicitContainer>(external, MemberEnum.ImplicitCast);

        public ExplicitCastDeclarationStmt TryFindExplicitCastDeclaration(DataType external) => TryFindCast<ExplicitCastDeclarationStmt, IExplicitContainer>(external, MemberEnum.ExplicitCast);

        public IndexerCreationStmt TryFindIndexerCreationStatement(IdentifierNode identifier) => globalMemberCollection.GetCreationStatement<IndexerCreationStmt, IIndexerContainer>(MemberEnum.Indexer, identifier.Value);

        public override string ToString() => $"Struct Node [{name.Value}]";
    }
}
