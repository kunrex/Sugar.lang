using System;

using Sugar.Language.Tokens;

namespace Sugar.Language.Parsing.Nodes.Values
{
    internal abstract class ValueNode<T> : Node where T : Token
    {
        public readonly T Token;
        public string Value { get => Token.Value; }

        public ValueNode(T token)
        {
            Token = token;
        }

        public override Node AddChild(Node _node) => throw new NotImplementedException();
    }
}
