using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Functions.Calling.Structure
{
    internal sealed class FunctionArgumentsNode : NodeCollection<FunctionArgumentNode>
    {
        public override ParseNodeType NodeType { get => ParseNodeType.ArgumentCall; }

        public FunctionArgumentsNode() : base()
        {
            
        }

        public FunctionArgumentsNode AddChild(FunctionArgumentNode node)
        {
            Add(node);

            return this;
        }

        public override string ToString() => $"Function Arguments Node";
    }
}
