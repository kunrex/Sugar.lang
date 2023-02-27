using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.NodeGroups;

namespace Sugar.Language.Parsing.Nodes.Describers
{
    internal class DescriberNode : ExpressionListNode
    {
        public override NodeType NodeType => NodeType.Describer;

        public DescriberNode()
        {

        }

        public DescriberNode(Node _keyword) : base(_keyword)
        {
            
        }

        public DescriberNode(List<Node> _keywords) : base(_keywords)
        {
           
        }

        public override string ToString() => $"Describer Node";
    }
}
