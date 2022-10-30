using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes.Structure;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Local;

namespace Sugar.Language.Semantics.ActionTrees
{
    internal sealed class Scope : IFunctionContainer<LocalMethodCreationStmt, LocalVoidDeclarationStmt>
    {
        private readonly Node body;
        public Node Body { get => body; }

        private readonly MemberCollection memberCollection;

        private readonly List<Scope> subScopes;

        public Scope(Node _body)
        {
            body = _body;
            memberCollection = new MemberCollection(MemberEnum.Function | MemberEnum.Void);

            subScopes = new List<Scope>();
        }

        public Scope AddScope(Scope subScope)
        {
            subScopes.Add(subScope);

            return this;
        }

        public IFunctionContainer<LocalMethodCreationStmt, LocalVoidDeclarationStmt> AddDeclaration(LocalMethodCreationStmt declaration)
        {
            memberCollection.Add(MemberEnum.Function, declaration);

            return this;
        }

        public IFunctionContainer<LocalMethodCreationStmt, LocalVoidDeclarationStmt> AddDeclaration(LocalVoidDeclarationStmt declaration)
        {
            memberCollection.Add(MemberEnum.Void, declaration);

            return this;
        }

        public LocalMethodCreationStmt TryFindFunctionDeclaration(IdentifierNode identifier) => memberCollection.GetCreationStatement<LocalMethodCreationStmt, IFunctionContainer<LocalMethodCreationStmt, LocalVoidDeclarationStmt>>(MemberEnum.Function, identifier.Value);

        public LocalVoidDeclarationStmt TryFindMethodDeclaration(IdentifierNode identifier) => memberCollection.GetCreationStatement<LocalVoidDeclarationStmt, IFunctionContainer<LocalMethodCreationStmt, LocalVoidDeclarationStmt>>(MemberEnum.Function, identifier.Value);

        public void Print(string indent, bool last) => Console.WriteLine(indent + " Scope Node");
    }
}
