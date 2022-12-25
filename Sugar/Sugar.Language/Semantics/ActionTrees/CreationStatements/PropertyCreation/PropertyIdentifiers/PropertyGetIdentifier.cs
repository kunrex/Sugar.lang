using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Semantics.ActionTrees.Enums;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation.PropertyIdentifiers
{
    internal sealed class PropertyGetIdentifier : PropertyIdentifier
    {
        public override ActionNodeEnum ActionNodeType { get => ActionNodeEnum.PropertyGet; }

        public PropertyGetIdentifier(Node _body) : base(_body)
        {

        }

        public override void Print(string indent, bool last) => Console.WriteLine(indent + " Get Property Identifier");
    }
}
