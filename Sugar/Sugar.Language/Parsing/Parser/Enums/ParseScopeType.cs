using System;

namespace Sugar.Language.Parsing.Parser.Enums
{
    [Flags]
    internal enum ParseScopeType : short
    {
        Scope = 1,
        SingleLine = 2,
        LambdaStatement = 4,
        LambdaExpression = 8,
        Lambda = LambdaStatement | LambdaExpression
    }
}
