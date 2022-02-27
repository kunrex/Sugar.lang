using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.DataTypes;

namespace Sugar.Language.Semantics.ActionTrees.VariableCreation
{
    internal sealed class GlobalVariableDeclaration : VariableCreationNode
    {
        public GlobalVariableDeclaration(DataType _creationType, IdentifierNode _creationName) : base(_creationType, _creationName)
        {
        }

        public override string ToString() => $"Global Variable Declaraion [{CreationName.Value}]";
    }
}
