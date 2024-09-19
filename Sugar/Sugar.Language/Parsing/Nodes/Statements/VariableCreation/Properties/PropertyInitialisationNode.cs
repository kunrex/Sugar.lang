using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Types;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Interfaces.Creation;

using Sugar.Language.Parsing.Nodes.Functions.Properties;

namespace Sugar.Language.Parsing.Nodes.Statements.VariableCreation.Properties
{
    internal sealed class PropertyInitialisationNode : PropertyDeclarationNode, ICreationNode_Value
    {
        public override ParseNodeType NodeType { get => ParseNodeType.PropertyInitialise; }

        private readonly ParseNodeCollection value;
        public ParseNodeCollection Value { get => value; }

        public PropertyInitialisationNode(DescriberNode _describer, TypeNode _type, IdentifierNode _name, PropertyNode _property, ParseNodeCollection _value) : base(_describer, _type, _name, _property)
        {
            value = _value;
            Add(value);
        }

        public override string ToString() => $"Property Initialisation Node";
    }
}
