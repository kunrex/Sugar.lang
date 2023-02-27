using System;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Describers;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation.PropertyIdentifiers
{
    internal sealed class PropertyGetIdentifier : PropertyIdentifier
    {
        public override ActionNodeEnum ActionNodeType { get => ActionNodeEnum.PropertyGet; }

        public PropertyGetIdentifier(Describer _describer, Node _body) : base(_describer, _body)
        {

        }

        public override void Print(string indent, bool last) => Console.WriteLine(indent + " Get Property Identifier");
    }
}
