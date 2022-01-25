using System;

namespace Sugar.Language.Parsing.Nodes.Functions.Declarations
{
    internal sealed class IndexerDeclarationNode : UnnamedFunctionDeclarationNode
    {
        public IndexerDeclarationNode(Node _describer, Node _returnType, Node _arguments, Node _body) : base(_describer, _returnType, _arguments, _body)
        {

        }

        public override string ToString() => $"Indexer Declaration Node";
    }
}
