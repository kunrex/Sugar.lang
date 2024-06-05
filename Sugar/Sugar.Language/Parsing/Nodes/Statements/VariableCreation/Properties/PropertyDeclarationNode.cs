using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Types;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Functions.Properties;

namespace Sugar.Language.Parsing.Nodes.Statements.VariableCreation.Properties
{
    internal class PropertyDeclarationNode : DeclarationNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.PropertyDeclaration; }

        protected readonly PropertyNode property;
        public PropertyNode Property { get => property; }

        public PropertyDeclarationNode(DescriberNode _describer, TypeNode _type, IdentifierNode _name, PropertyNode _property) : base(_describer, _type, _name)
        {
            property = _property;
            Add(property);
        }

        public override string ToString() => $"Property Declaration Node";
    }
}
