using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Interfaces;
using Sugar.Language.Semantics.ActionTrees.CreationStatements;
using Sugar.Language.Semantics.ActionTrees.DataTypes.Structure;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions;
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

        public LocalVariableDeclarationStmt TryFindVariableCreation(IdentifierNode identifier)
        {
            var members = memberCollection[MemberEnum.Variable];

            var name = identifier.Value;
            foreach (var member in members)
                if (member.Name == name)
                    return (LocalVariableDeclarationStmt)member;

            return null;
        }

        public LocalVoidDeclarationStmt TryFindVoidDeclaration(IdentifierNode identifier, IFunctionArguments arguments) => TryFindIdentifiedArgumentedMember<LocalVoidDeclarationStmt, IFunctionContainer<LocalMethodCreationStmt, LocalVoidDeclarationStmt>>(identifier, arguments, MemberEnum.Void);

        public LocalMethodCreationStmt TryFindMethodDeclaration(IdentifierNode identifier, IFunctionArguments arguments) => TryFindIdentifiedArgumentedMember<LocalMethodCreationStmt, IFunctionContainer<LocalMethodCreationStmt, LocalVoidDeclarationStmt>>(identifier, arguments, MemberEnum.Function);

        private ReturnType TryFindIdentifiedArgumentedMember<ReturnType, Parent>(IdentifierNode identifier, IFunctionArguments arguments, MemberEnum memberType) where ReturnType : CreationStatement<Parent>, IFunction where Parent : IActionTreeNode
        {
            var members = memberCollection[memberType];

            var name = identifier.Value;
            foreach (var function in members)
                if (function.Name == name)
                {
                    var converted = (ReturnType)function;
                    var found = true;

                    for (int i = 0; i < converted.FunctionArguments.Count; i++)
                    {
                        if (!arguments[i].CompareTo(converted.FunctionArguments[i]))
                        {
                            found = false;
                            break;
                        }
                    }

                    if(found)
                        return converted;
                }

            return null;
        }

        public override string ToString() => "Scope Node";
    }
}
