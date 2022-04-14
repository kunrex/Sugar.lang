using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.UDDataTypes;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreatableNodes.VariableCreation;

namespace Sugar.Language.Semantics.ActionTrees.DataTypes
{
    internal sealed class ClassType : DataTypeWrapper<ClassNode>, IVariableContainer
    {
        public override DataTypeEnum TypeEnum { get => DataTypeEnum.Class; }

        private readonly List<GlobalVariableDeclaration> globalVariableDeclarations;

        public ClassType(IdentifierNode _name, List<ImportNode> _imports, ClassNode _skeleton) : base(_name, _imports, _skeleton)
        {
            globalVariableDeclarations = new List<GlobalVariableDeclaration>();
        }

        public GlobalVariableDeclaration TryFindVariableDeclaration(IdentifierNode identifier)
        {
            foreach (var declaration in globalVariableDeclarations)
                if (declaration.Name == identifier.Value)
                    return declaration;

            return null;
        }

        public IVariableContainer AddDeclaration(GlobalVariableDeclaration declaration)
        {
            globalVariableDeclarations.Add(declaration);

            return this;
        }

        public override string ToString() => $"Class Node [{name.Value}]";
    }
}
