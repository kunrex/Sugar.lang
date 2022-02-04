using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords.Subtypes.Loops
{
    internal sealed partial class LoopKeyword : Keyword
    {
        public static readonly LoopKeyword Do = new LoopKeyword("do", SyntaxKind.Do);
        public static readonly LoopKeyword For = new LoopKeyword("for", SyntaxKind.For);
        public static readonly LoopKeyword While = new LoopKeyword("while", SyntaxKind.While);
        public static readonly LoopKeyword Foreach = new LoopKeyword("foreach", SyntaxKind.Foreach);
    }
}
