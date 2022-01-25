using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Functions.Properties.Accessors
{
    internal sealed class GetNode : AccessorNode
    {
        public override NodeType NodeType => NodeType.Get;

        public GetNode(Node _describer, Node _body) : base(_describer, _body)
        {

        }

        public override string ToString() => $"Get Node";
    }
}
