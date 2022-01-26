using System;

namespace Sugar.Language.Parsing.Nodes.Enums
{
    internal enum NodeType : byte
    {
        Group,

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

        For,
        While,
        DoWhile,
        Foreach,

        Get,
        Set,
        Property,

        FunctionCall,
        ArgumentCall,
        ConstructorCall,
        FunctionDeclaration,
        ArgumentDeclaration,

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
        ThrowException
    }
}
