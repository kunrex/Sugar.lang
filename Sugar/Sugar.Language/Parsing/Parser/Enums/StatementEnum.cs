using System;

namespace Sugar.Language.Parsing.Parser.Enums
{
    [Flags]
    internal enum StatementEnum : sbyte
    {
        Scope = 1,

        LambdaStatement = 2,
        LambdaExpression = 4,

        EmptyStatement = 8, 
        DescriberStatement = 16,
        StandaloneStatement = 32,

        NonEmptyStatement = DescriberStatement | StandaloneStatement,
        GeneralStatement = NonEmptyStatement | EmptyStatement,
        Statement = LambdaStatement | GeneralStatement
    }
}
