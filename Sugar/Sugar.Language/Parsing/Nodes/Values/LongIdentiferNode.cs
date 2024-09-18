using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Expressions.Associative;
using Sugar.Language.Services.Interfaces;

namespace Sugar.Language.Parsing.Nodes.Values
{
    //change referencing to just match strings cause its easier
    internal sealed class LongIdentiferNode : ParseNodeCollection
    {
        public override ParseNodeType NodeType { get => ParseNodeType.LongIdentifier; }

        private readonly DotExpression dot;
        public DotExpression Dot { get => dot; }

        private readonly string fullName;
        public string FullName { get => fullName; }

        private readonly string[] fullSplit;
        public int SplitLength { get => fullSplit.Length; }

        public LongIdentiferNode(DotExpression _dot) : base(_dot)
        {
            dot = _dot;
 
            var builder = new StringBuilder();
            var current = dot;
            while (current != null)
            {
                builder.Append($"{((IdentifierNode)dot.LHS).Value}.");

                if (dot.RHS.NodeType == ParseNodeType.Dot)
                    current = (DotExpression)dot.RHS;
                else
                    current = null;
            }

            builder.Remove(builder.Length - 1, 1);
            fullName = builder.ToString();
            fullSplit = fullName.Split('.');
        }

        public string NameAt(int index) => fullSplit[index];
    }
}
