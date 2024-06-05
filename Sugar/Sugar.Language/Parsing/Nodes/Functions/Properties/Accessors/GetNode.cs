using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Describers;

namespace Sugar.Language.Parsing.Nodes.Functions.Properties.Accessors
{
    internal sealed class GetNode : AccessorNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Get; }

        public GetNode(DescriberNode _describer, ParseNode _body) : base(_describer, _body)
        {

        }

        public override string ToString() => $"Get Node";
    }
}
