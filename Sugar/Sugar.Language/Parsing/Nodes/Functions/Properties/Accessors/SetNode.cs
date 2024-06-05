using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Describers;
using Sugar.Language.Parsing.Nodes.Statements.VariableCreation;

namespace Sugar.Language.Parsing.Nodes.Functions.Properties.Accessors
{
    internal sealed class SetNode : AccessorNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Set; }

        private readonly DeclarationNode value;
        public ParseNodeCollection ValueNode { get => value; }

        public SetNode(DescriberNode _describer, DeclarationNode _valueDeclaration, ParseNode _body) : base(_describer, _body)
        {
            value = _valueDeclaration;
            Add(_valueDeclaration);
        }

        public override string ToString() => $"Set Node";
    }
}
