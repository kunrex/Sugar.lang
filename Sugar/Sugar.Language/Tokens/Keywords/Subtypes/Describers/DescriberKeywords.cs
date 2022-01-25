using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords.Subtypes.Describers
{
    internal sealed partial class DescriberKeyword : Keyword
    {
        public static readonly DescriberKeyword Static = new DescriberKeyword("static", 0, DescriberType.Static);

        public static readonly DescriberKeyword Public = new DescriberKeyword("public", 1, DescriberType.AccessModifier);
        public static readonly DescriberKeyword Private = new DescriberKeyword("private", 2, DescriberType.AccessModifier);
        public static readonly DescriberKeyword Protected = new DescriberKeyword("protected", 3, DescriberType.AccessModifier);

        public static readonly DescriberKeyword Abstract = new DescriberKeyword("abstract", 4, DescriberType.Inhertiance);
        public static readonly DescriberKeyword Sealed = new DescriberKeyword("sealed", 5, DescriberType.Inhertiance);
        public static readonly DescriberKeyword Virtual = new DescriberKeyword("virtual", 6, DescriberType.Inhertiance);
        public static readonly DescriberKeyword Override = new DescriberKeyword("override", 7, DescriberType.Inhertiance);

        public static readonly DescriberKeyword Const = new DescriberKeyword("const", 8, DescriberType.Mutability);
        public static readonly DescriberKeyword ReadOnly = new DescriberKeyword("readonly", 9, DescriberType.Mutability);

        public static readonly DescriberKeyword In = new DescriberKeyword("in", 10, DescriberType.Function);
        public static readonly DescriberKeyword Out = new DescriberKeyword("out", 11, DescriberType.Function);
    }
}
