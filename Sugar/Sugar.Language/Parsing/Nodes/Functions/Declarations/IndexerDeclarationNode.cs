using System;
using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Functions.Declarations
{
    internal sealed class IndexerDeclarationNode : UnnamedFunctionDeclarationNode
    {
        public override NodeType NodeType { get => NodeType.Indexer; }

        public IndexerDeclarationNode(Node _describer, Node _returnType, Node _arguments, Node _body) : base(_describer, _returnType, _arguments, _body)
        {

        }

        public override string ToString() => $"Indexer Declaration Node";
    }
}
