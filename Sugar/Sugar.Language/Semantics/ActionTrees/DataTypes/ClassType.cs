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
using Sugar.Language.Parsing.Nodes.Functions.Declarations.OperatorOverloading;
using Sugar.Language.Semantics.ActionTrees.CreationStatements;
using Sugar.Language.Semantics.ActionTrees.Interfaces;

namespace Sugar.Language.Semantics.ActionTrees.DataTypes
{
    internal sealed class ClassType : DataTypeWrapper<ClassNode>, IVariableContainer, IPropertyContainer, IFunctionContainer<MethodDeclarationStmt>, IOperatorContainer, IImplicitContainer, IExplicitContainer
    {
        public override DataTypeEnum TypeEnum { get => DataTypeEnum.Class; }

        private readonly List<PropertyCreationStmt> propertyCreationNodes;
        private readonly List<VariableCreationStmt> globalVariableCreations;

        private readonly List<MethodDeclarationStmt> functionDeclarations;

        private readonly List<ExplicitCastDeclarationStmt> explicitCasts;
        private readonly List<ImplicitCastDeclarationStmt> implicitCasts;
        private readonly List<OperatorOverloadDeclarationStmt> operatorOverloads;

        public ClassType(IdentifierNode _name, List<ImportNode> _imports, ClassNode _skeleton) : base(_name, _imports, _skeleton)
        {
            propertyCreationNodes = new List<PropertyCreationStmt>();
            globalVariableCreations = new List<VariableCreationStmt>();

            functionDeclarations = new List<MethodDeclarationStmt>();

            explicitCasts = new List<ExplicitCastDeclarationStmt>();
            implicitCasts = new List<ImplicitCastDeclarationStmt>();

            operatorOverloads = new List<OperatorOverloadDeclarationStmt>();
        }

        public IVariableContainer AddDeclaration(VariableCreationStmt declaration)
        {
            globalVariableCreations.Add(declaration);

            return this;
        }

        public IPropertyContainer AddDeclaration(PropertyCreationStmt declaration)
        {
            propertyCreationNodes.Add(declaration);

            return this;
        }

        public IFunctionContainer<MethodDeclarationStmt> AddDeclaration(MethodDeclarationStmt declaration)
        {
            functionDeclarations.Add(declaration);

            return this;
        }

        public IOperatorContainer AddDeclaration(OperatorOverloadDeclarationStmt declaration)
        {
            operatorOverloads.Add(declaration);

            return this;
        }

        public ICastContainer<ImplicitCastDeclarationStmt, ImplicitCastDeclarationNode, IImplicitContainer> AddDeclaration(ImplicitCastDeclarationStmt declaration)
        {
            implicitCasts.Add(declaration);

            return this;
        }

        public ICastContainer<ExplicitCastDeclarationStmt, ExplicitCastDeclarationNode, IExplicitContainer> AddDeclaration(ExplicitCastDeclarationStmt declaration)
        {
            explicitCasts.Add(declaration);

            return this;
        }

        public VariableCreationStmt TryFindVariableCreation(IdentifierNode identifier) => TryFindEntity<VariableCreationStmt, IVariableContainer>(identifier, globalVariableCreations);

        public PropertyCreationStmt TryFindpropertyCreation(IdentifierNode identifier) => TryFindEntity<PropertyCreationStmt, IPropertyContainer>(identifier, propertyCreationNodes);

        public MethodDeclarationStmt TryFindFunctionDeclaration(IdentifierNode identifier) => TryFindEntity<MethodDeclarationStmt, IFunctionContainer<MethodDeclarationStmt>>(identifier, functionDeclarations);

        public OperatorOverloadDeclarationStmt TryFindOperatorOverloadDeclaration(IdentifierNode identifier) => TryFindEntity<OperatorOverloadDeclarationStmt, IOperatorContainer>(identifier, operatorOverloads);

        public ImplicitCastDeclarationStmt TryFindImplicitCastDeclaration(IdentifierNode identifier) => TryFindEntity<ImplicitCastDeclarationStmt, IImplicitContainer>(identifier, implicitCasts);

        public ExplicitCastDeclarationStmt TryFindExplicitCastDeclaration(IdentifierNode identifier) => TryFindEntity<ExplicitCastDeclarationStmt, IExplicitContainer>(identifier, explicitCasts);

        private T TryFindEntity<T, Parent>(IdentifierNode identifier, List<T> collection) where T : CreationStatement<Parent> where Parent : IActionTreeNode
        {
            foreach (var creation in collection)
                if (creation.Name == identifier.Value)
                    return creation;

            return null;
        }

        public override string ToString() => $"Class Node [{name.Value}]";
    }
}
