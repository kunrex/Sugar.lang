using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.CtrlStatements
{
    internal sealed class ReturnKeyword : ControlStatement
    {
        public override NodeType NodeType => NodeType.Return; 

        public ReturnKeyword()
        {

        }

        public ReturnKeyword(Node value)
        {
            Children = new List<Node>() { value };
        }

        public override string ToString() => $"Return Node";
    }
}
