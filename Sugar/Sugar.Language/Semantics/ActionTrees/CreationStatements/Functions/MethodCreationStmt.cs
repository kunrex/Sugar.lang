using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Local;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Local;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Local.FunctionArguments;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions
{
    internal abstract class MethodCreationStmt<Parent> : ReturnableCreationStatement<Parent>, IFunction where Parent : IActionTreeNode
    {
        protected readonly Scope scope;
        public Scope Scope { get => scope; }

        protected readonly FunctionDeclArgs arguments;
        public FunctionDeclArgs FunctionArguments { get => arguments; }

        public MethodCreationStmt(DataType _creationType, string _name, Describer _describer, DescriberEnum _allowed, FunctionDeclArgs _arguments, Node _nodeBody) : base(
            _creationType,
            _name,
            _describer,
            _allowed)
        {
            arguments = _arguments;

            scope = new Scope(_nodeBody);
            scope.SetParent(this);
        }

        public ILocalVariableContainer AddDeclaration(LocalVariableDeclarationStmt declaration)
        {
            scope.AddDeclaration(declaration);

            return this;
        }

        public IFunctionContainer<LocalMethodCreationStmt, LocalVoidDeclarationStmt> AddDeclaration(LocalMethodCreationStmt declaration)
        {
            scope.AddDeclaration(declaration);

            return this;
        }

        public IFunctionContainer<LocalMethodCreationStmt, LocalVoidDeclarationStmt> AddDeclaration(LocalVoidDeclarationStmt declaration)
        {
            scope.AddDeclaration(declaration);

            return this;
        }

        public FunctionArgumentDeclarationStmt TryFindFunctionArgument(IdentifierNode identifier) => arguments[identifier.Value];

        public LocalVariableDeclarationStmt TryFindVariableCreation(IdentifierNode identifier) => scope.TryFindVariableCreation(identifier);

        public LocalVoidDeclarationStmt TryFindVoidDeclaration(IdentifierNode identifier, IFunctionArguments arguments) => scope.TryFindVoidDeclaration(identifier, arguments);

        public LocalMethodCreationStmt TryFindMethodDeclaration(IdentifierNode identifier, IFunctionArguments arguments) => scope.TryFindMethodDeclaration(identifier, arguments);
    }
}