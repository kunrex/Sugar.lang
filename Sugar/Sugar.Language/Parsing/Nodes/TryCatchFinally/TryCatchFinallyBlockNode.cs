using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.TryCatchFinally.Blocks;

namespace Sugar.Language.Parsing.Nodes.TryCatchFinally
{
    internal class TryCatchFinallyBlockNode : ParseNodeCollection
    {
        public override ParseNodeType NodeType { get => ParseNodeType.TryCatchFinally; }

        private readonly TryBlockNode tryNode;
        public TryBlockNode Try { get => tryNode; }

        private readonly CatchBlockNode catchNode;
        public CatchBlockNode Catch { get => catchNode; }

        private readonly FinallyBlockNode finallyNode;
        public FinallyBlockNode Finally { get => finallyNode; }

        protected TryCatchFinallyBlockNode(TryBlockNode _try) : base(_try)
        {
            tryNode = _try;
            catchNode = null;
            finallyNode = null;
        }

        public TryCatchFinallyBlockNode(TryBlockNode _try, CatchBlockNode _catch) : base(_try, _catch)
        {
            tryNode = _try;
            catchNode = _catch;
            finallyNode = null;
        }

        public TryCatchFinallyBlockNode(TryBlockNode _try, FinallyBlockNode _finally) : base(_try, _finally)
        {
            tryNode = _try;
            catchNode = null;
            finallyNode = _finally;
        }

        public TryCatchFinallyBlockNode(TryBlockNode _try, CatchBlockNode _catch, FinallyBlockNode _finally) : base(_try, _catch, _finally)
        {
            tryNode = _try;
            catchNode = _catch;
            finallyNode = _finally;
        }

        public override ParseNode AddChild(ParseNode node) { return this; }

        public override string ToString() => $"Try Catch Finally Node";
    }
}
