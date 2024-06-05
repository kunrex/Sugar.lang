using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Values.Generics;

using Sugar.Language.Parsing.Nodes.Functions.Calling.Structure;
using Sugar.Language.Parsing.Nodes.NodeGroups;

namespace Sugar.Language.Parsing.Nodes.Functions.Calling
{
    internal sealed class ConstructorCallNode : BaseFunctionCallNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.ConstructorCall; }

        private readonly ExpressionListNode expressions;
        public ExpressionListNode Expressions { get => expressions; }

        public ConstructorCallNode(ParseNodeCollection _name, FunctionArgumentsNode _arguments) : base(_name, _arguments)
        {

        }

        public ConstructorCallNode(ParseNodeCollection _name, FunctionArgumentsNode _arguments, ExpressionListNode _expressions) : base(_name, _arguments)
        {
            expressions = _expressions;
            Add(expressions);
        }

        public ConstructorCallNode(ParseNodeCollection _value, FunctionArgumentsNode _arguments, GenericCallNode _generic) : base(_value, _arguments, _generic)
        {

        }

        public ConstructorCallNode(ParseNodeCollection _value, FunctionArgumentsNode _arguments, GenericCallNode _generic, ExpressionListNode _expressions) : base(_value, _arguments, _generic)
        {
            expressions = _expressions;
            Add(expressions);
        }

        public override string ToString() => $"Constructor Call Node";
    }
}
