using System;

namespace Sugar.Language.Analysis.ProjectStructure.Enums
{
    [Flags]
    public enum DescriberEnum : ushort
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

        None = 8192,

        ReferenceModifiers = In | Out | Ref,
        EnumModifiers = Static | Public | Readonly,
        MutabilityModifier = Const | Readonly | In,
        AccessModifiers = Public | Private | Protected,
        InheritanceModifiers = Sealed | Virtual | Abstract,

        VariableBaseDescribers = AccessModifiers | Static | Readonly,

        ConstructorBaseDescriber = Static | AccessModifiers,

        CastBaseDescriber = Static | Override | AccessModifiers,
        GlobalFunctionBaseDescriber = CastBaseDescriber | InheritanceModifiers
    }
}
