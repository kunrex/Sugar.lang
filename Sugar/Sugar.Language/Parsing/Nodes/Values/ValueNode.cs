using System;

using Sugar.Language.Tokens;

namespace Sugar.Language.Parsing.Nodes.Values
{
    internal abstract class ValueNode<T> : ParseNodeCollection where T : Token
    {
        public readonly T Token;
        public string Value { get => Token.Value; }

        public ValueNode(T token)
        {
            Token = token;
        }

        public override ParseNode AddChild(ParseNode _node) { return this; }
    }
}
