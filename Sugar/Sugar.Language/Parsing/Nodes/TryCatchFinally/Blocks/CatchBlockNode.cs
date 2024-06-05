using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

namespace Sugar.Language.Parsing.Nodes.TryCatchFinally.Blocks
{
    internal sealed class CatchBlockNode : BlockNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Catch; }

        private readonly FunctionParamatersNode arguments;
        public FunctionParamatersNode Arguments { get => arguments; }

        public CatchBlockNode(FunctionParamatersNode _arguments, ParseNode _body) : base(_body)
        {
            arguments = _arguments;
            Add(arguments);
        }

        public override string ToString() => $"Catch Node";
    }
}


