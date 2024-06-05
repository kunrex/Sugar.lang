using System;

using Sugar.Language.Parsing.Nodes.Types;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Describers;
using Sugar.Language.Parsing.Nodes.NodeGroups;

using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

namespace Sugar.Language.Parsing.Nodes.Functions.Declarations
{
    internal sealed class ConstructorDeclarationNode : BaseFunctionDeclarationNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.ConstructorDeclaration; }

        private readonly ExpressionListNode parentOverrideNode;
        public ExpressionListNode ParentOverrideNode { get => parentOverrideNode; }

        public ConstructorDeclarationNode(DescriberNode _describer, TypeNode _returnType, FunctionParamatersNode _arguments, ParseNode _body) : base(_describer, _returnType, _arguments, _body)
        {
            parentOverrideNode = null;
        }

        public ConstructorDeclarationNode(DescriberNode _describer, TypeNode _returnType, FunctionParamatersNode _arguments, ParseNode _body, ExpressionListNode _parent) : base(_describer, _returnType, _arguments, _body)
        {
            parentOverrideNode = _parent;
            Add(parentOverrideNode);
        }

        public override string ToString() => $"Constructor Declaration Node";
    }
}
