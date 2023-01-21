using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Interfaces;
using Sugar.Language.Semantics.ActionTrees.DataTypes.Structure;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Local;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Local;

namespace Sugar.Language.Semantics.ActionTrees
{
    internal sealed class Scope : ActionTreeNode<IScopeParent>, IScopeParent
    {
        private readonly Node body;
        public Node Body { get => body; }

        private readonly List<Scope> subScopes;
        private readonly MemberCollection memberCollection;

        public override ActionNodeEnum ActionNodeType { get => ActionNodeEnum.Scope; }

        public Scope(Node _body)
        {
            body = _body;
            memberCollection = new MemberCollection(MemberEnum.Function | MemberEnum.Void | MemberEnum.Variable);

            subScopes = new List<Scope>();
        }

        public Scope AddScope(Scope subScope)
        {
            subScopes.Add(subScope);
            subScope.SetParent(this);

            return this;
        }

        public ILocalVariableContainer AddDeclaration(LocalVariableDeclarationStmt declaration)
        {
            memberCollection.Add(MemberEnum.Variable, declaration);
            declaration.SetParent(Parent);

            return this;
        }

        public IFunctionContainer<LocalMethodCreationStmt, LocalVoidDeclarationStmt> AddDeclaration(LocalMethodCreationStmt declaration)
        {
            memberCollection.Add(MemberEnum.Function, declaration);
            declaration.SetParent(Parent);

            return this;
        }

        public IFunctionContainer<LocalMethodCreationStmt, LocalVoidDeclarationStmt> AddDeclaration(LocalVoidDeclarationStmt declaration)
        {
            memberCollection.Add(MemberEnum.Void, declaration);
            declaration.SetParent(Parent);

            return this;
        }

        public LocalVariableDeclarationStmt TryFindVariableCreation(IdentifierNode identifier) => memberCollection.GetCreationStatement<LocalVariableDeclarationStmt, ILocalVariableContainer>(MemberEnum.Variable, identifier.Value);

        public LocalMethodCreationStmt TryFindFunctionDeclaration(IdentifierNode identifier) => memberCollection.GetCreationStatement<LocalMethodCreationStmt, IFunctionContainer<LocalMethodCreationStmt, LocalVoidDeclarationStmt>>(MemberEnum.Function, identifier.Value);

        public LocalVoidDeclarationStmt TryFindMethodDeclaration(IdentifierNode identifier) => memberCollection.GetCreationStatement<LocalVoidDeclarationStmt, IFunctionContainer<LocalMethodCreationStmt, LocalVoidDeclarationStmt>>(MemberEnum.Function, identifier.Value);

        public override string ToString() => "Scope Node";
    }
}
