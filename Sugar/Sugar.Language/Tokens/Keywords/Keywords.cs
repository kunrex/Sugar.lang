using System;

using Sugar.Language.Tokens.Enums;
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
        public static readonly Keyword Var = new Keyword("var", SyntaxKind.Var);
        public static readonly Keyword This = new Keyword("this", SyntaxKind.This);
        public static readonly Keyword When = new Keyword("when", SyntaxKind.When);
        public static readonly Keyword Print = new Keyword("print", SyntaxKind.Print);
        public static readonly Keyword Throw = new Keyword("throw", SyntaxKind.Throw);
        public static readonly Keyword Input = new Keyword("input", SyntaxKind.Input);
        public static readonly Keyword Create = new Keyword("create", SyntaxKind.Create);
        public static readonly Keyword Import = new Keyword("import", SyntaxKind.Import);
        public static readonly Keyword AsType = new Keyword("astype", SyntaxKind.AsType);
        public static readonly Keyword Default = new Keyword("default", SyntaxKind.Default);

        public static readonly Keyword Get = new Keyword("get", SyntaxKind.Get);
        public static readonly Keyword Set = new Keyword("set", SyntaxKind.Set);

        public static readonly Keyword Try = new Keyword("try", SyntaxKind.Try);
        public static readonly Keyword Catch = new Keyword("catch", SyntaxKind.Catch);
        public static readonly Keyword Finally = new Keyword("finally", SyntaxKind.Finally);

        public static readonly Keyword[] Keywords =
        {
            Input, Throw, Import, Print, Create, Var, Default, When, This, Get, Set, Try, Catch, Finally, AsType,

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
