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

        private readonly IReadOnlyList<string> fullSplit;
        public int SplitLength { get => fullSplit.Count; }
        
        public LongIdentiferNode(DotExpression _dot) : base(_dot)
        {
            dot = _dot;
            DotExpression current = dot;

            var split = new List<string>();
            while (current != null)
            {
                split.Add(((IdentifierNode)dot.LHS).Value);;

                if (dot.RHS.NodeType == ParseNodeType.Dot)
                    current = (DotExpression)dot.RHS;
                else
                {
                    split.Add(( (IdentifierNode)dot.RHS).Value);;
                    break;
                }
            }

            fullSplit = split.AsReadOnly();
        }

        public string NameAt(int index) => fullSplit[index];
    }
}
