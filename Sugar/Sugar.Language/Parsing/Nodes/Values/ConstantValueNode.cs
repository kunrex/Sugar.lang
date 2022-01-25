using System;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Constants;
using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Values
{
    internal sealed class ConstantValueNode : ValueNode<Constant>
    {
        public ConstantType Type { get => Token.ConstantType; }
        public override NodeType NodeType => NodeType.Constant;

        public ConstantValueNode(Constant _constant) : base(_constant)
        {
            
        }

        public override Node AddChild(Node _node) => throw new NotImplementedException();

        public override string ToString() => $"Constant Value Node [Value: {Value}]";
    }
}
