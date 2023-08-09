using System;
using System.Collections.Generic;

using Sugar.Language.Tokens;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.InvalidNodes.Structure
{
    internal sealed class UnparsedTokenCollectionNode : Node
    {
        public override NodeType NodeType { get => NodeType.NonParsed; }

        private readonly List<Token> tokens;
        public List<Token> Tokens { get => tokens; }

        public UnparsedTokenCollectionNode(List<Token> _tokens)
        {
            tokens = _tokens;
        }

        public override string ToString() => $"Unparsed Token Collection Node";
    }
}
