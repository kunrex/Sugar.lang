using System;

namespace Sugar.Language.Analysis.ProjectStructure.Enums
{
    internal enum LocalMemberEnum : ushort
    {
        Scope ,

        LocalVoid,
        LocalMethod,
        SingleFunctionCall,

        LocalVariable,
        LocalVariableAssignment,

        If,
        ElseIf,
        Else,

        Switch,

        For,
        While,
        DoWhile,
        Foreach,
    }
}
