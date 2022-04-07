using System;

namespace Sugar.Language.Semantics.ActionTrees.Enums
{
    [Flags]
    internal enum DescriberEnum : ushort
    {
        Static = 1,

        Public = 2,
        Private = 4,
        Protected = 8,

        Sealed = 16,
        Virtual = 32,
        Abstract = 64,

        Override = 128,

        Const = 256,
        Readonly = 512,

        In = 1024,
        Out = 2048,
        Ref = 4096,

        ReferenceModifiers = In | Out | Ref,
        MutabilityModifier = Const | Readonly,
        AccessModifiers = Public | Private | Protected,
        InheritanceModifiers = Sealed | Virtual | Abstract,
    }
}
