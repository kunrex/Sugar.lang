using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;

namespace Sugar.Language.Semantics.ActionTrees.VariableCreation
{
    internal sealed class GlobalVariableDeclaration : VariableCreationNode
    {
        public override DescriberEnum Allowed { get => DescriberEnum.AccessModifiers | DescriberEnum.Static; }

        public GlobalVariableDeclaration(DataType _creationType, IdentifierNode _creationName, Describer _describer) : base(_creationType, _creationName, _describer)
        {

        }

        public override string ToString() => $"Global Variable Declaraion [{CreationName.Value}]";
    }
}
