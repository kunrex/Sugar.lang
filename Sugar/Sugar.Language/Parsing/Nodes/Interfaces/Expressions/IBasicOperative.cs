using System;

using Sugar.Language.Tokens.Operators;

namespace Sugar.Language.Parsing.Nodes.Interfaces.Expressions
{
    internal interface IBasicOperative
    {
        public Operator Operator { get; }
    }
}
