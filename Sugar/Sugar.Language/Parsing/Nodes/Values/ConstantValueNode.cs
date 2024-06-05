using System;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Constants;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Values
{
    internal sealed class ConstantValueNode : ValueNode<Constant>
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Constant; }

        public ConstantType Type { get => Token.ConstantType; }

        public ConstantValueNode(Constant _constant) : base(_constant)
        {
            
        }

        public override string ToString() => $"Constant Value Node [Value: {Value}]";
    }
}
