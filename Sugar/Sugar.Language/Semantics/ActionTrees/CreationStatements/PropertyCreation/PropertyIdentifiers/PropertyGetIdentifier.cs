using System;

using Sugar.Language.Parsing.Nodes;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation.PropertyIdentifiers
{
    internal sealed class PropertyGetIdentifier : PropertyIdentifier
    {
        public PropertyGetIdentifier(Node _body) : base(_body)
        {

        }

        public override void Print(string indent, bool last) => Console.WriteLine(indent + " Get Property Identifier");
    }
}
