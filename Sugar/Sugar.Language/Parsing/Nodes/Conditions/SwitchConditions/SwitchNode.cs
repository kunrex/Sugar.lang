using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.NodeGroups;

namespace Sugar.Language.Parsing.Nodes.Conditions.SwitchConditions
{
    internal sealed class SwitchNode : ParseNodeCollection
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Switch; }

        private readonly CompoundStatementNode cases;
        public CompoundStatementNode Cases { get => cases; }

        private readonly ParseNodeCollection value;
        public ParseNodeCollection Value { get => value; }

        public SwitchNode(ParseNodeCollection _value, CompoundStatementNode _cases) : base(_cases, _value)
        {
            cases = _cases;
            value = _value;
        }

        public override string ToString() => $"Switch Node";
    }
}
