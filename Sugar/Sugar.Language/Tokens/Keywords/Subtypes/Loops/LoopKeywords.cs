using System;

namespace Sugar.Language.Tokens.Keywords.Subtypes.Loops
{
    internal sealed partial class LoopKeyword : Keyword
    {
        public static readonly LoopKeyword Do = new LoopKeyword("do", 0);
        public static readonly LoopKeyword For = new LoopKeyword("for", 1);
        public static readonly LoopKeyword While = new LoopKeyword("while", 2);
    }
}
