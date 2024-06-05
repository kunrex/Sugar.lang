using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Statements.VariableCreation;

namespace Sugar.Language.Parsing.Nodes.Loops
{
    internal sealed class ForeachLoopNode : LoopNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Foreach; }

        private readonly DeclarationNode declaration;
        public DeclarationNode Declaration { get => declaration; }

        private readonly ParseNodeCollection collection;
        public ParseNodeCollection Collection { get => collection; }

        public ForeachLoopNode(DeclarationNode _declaration, ParseNodeCollection _collection, ParseNode _body) : base(_body)
        {
            declaration = _declaration;
            Add(declaration);

            collection = _collection;
            Add(collection);
        }

        public override string ToString() => "Foreach Loop Node";
    }
}
