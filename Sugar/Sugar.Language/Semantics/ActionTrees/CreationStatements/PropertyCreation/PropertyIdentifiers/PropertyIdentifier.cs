using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Local;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation.PropertyIdentifiers
{
    internal abstract class PropertyIdentifier : IFunctionContainer<LocalMethodCreationStmt, LocalVoidDeclarationStmt>
    {
        protected readonly Scope scope;
        public Scope Scope { get => scope; }

        public PropertyIdentifier(Node _body)
        {
            scope = new Scope(_body);
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

        public LocalMethodCreationStmt TryFindFunctionDeclaration(IdentifierNode identifier) => scope.TryFindFunctionDeclaration(identifier);

        public LocalVoidDeclarationStmt TryFindMethodDeclaration(IdentifierNode identifier) => scope.TryFindMethodDeclaration(identifier);

        public abstract void Print(string indent, bool last);
    }
}
