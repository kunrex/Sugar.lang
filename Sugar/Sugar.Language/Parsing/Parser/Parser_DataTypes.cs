using System;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Keywords;
using Sugar.Language.Tokens.Keywords.Subtypes.Entities;
using Sugar.Language.Tokens.Operators.Binary;
using Sugar.Language.Tokens.Seperators;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Parsing.Nodes.DataTypes;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Values.Generics;

namespace Sugar.Language.Parsing.Parser
{
    internal sealed partial class Parser
    {
        private ParseNodeCollection ParseScopedEntity(DescriberNode describer, Keyword keyword)
        {
            if (keyword == EntityKeyword.Namespace)
                return ParseNamespaceDeclaration(describer);
            else
                return ParseTypeDeclaration(describer, keyword);
        }

        private ParseNodeCollection ParseNamespaceDeclaration(DescriberNode describer)
        {
            index++;
            var name = ParseVariable(SeperatorKind.FlowerOpenBracket);

            TryMatchCurrent(Seperator.FlowerOpenBracket);
            return new NamespaceNode(describer, name, ParseScope());
        }

        private ParseNodeCollection ParseTypeDeclaration(DescriberNode describer, Keyword keyword)
        {
            index++;
 
            switch(keyword.SyntaxKind)
            {
                case SyntaxKind.Enum:
                    return ParseEnumDeclaration(describer, ParseIdentifier(true));
                case SyntaxKind.Struct:
                    return ParseStructDeclaration(describer, ParseIdentifier(true));
                case SyntaxKind.Class:
                    return ParseClassDeclaration(describer, ParseIdentifier(true));
                default:
                    return ParseInterfaceDeclaration(describer, ParseIdentifier(true));
            }
        }

        private EnumNode ParseEnumDeclaration(DescriberNode describer, IdentifierNode name)
        {
            InheritanceNode inherits = null;
            if (MatchCurrent(Seperator.Colon))
                inherits = ParseInheritanceTypes(SeperatorKind.FlowerOpenBracket);

            if (inherits == null)
                return new EnumNode(describer, name, ParseExpressionList(SeperatorKind.FlowerCloseBracket));
            else
                return new EnumNode(describer, name, ParseExpressionList(SeperatorKind.FlowerCloseBracket), inherits);
        }

        private StructNode ParseStructDeclaration(DescriberNode describer, IdentifierNode name)
        {
            GenericDeclarationNode generic = null;
            if (MatchCurrent(BinaryOperator.LesserThan))
                generic = ParseGenericDeclaration();

            TryMatchCurrent(Seperator.FlowerOpenBracket);

            if (generic == null)
                return new StructNode(describer, name, ParseScope());
            else
                return new StructNode(describer, name, ParseScope(), generic);
        }

        private ClassNode ParseClassDeclaration(DescriberNode describer, IdentifierNode name)
        {
            GenericDeclarationNode generic = null;
            if (MatchCurrent(BinaryOperator.LesserThan))
                generic = ParseGenericDeclaration();

            InheritanceNode inherits = null;
            if (MatchCurrent(Seperator.Colon))
                inherits = ParseInheritanceTypes(SeperatorKind.FlowerOpenBracket);

            TryMatchCurrent(Seperator.FlowerOpenBracket);

            if (generic != null)
            {
                if (inherits == null)
                    return new ClassNode(describer, name, ParseScope(), generic);
                else
                    return new ClassNode(describer, name, ParseScope(), generic, inherits);
            }
            else if (inherits != null)
                return new ClassNode(describer, name, ParseScope(), inherits);
            else
                return new ClassNode(describer, name, ParseScope());
        }

        private InterfaceNode ParseInterfaceDeclaration(DescriberNode describer, IdentifierNode name)
        {
            GenericDeclarationNode generic = null;
            if (MatchCurrent(BinaryOperator.LesserThan))
                generic = ParseGenericDeclaration();

            InheritanceNode inherits = null;
            if (MatchCurrent(Seperator.Colon))
                inherits = ParseInheritanceTypes(SeperatorKind.FlowerOpenBracket);

            TryMatchCurrent(Seperator.FlowerOpenBracket);

            if (generic != null)
            {
                if (inherits == null)
                    return new InterfaceNode(describer, name, ParseScope(), generic);
                else
                    return new InterfaceNode(describer, name, ParseScope(), generic, inherits);
            }
            else if (inherits != null)
                return new InterfaceNode(describer, name, ParseScope(), inherits);
            else
                return new InterfaceNode(describer, name, ParseScope());
        }

    }
}