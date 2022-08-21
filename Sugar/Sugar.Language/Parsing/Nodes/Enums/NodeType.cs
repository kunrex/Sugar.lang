using System;

namespace Sugar.Language.Parsing.Nodes.Enums
{
    internal enum NodeType : byte
    {
        Scope,
        Compound,
        SugarFile,
        ExpressionList,

        Variable,
        Constant,

        This,
        Empty,
        Default,
        Generic,

        Type,

        Assignment,
        Declaration,
        Initialise,
        CompoundDeclaration,

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
        ConstructorDeclaration,

        Lambda,

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

        Parent,

        Action,
        Function
    }
}
