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
        public static readonly Keyword And = new Keyword("and", 1);
        public static readonly Keyword This = new Keyword("this", 2);
        public static readonly Keyword When = new Keyword("when", 3);
        public static readonly Keyword Print = new Keyword("print", 4);
        public static readonly Keyword Throw = new Keyword("throw", 5);
        public static readonly Keyword Input = new Keyword("input", 6);
        public static readonly Keyword Create = new Keyword("create", 7);
        public static readonly Keyword Import = new Keyword("import", 8);
        public static readonly Keyword Default = new Keyword("default", 9);

        public static readonly Keyword Get = new Keyword("get", 10);
        public static readonly Keyword Set = new Keyword("set", 11);

        public static readonly Keyword[] LoopKeywords = { LoopKeyword.For, LoopKeyword.While, LoopKeyword.Do };
        public static readonly Keyword[] FunctionKeywords = { FunctionKeyword.Operator, FunctionKeyword.Explicit, FunctionKeyword.Implicit };
        public static readonly Keyword[] EntitiKeywords = { EntityKeyword.Class, EntityKeyword.Struct, EntityKeyword.Interface, EntityKeyword.Namespace, EntityKeyword.Enum };
        public static readonly Keyword[] ControlStatementKeywords = { ControlKeyword.Return, ControlKeyword.Continue, ControlKeyword.Break };
        public static readonly Keyword[] ConditionKeywords = { ConditionKeyword.If, ConditionKeyword.Else, ConditionKeyword.Switch };
        public static readonly Keyword[] TypeKeywords = { TypeKeyword.SByte, TypeKeyword.Byte, TypeKeyword.Short, TypeKeyword.UShort, TypeKeyword.Int, TypeKeyword.UInt, TypeKeyword.Long, TypeKeyword.Ulong, TypeKeyword.Float, TypeKeyword.Double, TypeKeyword.Decimal, TypeKeyword.Bool, TypeKeyword.String, TypeKeyword.Char, Var, TypeKeyword.Object };
        public static readonly Keyword[] DescriberKeywords = { DescriberKeyword.Static, DescriberKeyword.Public, DescriberKeyword.Private, DescriberKeyword.Protected, DescriberKeyword.Abstract, DescriberKeyword.Virtual, DescriberKeyword.Override, DescriberKeyword.Sealed, DescriberKeyword.Const, DescriberKeyword.ReadOnly, DescriberKeyword.In, DescriberKeyword.Out };

        public static readonly Keyword[] Keywords = { And, Input, Throw, Import, Print, Create, Var, TypeKeyword.Object, Default, When, This, Get, Set, TypeKeyword.SByte, TypeKeyword.Byte, TypeKeyword.Short, TypeKeyword.UShort, TypeKeyword.Int, TypeKeyword.UInt, TypeKeyword.Long, TypeKeyword.Ulong, TypeKeyword.Float, TypeKeyword.Double, TypeKeyword.Decimal, TypeKeyword.Bool, TypeKeyword.String, TypeKeyword.Char, TypeKeyword.Array, EntityKeyword.Class, EntityKeyword.Struct, EntityKeyword.Interface, EntityKeyword.Namespace, EntityKeyword.Enum, ConditionKeyword.If, ConditionKeyword.Else, ConditionKeyword.Switch, ConditionKeyword.Case, LoopKeyword.For, LoopKeyword.While, LoopKeyword.Do, ControlKeyword.Return, ControlKeyword.Continue, ControlKeyword.Break
        ,DescriberKeyword.Static, DescriberKeyword.Public, DescriberKeyword.Private, DescriberKeyword.Protected, DescriberKeyword.Abstract, DescriberKeyword.Virtual, DescriberKeyword.Override, DescriberKeyword.Sealed, DescriberKeyword.Const, DescriberKeyword.ReadOnly, FunctionKeyword.Operator, FunctionKeyword.Explicit, FunctionKeyword.Implicit, FunctionKeyword.Void, FunctionKeyword.Constructor, DescriberKeyword.In, DescriberKeyword.Out };
    }
}
