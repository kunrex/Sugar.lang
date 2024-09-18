using System;

namespace Sugar.Language.Parsing.Nodes.Enums
{
    internal enum ParseNodeType : byte
    {
        Scope,
        Compound,
        SugarFile,
        ExpressionList,

        Identifier,
        Constant,
        LongIdentifier,

        This,
        Empty,
        Default,
        GenericCall,
        GenericDeclarataion,

        Type,

        Assignment,
        Declaration,
        Initialise,
        CompoundDeclaration,

        PropertyDeclaration,
        PropertyInitialise,

        For,
        While,
        DoWhile,
        Foreach,

        Get,
        Set,
        PropertyGet,
        PropertySet,
        PropertyGetSet,

        FunctionCall,
        ArgumentCall,
        ConstructorCall,
        FunctionDeclaration,
        ArgumentDeclaration,
        ExtensionDeclaration,
        ConstructorDeclaration,

        LambdaStatement,
        LambdaExpression,

        BuiltInFunction,

        Dot,
        Cast,
        Indexer,

        Unary,
        Binary,
        Ternary,

        Enum,
        Class,
        Struct,
        Interface,
        Namespace,
        Inheritance,

        Break,
        Return,
        Continue,

        If,
        Else,
        Case,
        When,
        Switch,
        IfElseChain,

        Import,
        Describer,
        ThrowException,

        Try,
        Catch,
        Finally,
        TryCatchFinally,

        AsType,

        OperatorOverload,
        ImplicitDeclaration,
        ExplicitDeclaration,

        Input,
        Print,

        Delegate,

        Invalid,
    }
}
