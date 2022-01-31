using System;

using Sugar.Language.Tokens.Keywords.Subtypes.Types;
using Sugar.Language.Tokens.Keywords.Subtypes.Loops;
using Sugar.Language.Tokens.Keywords.Subtypes.Entities;
using Sugar.Language.Tokens.Keywords.Subtypes.Functions;
using Sugar.Language.Tokens.Keywords.Subtypes.Conditions;
using Sugar.Language.Tokens.Keywords.Subtypes.Describers;
using Sugar.Language.Tokens.Keywords.Subtypes.ControlStatements;

namespace Sugar.Language.Tokens.Keywords
{
    internal partial class Keyword : Token
    {
        public static readonly Keyword Var = new Keyword("var", 0);
        public static readonly Keyword This = new Keyword("this", 1);
        public static readonly Keyword When = new Keyword("when", 2);
        public static readonly Keyword Print = new Keyword("print", 3);
        public static readonly Keyword Throw = new Keyword("throw", 4);
        public static readonly Keyword Input = new Keyword("input", 5);
        public static readonly Keyword Create = new Keyword("create", 6);
        public static readonly Keyword Import = new Keyword("import", 7);
        public static readonly Keyword Default = new Keyword("default", 8);

        public static readonly Keyword Get = new Keyword("get", 9);
        public static readonly Keyword Set = new Keyword("set", 10);

        public static readonly Keyword Try = new Keyword("try", 11);
        public static readonly Keyword Catch = new Keyword("catch", 12);
        public static readonly Keyword Finally = new Keyword("finally", 13);

        public static readonly Keyword[] Keywords =
        {
            Input, Throw, Import, Print, Create, Var, Default, When, This, Get, Set, Try, Catch, Finally,

            TypeKeyword.Object, TypeKeyword.SByte, TypeKeyword.Byte, TypeKeyword.Short, TypeKeyword.UShort, TypeKeyword.Int,
            TypeKeyword.UInt, TypeKeyword.Long, TypeKeyword.Ulong, TypeKeyword.Float, TypeKeyword.Double, TypeKeyword.Decimal,
            TypeKeyword.Bool, TypeKeyword.String, TypeKeyword.Char, TypeKeyword.Array,

            EntityKeyword.Class, EntityKeyword.Struct, EntityKeyword.Interface, EntityKeyword.Namespace,
            EntityKeyword.Enum,

            ConditionKeyword.If, ConditionKeyword.Else, ConditionKeyword.Switch, ConditionKeyword.Case,

            LoopKeyword.For, LoopKeyword.While, LoopKeyword.Do, LoopKeyword.Foreach,

            ControlKeyword.Return, ControlKeyword.Continue, ControlKeyword.Break,

            DescriberKeyword.Static, DescriberKeyword.Public, DescriberKeyword.Private, DescriberKeyword.Protected, DescriberKeyword.Abstract,
            DescriberKeyword.Virtual, DescriberKeyword.Override, DescriberKeyword.Sealed, DescriberKeyword.Const, DescriberKeyword.ReadOnly,
            DescriberKeyword.In, DescriberKeyword.Out, DescriberKeyword.Ref,

            FunctionKeyword.Operator, FunctionKeyword.Explicit, FunctionKeyword.Implicit, FunctionKeyword.Void, FunctionKeyword.Constructor,
            FunctionKeyword.Indexer
        };
    }
}
