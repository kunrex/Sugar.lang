using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords.Subtypes.Describers
{
    internal sealed partial class DescriberKeyword : Keyword
    {
        public static readonly DescriberKeyword Static = new DescriberKeyword("static", SyntaxKind.Static, DescriberType.Static);

        public static readonly DescriberKeyword Public = new DescriberKeyword("public", SyntaxKind.Public, DescriberType.AccessModifier);
        public static readonly DescriberKeyword Private = new DescriberKeyword("private", SyntaxKind.Private, DescriberType.AccessModifier);
        public static readonly DescriberKeyword Protected = new DescriberKeyword("protected", SyntaxKind.Protected, DescriberType.AccessModifier);

        public static readonly DescriberKeyword Abstract = new DescriberKeyword("abstract", SyntaxKind.Abstract, DescriberType.Inhertiance);
        public static readonly DescriberKeyword Sealed = new DescriberKeyword("sealed", SyntaxKind.Sealed, DescriberType.Inhertiance);
        public static readonly DescriberKeyword Virtual = new DescriberKeyword("virtual", SyntaxKind.Virtual, DescriberType.Inhertiance);
        public static readonly DescriberKeyword Override = new DescriberKeyword("override", SyntaxKind.Override, DescriberType.Inhertiance);

        public static readonly DescriberKeyword Const = new DescriberKeyword("const", SyntaxKind.Const, DescriberType.Mutability);
        public static readonly DescriberKeyword ReadOnly = new DescriberKeyword("readonly", SyntaxKind.Readonly, DescriberType.Mutability);

        public static readonly DescriberKeyword In = new DescriberKeyword("in", SyntaxKind.In, DescriberType.Function);
        public static readonly DescriberKeyword Out = new DescriberKeyword("out", SyntaxKind.Out, DescriberType.Function);
        public static readonly DescriberKeyword Ref = new DescriberKeyword("ref", SyntaxKind.Ref, DescriberType.Function);
    }
}
