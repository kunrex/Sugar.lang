using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Types;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Interfaces.Creation;

namespace Sugar.Language.Parsing.Nodes.Statements.VariableCreation
{
    internal class InitializeNode : DeclarationNode, ICreationNode_Value
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Declaration; }

        private readonly ParseNodeCollection value;
        public ParseNodeCollection Value { get => value; }

        public InitializeNode(DescriberNode _describer, TypeNode _type, IdentifierNode _name, ParseNodeCollection _value) : base(_describer, _type, _name)
        {
            value = _value;
            Add(value);
        }

        public override string ToString() => $"Initialize Node";
    }
}
