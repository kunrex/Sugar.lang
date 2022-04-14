using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;

namespace Sugar.Language.Semantics.ActionTrees.CreatableNodes.VariableCreation
{
    internal class GlobalVariableDeclaration : VariableCreationNode
    {
        public GlobalVariableDeclaration(DataType _creationType, IdentifierNode _creationName, Describer _describer) : base(
            _creationType,
            _creationName,
            _describer,
            DescriberEnum.AccessModifiers | DescriberEnum.Static | DescriberEnum.MutabilityModifier)
        {
            
        }

        public override string ToString() => $"Global Variable Declaraion [{creationName.Value}]";
    }
}
