using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Types;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Values.Generics;

using Sugar.Language.Parsing.Nodes.Functions.Properties;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

namespace Sugar.Language.Parsing.Nodes.Functions.Declarations
{
    internal sealed class IndexerDeclarationNode : BaseFunctionDeclarationNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Indexer; }

        private readonly PropertyNode property;
        public  PropertyNode Property { get => property; }

        public IndexerDeclarationNode(DescriberNode _describer, TypeNode _returnType, FunctionParamatersNode _arguments, PropertyNode _property) : base(_describer, _returnType, _arguments, _property)
        {
            property = _property;
        }

        public IndexerDeclarationNode(DescriberNode _describer, TypeNode _returnType, FunctionParamatersNode _arguments, PropertyNode _property, GenericDeclarationNode _generic) : base(_describer, _returnType, _arguments, _property, _generic)
        {
            property = _property;
        }

        public override string ToString() => $"Indexer Declaration Node";
    }
}
