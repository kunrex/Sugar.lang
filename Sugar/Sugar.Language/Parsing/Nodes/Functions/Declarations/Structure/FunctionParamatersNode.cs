using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure
{
    internal sealed class FunctionParamatersNode : NodeCollection<FunctionParamaterNode>
    {
        public override ParseNodeType NodeType { get => ParseNodeType.FunctionDeclaration; }

        public FunctionParamatersNode() : base()
        {

        }

        public FunctionParamatersNode AddChild(FunctionParamaterNode node)
        {
            Add(node);

            return this;
        }

        public override string ToString() => $"Function Paramaters Node";
    }
}
