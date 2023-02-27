using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Interfaces;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Local;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation.PropertyIdentifiers
{
    internal abstract class PropertyIdentifier : IFunctionContainer<LocalMethodCreationStmt, LocalVoidDeclarationStmt>, IDescribable
    {
        protected readonly Scope scope;
        public Scope Scope { get => scope; }

        public abstract ActionNodeEnum ActionNodeType { get; }

        private readonly Describer describer;
        public Describer Describer { get => describer; }

        public DescriberEnum Allowed { get => DescriberEnum.AccessModifiers; }

        public PropertyIdentifier(Describer _describer, Node _body)
        {
            describer = _describer;
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

        public bool ValidateDescriber() => describer.ValidateDescriber(Allowed);

        public bool CheckDescription(DescriberEnum description) => describer.CheckDescription(description);

        public LocalVoidDeclarationStmt TryFindVoidDeclaration(IdentifierNode identifier, IFunctionArguments arguments) => scope.TryFindVoidDeclaration(identifier, arguments);

        public LocalMethodCreationStmt TryFindMethodDeclaration(IdentifierNode identifier, IFunctionArguments arguments) => scope.TryFindMethodDeclaration(identifier, arguments);

        public abstract void Print(string indent, bool last);
    }
}
