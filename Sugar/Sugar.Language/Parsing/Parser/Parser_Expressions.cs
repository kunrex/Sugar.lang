using System;
using System.Collections.Generic;

using Sugar.Language.Tokens;
using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Keywords;
using Sugar.Language.Tokens.Constants;
using Sugar.Language.Tokens.Operators;
using Sugar.Language.Tokens.Seperators;
using Sugar.Language.Tokens.Operators.Binary;
using Sugar.Language.Tokens.Operators.Assignment;
using Sugar.Language.Tokens.Keywords.Subtypes.Types;
using Sugar.Language.Tokens.Keywords.Subtypes.ControlStatements;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Exceptions.Parsing;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.DataTypes;

using Sugar.Language.Parsing.Nodes.NodeGroups;

using Sugar.Language.Parsing.Nodes.Statements;

using Sugar.Language.Parsing.Parser.Structure;

using Sugar.Language.Parsing.Nodes.CtrlStatements;

using Sugar.Language.Parsing.Nodes.Types;
using Sugar.Language.Parsing.Nodes.Types.Invalid;
using Sugar.Language.Parsing.Nodes.Types.Subtypes;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Values.Invalid;
using Sugar.Language.Parsing.Nodes.Values.Generics;
using Sugar.Language.Parsing.Nodes.Values.Generics.Invalid;

using Sugar.Language.Parsing.Nodes.Expressions.Invalid;
using Sugar.Language.Parsing.Nodes.Expressions.Operative;
using Sugar.Language.Parsing.Nodes.Expressions.Associative;

namespace Sugar.Language.Parsing.Parser
{ 
    internal sealed partial class Parser
    {
        /// <summary>
        /// Parse Expression (can be empty)
        /// <para>• Ends on same index as <paramref name="breakOutSeperators"/></para>
        /// • if <paramref name="readAssignment"/> is set to true, ends one index after =
        /// </summary>
        private ParseNode ParseExpression(bool readAssignment, SeperatorKind breakOutSeperators)
        {
            var stack = new List<Token>();
            var output = new List<ParseNodeCollection>();
            TokenType expected = TokenType.Constant | TokenType.Identifier | TokenType.Seperator | TokenType.UnaryOperator | TokenType.Keyword;

            bool breakOut = false, invalid = false;
            while (index < sourceFile.TokenCount)
            {
                var current = Current;

                if (!MatchType(expected, current))
                    invalid = breakOut = true;
                else
                {
                    switch (current.Type)
                    {
                        case TokenType.Keyword:
                            switch (current.SyntaxKind)
                            {
                                case SyntaxKind.Create:
                                    output.Add(ParseConstructorCall(true));
                                    break;
                                case SyntaxKind.This:
                                    index++;
                                    TryMatchCurrent(Seperator.Dot, true);
                                    output.Add(new ThisNode(ParseEntity(true, SeperatorKind.None)));
                                    break;
                                case SyntaxKind.Input:
                                    output.Add(ParseInputCall(true));
                                    break;
                                case SyntaxKind.Default:
                                    output.Add(ParseDefault());
                                    break;
                                case SyntaxKind.AsType:
                                    output.Add(ParseAsType());
                                    break;
                                default:
                                    if (((Keyword)current).KeywordType == KeywordType.Type)
                                    {
                                        output.Add(ParseEntity(true, SeperatorKind.None));
                                        break;
                                    }
                                    invalid = breakOut = true;
                                    break;
                            }

                            expected = TokenType.Seperator | TokenType.Operator;
                            break;
                        case TokenType.Constant:
                            output.Add(ParseEntity(true, SeperatorKind.None));
                            expected = TokenType.Seperator | TokenType.BinaryOperator;
                            break;
                        case TokenType.Identifier:
                            output.Add(ParseEntity(true, SeperatorKind.None));
                            expected = TokenType.Seperator | TokenType.Operator;
                            break;
                        case TokenType.Seperator:
                            if (MatchBreakOutSeperator(current, breakOutSeperators))
                            {
                                breakOut = true;
                                break;
                            }

                            if (MatchToken(Seperator.OpenBracket, current, true))
                            {
                                output.Add(ParseNonEmptyExpression(false, SeperatorKind.CloseBracket));
                                expected = TokenType.Operator | TokenType.Seperator;
                            }
                            else if (MatchToken(Seperator.Questionmark, current, true))
                            {
                                output.Add(ParseTernary(output, stack, breakOutSeperators));
                                expected = TokenType.Seperator | TokenType.Operator;
                            }
                            else
                                invalid = breakOut = true;
                            break;
                        case TokenType.UnaryOperator:
                            if (stack.Count > 0 && MatchType(TokenType.UnaryOperator, stack[stack.Count - 1], false))
                            {
                                invalid = breakOut = true;
                                break;
                            }

                            switch (sourceFile[index - 1].Type)
                            {
                                case TokenType.Identifier:
                                    AddPostfix();
                                    break;
                                case TokenType.BinaryOperator:
                                case TokenType.AssignmentOperator:
                                    AddPrefix();
                                    break;
                                case TokenType.Seperator:
                                    var prev = sourceFile[index - 1];
                                    if (prev == Seperator.CloseBracket || prev == Seperator.BoxCloseBracket)
                                        AddPostfix();
                                    else
                                        AddPrefix();
                                    break;
                            }

                            void AddPrefix()
                            {
                                stack.Add(Operator.ConvertUnaryToPrefix((Operator)current));
                                expected = TokenType.Identifier | TokenType.Constant;

                                if (LookAhead() == Seperator.OpenBracket)
                                    expected |= TokenType.Seperator;
                            }
                            void AddPostfix()
                            {
                                stack.Add((Operator)current);
                                expected = TokenType.BinaryOperator | TokenType.Seperator;
                            }
                            break;
                        case TokenType.AssignmentOperator:
                            if (!readAssignment)
                                invalid = true;

                            breakOut = true;
                            break;
                        case TokenType.BinaryOperator:
                            var asOperator = (Operator)current;

                            if (asOperator == BinaryOperator.GreaterThan && LookAhead() == BinaryOperator.GreaterThan)
                            {
                                asOperator = BinaryOperator.RightShift;
                                index++;
                            }

                            while (stack.Count > 0)
                            {
                                var top = stack[stack.Count - 1];
                                if (top == Seperator.OpenBracket)
                                    break;

                                var node = ParseExpression(asOperator, output, stack);
                                if (node == null)
                                    break;

                                output.Add(node);
                                stack.RemoveAt(stack.Count - 1);
                            }

                            if (asOperator == BinaryOperator.AsCastOperator)
                            {
                                stack.Add(asOperator);
                                expected = TokenType.Identifier | TokenType.Keyword;
                                break;
                            }
                            else
                                stack.Add(asOperator);

                            expected = TokenType.Identifier | TokenType.Constant | TokenType.UnaryOperator | TokenType.Seperator;
                            break;
                    }
                }

                if (breakOut)
                {
                    ClearStack(output, stack);

                    if (invalid)
                    {
                        if ((expected & TokenType.Seperator) == TokenType.Seperator)
                            output.Add(new InvalidExpressionNode(PushExpectedTokenException($"{breakOutSeperators}", current), PanicModeSimple(breakOutSeperators)));
                        else
                            output.Add(new InvalidExpressionNode(PushInvalidCurrentException(), PanicModeSimple(breakOutSeperators)));
                    }

                    break;
                }

                index++;
            }

            if (breakOut && output.Count == 0)
                return new EmptyNode();
            else if (index >= sourceFile.TokenCount)
                PushExpectedCurrentException($"{breakOutSeperators}");

            return output[0];
        }

        private ParseNodeCollection ParseExpression(Operator top, List<ParseNodeCollection> output)
        {
            if (top.Type == TokenType.UnaryOperator)
            {
                var operhand = output[output.Count - 1];

                output.RemoveAt(output.Count - 1);
                return new UnaryExpression(top, operhand);
            }
            else
            {
                var lhs = output[output.Count - 2];
                var rhs = output[output.Count - 1];

                output.RemoveRange(output.Count - 2, 2);
                if (top == BinaryOperator.DotOperator)
                    return new DotExpression(lhs, rhs);
                else if (top == BinaryOperator.AsCastOperator)
                    return new CastExpression(lhs, (TypeNode)rhs);
                else
                    return new BinaryExpression(top, lhs, rhs);
            }
        }

        private ParseNodeCollection ParseExpression(Operator current, List<ParseNodeCollection> output, List<Token> stack)
        {
            var top = (Operator)stack[stack.Count - 1];
            if (current.Precedence < top.Precedence && !(top.Precedence == current.Precedence && current.LeftAssociative))
                return null;

            return ParseExpression(top, output);
        }

        /// <summary>
        /// Parse Expression (non empty)
        /// <para>• Ends on same index as <paramref name="breakOutSeperators"/></para>
        /// • if <paramref name="readAssignment"/> is set to true, ends one index after =
        /// </summary>
        private ParseNodeCollection ParseNonEmptyExpression(bool readAssignment, SeperatorKind breakOutSeperators)
        {
            var expression = ParseExpression(readAssignment, breakOutSeperators);

            if (expression.NodeType == ParseNodeType.Empty)
                return new EmptyExpressionNode(PushInvalidCurrentException());
            else
                return (ParseNodeCollection)expression;
        }

        private void ClearStack(List<ParseNodeCollection> output, List<Token> stack)
        {
            while (stack.Count > 0)
                output.Add(ParseExpression((Operator)stack.Pop(), output));
        }

        /// <summary>
        /// Parse Condition (non empty)
        /// <para>• Starts at opening (</para>
        /// • Ends one index after closing )
        /// </summary>
        private ParseNodeCollection ParseCondition()
        {
            TryMatchCurrent(Seperator.OpenBracket, true);
            var condition = ParseNonEmptyExpression(false, SeperatorKind.CloseBracket);

            TryMatchCurrent(Seperator.CloseBracket, true);
            return condition;
        }

        /// <summary>
        /// Parse Ternary (non empty)
        /// <para>• Ends on same index as <paramref name="breakOutSeperators"/></para>
        /// </summary>
        private TernaryExpression ParseTernary(List<ParseNodeCollection> output, List<Token> stack, SeperatorKind breakOutSeperators)
        {
            ParseNodeCollection condition;
            if (output.Count == 0)
                condition = new EmptyExpressionNode(PushInvalidCurrentException());
            else
            {
                ClearStack(output, stack);

                condition = output.Pop();
            }

            var trueExpression = ParseNonEmptyExpression(false, SeperatorKind.Colon);
            index++;
            var falseExpression = ParseNonEmptyExpression(false, SeperatorKind.Semicolon | breakOutSeperators);

            index--;
            return new TernaryExpression(condition, trueExpression, falseExpression);
        }

        /// <summary>
        /// Parse Indexer (non empty)
        /// <para>• Force starts on opening [ </para>
        /// • Ends on same index as closing ]
        /// </summary>
        private IndexerExpression ParseIndexer(ParseNodeCollection operhand)
        {
            ForceMatchCurrent(Seperator.BoxOpenBracket, true);

            var arguments = new ExpressionListNode();
            while (index < sourceFile.TokenCount)
            {
                arguments.AddChild(ParseNonEmptyExpression(false, SeperatorKind.BoxCloseBracket | SeperatorKind.Comma));

                if (MatchCurrent(Seperator.Comma, true))
                    continue;
                else if (MatchCurrent(Seperator.BoxCloseBracket))
                    return new IndexerExpression(operhand, arguments);
                else
                    break;
            }

            return new InvalidIndexerNode(PushExpectedCurrentException(Seperator.BoxCloseBracket), PanicModeSimple(SeperatorKind.BoxCloseBracket));
        }

        /// <summary>
        /// <para>• Force starts on default </para>
        /// • Ends on index of closing ;
        /// </summary>
        private ReturnKeyword ParseReturn()
        {
            ForceMatchCurrent(ControlKeyword.Return, true);

            if (MatchCurrent(Seperator.Semicolon))
                return new ReturnKeyword();
            else
                return new ReturnKeyword(ParseNonEmptyExpression(false, SeperatorKind.Semicolon));
        }

        /// <summary>
        /// <para>• Force starts on default </para>
        /// • Ends on index of closing )
        /// </summary>
        private DefaultValueNode ParseDefault()
        {
            ForceMatchCurrent(Keyword.Default, false);

            if (MatchToken(Seperator.OpenBracket, LookAhead()))
            {
                index += 2;
                return new DefaultValueNode(ParseType(SeperatorKind.CloseBracket));
            }

            return new DefaultValueNode();
        }

        /// <summary>
        /// <para>• Force starts on AsType </para>
        /// • Ends on index of closing )
        /// </summary>
        private AsTypeNode ParseAsType()
        {
            ForceMatchCurrent(Keyword.AsType, true);
            TryMatchCurrent(Seperator.OpenBracket, true);

            return new AsTypeNode(ParseType(SeperatorKind.CloseBracket));
        }

        /// <summary>
        /// Parse Expression List 
        /// <para>• Starts on opening seperator </para>
        /// • Ends on index of closing <paramref name="finalBreakOut"/> if <paramref name="increment"/> is false
        /// </summary>
        private ExpressionListNode ParseExpressionList(SeperatorKind finalBreakOut, bool increment = false)
        {
            index++;
            var nodes = new ExpressionListNode();

            if (MatchBreakOutSeperator(Current, finalBreakOut))
            {
                if (increment)
                    index++;

                return nodes;
            }

            while (index < sourceFile.TokenCount)
            {
                var expression = ParseNonEmptyExpression(true, SeperatorKind.Comma | finalBreakOut);
                if (MatchCurrent(AssignmentOperator.Assignment, true))
                    expression = new AssignmentNode(expression, ParseNonEmptyExpression(false, SeperatorKind.Comma | finalBreakOut));

                nodes.AddChild(expression);

                if (MatchCurrent(Seperator.Comma, true))
                    continue;
                else if (MatchBreakOutSeperator(Current, finalBreakOut))
                {
                    if (increment)
                        index++;

                    break;
                }
                else
                {
                    nodes.AddChild(new InvalidEntityNode(PushExpectedCurrentException($"{finalBreakOut}"), PanicModeSimple(finalBreakOut)));
                    break;
                }
            }

            return nodes;
        }

        /// <summary>
        /// Parse Expression List 
        /// <para>• Ends on index of last valid entity or <paramref name="breakOutSeperators"/></para>
        /// </summary>
        private ParseNodeCollection ParseEntity(bool inExpression, SeperatorKind breakOutSeperators)
        {
            var stack = new List<Token>();
            var output = new List<ParseNodeCollection>();
            TokenType expected = TokenType.Constant | TokenType.Identifier | TokenType.Keyword;

            bool breakOut = false, subtract = false;
            while (index < sourceFile.TokenCount)
            {
                var current = Current;

                switch (current.Type)
                {
                    case TokenType.Constant:
                        if (!MatchType(expected, current))
                        {
                            breakOut = subtract = true;
                            break;
                        }

                        output.Add(new ConstantValueNode((Constant)current));
                        expected = TokenType.Seperator;
                        break;
                    case TokenType.Identifier:
                        if (!MatchType(expected, current))
                        {
                            breakOut = subtract = true;
                            break;
                        }

                        IdentifierNode identifier = ParseIdentifier();
                        if (MatchToken(BinaryOperator.LesserThan, LookAhead()))
                        {
                            GenericCallNode generic = ParseGenericCall(!inExpression);

                            if (generic == null)
                                output.Add(identifier);
                            else
                                output.Add(new CreatedTypeNode(identifier, generic));
                        }
                        else
                        {
                            output.Add(identifier);
                        }


                        expected = TokenType.Seperator;
                        break;
                    case TokenType.Seperator:
                        if (!MatchType(expected, current))
                        {
                            breakOut = subtract = true;
                            break;
                        }
                        else if (MatchBreakOutSeperator(current, breakOutSeperators))
                        {
                            breakOut = true;
                            break;
                        }

                        switch (current.SyntaxKind)
                        {
                            case SyntaxKind.Dot:
                                stack.Add(BinaryOperator.DotOperator);
                                expected = TokenType.Identifier;
                                break;
                            case SyntaxKind.BoxOpenBracket:
                                output.Add(ParseIndexer(output.Pop()));
                                break;
                            case SyntaxKind.OpenBracket:
                                output.Add(ParseFunctionCall(inExpression, ParseVariable(output.Pop())));
                                break;
                            default:
                                breakOut = subtract = true;
                                break;
                        }
                        break;
                    case TokenType.Keyword:
                        if (!MatchType(expected, current))
                        {
                            breakOut = subtract = true;
                            break;
                        }
                        else if (current.SyntaxKind == SyntaxKind.Array)
                        {
                            if (inExpression)
                                output.Add(new TypeKeywordNode(TypeKeyword.Array));
                            else
                                output.Add(new ArrayTypeNode(ParseGenericCall(true)));
                        }

                        var keyword = (Keyword)current;

                        if (keyword.KeywordType == KeywordType.Type)
                        {
                            output.Add(new TypeKeywordNode(keyword));
                            expected = TokenType.Seperator;
                            break;
                        }

                        breakOut = subtract = true;
                        break;
                    default:
                        breakOut = subtract = true;
                        break;
                }

                if (breakOut)
                {
                    if (subtract)
                        index--;

                    ClearStack(output, stack);
                    break;
                }

                index++;
            }

            if (output.Count > 0)
                return output[0];
            else
                return new EmptyEntityNode(PushExpectedTokenException(expected, Current), expected);
        }

        /// <summary>
        /// Parse Identifier
        /// <para>• Returns one index after identifer if <paramref name="increment"/> is true</para>
        /// </summary>
        private IdentifierNode ParseIdentifier(bool increment = false)
        {
            if (TryMatchCurrentType(TokenType.Identifier))
            {
                var node = new IdentifierNode((Identifier)Current);

                if (increment)
                    index++;
                return node;
            }
            else
                return new InvalidIdentifierNode(Current, PushInvalidCurrentException());
        }

        /// <summary>
        /// Force Parse a Variable
        /// </summary>
        private ParseNodeCollection ParseVariable(ParseNodeCollection entity)
        {
            if (CheckVariable(entity))
            {
                if (entity.NodeType == ParseNodeType.Dot)
                    return new LongIdentiferNode((DotExpression)entity);
                else
                    return entity;
            }
            else
                return new InvalidVariableNode(PushExpectedCurrentException(TokenType.Identifier), entity);
        }

        /// <summary>
        /// Force Parse a Variable
        /// <para>• Ends on index of closing <paramref name="breakOutSeperators"/> </para>
        /// </summary>
        private ParseNodeCollection ParseVariable(SeperatorKind breakOutSeperators) => ParseVariable(ParseEntity(false, breakOutSeperators));

        private bool CheckVariable(ParseNodeCollection entity)
        {
            var node = entity;
            while (true)
            {
                if (node.NodeType == ParseNodeType.Dot)
                    node = ((DotExpression)node).RHS;
                else if (node.NodeType == ParseNodeType.Identifier)
                    break;
                else
                    return false;
            }

            return node.NodeType == ParseNodeType.Identifier;
        }

        /// <summary>
        /// Force Parse a Type
        /// <para>• Ends on index of closing <paramref name="breakOutSeperators"/> </para>
        /// </summary>
        private TypeNode ParseType(SeperatorKind breakOutSeperators, bool force = true)
        {
            if (MatchCurrent(Keyword.Var, true))
            {
                TryMatchBreakOutSeperator(breakOutSeperators);
                return new AnonymousTypeNode();
            }

            return ValidateType(Current, ParseEntity(false, breakOutSeperators), force);
        }

        private TypeNode ValidateType(Token token, ParseNodeCollection entity, bool force)
        {
            var node = entity;

            while (true)
                if (node.NodeType == ParseNodeType.Dot)
                {
                    var dot = (DotExpression)node;
                    var lhs = dot.LHS.NodeType;

                    if (lhs != ParseNodeType.Type || lhs != ParseNodeType.Identifier)
                        break;

                    node = dot.RHS;
                }
                else
                {
                    switch (node.NodeType)
                    {
                        case ParseNodeType.Type:
                            return (TypeNode)entity;
                        case ParseNodeType.Identifier:
                            return new CreatedTypeNode(entity);
                    }

                    break;
                }

            var exception = new TokenExpectedException("Type", token);
            if (force)
                sourceFile.PushException(exception);

            return new InvalidTypeNode(exception, entity);
        }

        /// <summary>
        /// Parse list of Types
        /// <para>• Force starts at beginning : </para>
        /// • Ends one index after final valid Type parsed
        /// </summary>
        private InheritanceNode ParseInheritanceTypes(SeperatorKind breakoutSeperators)
        {
            var inheritance = new InheritanceNode();
            ForceMatchCurrent(Seperator.Colon, true);

            bool breakOut;
            while (index < sourceFile.TokenCount)
            {
                breakOut = false;
                var current = Current;

              
                switch (current.Type)
                {
                    case TokenType.Keyword:
                    case TokenType.Identifier:
                        inheritance.AddChild(ParseType(SeperatorKind.Comma | breakoutSeperators));

                        if (MatchBreakOutSeperator(Current, breakoutSeperators))
                            breakOut = true;
                        else
                            index++;
                        break;
                    case TokenType.Seperator:
                        if (MatchToken(current, Seperator.Comma, true))
                            continue;

                        breakOut = true;
                        break;
                    default:
                        breakOut = true;
                        break;
                }

                if (breakOut)
                    break;
            }

            return inheritance;
        }

        private ParseGenericResult ParseGenericCall()
        {
            index++;
            ForceMatchCurrent(BinaryOperator.LesserThan, true);

            var generic = new GenericCallNode();
            TokenType expected = TokenType.Identifier | TokenType.Keyword;

            while (index < sourceFile.TokenCount)
            {
                var current = Current;

                switch (current.Type)
                {
                    case TokenType.BinaryOperator:
                        if (!MatchType(expected, current))
                            return new ParseGenericResult(new TokenExpectedException(expected, 0));

                        if(current.SyntaxKind == SyntaxKind.GreaterThan)
                            return new ParseGenericResult(generic);

                        return new ParseGenericResult(new InvalidTokenException(current));
                    case TokenType.Keyword:
                        if (!MatchType(expected, current))
                        { }
                        else if ((current as Keyword).KeywordType == KeywordType.Type)
                        {
                            TypeNode type = ParseType(SeperatorKind.Comma, false);

                            if (type.NodeType != ParseNodeType.Invalid)
                            {
                                generic.AddChild(type);
                                expected = TokenType.Seperator | TokenType.Operator;
                                break;
                            }
                        }

                        return new ParseGenericResult(new InvalidTokenException(current));
                    case TokenType.Identifier:
                        if (!MatchType(expected, current))
                            return new ParseGenericResult(new InvalidTokenException(current));
                        else
                        {
                            TypeNode type = ParseType(SeperatorKind.Comma, false);

                            if (type.NodeType == ParseNodeType.Invalid)
                                return new ParseGenericResult(new InvalidTokenException(current));

                            generic.AddChild(type);
                            if (MatchCurrent(Seperator.Comma, true))
                                continue;
                            else
                                expected = TokenType.BinaryOperator;
                        }
                        break;
                    case TokenType.Seperator:
                        if (!MatchType(expected, current) || !MatchToken(Seperator.Comma, current))
                            return new ParseGenericResult(new InvalidTokenException(current));
                        else
                            expected = TokenType.Identifier | TokenType.Keyword;
                        break;
                    default:
                        break;
                }

                index++;
            }

            return new ParseGenericResult(new TokenExpectedException(BinaryOperator.GreaterThan, Current));
        }

        /// <summary>
        /// Parse Generic Call
        /// <para>• Force starts one index before the beginning LESSERTHAN </para>
        /// • Ends on same index as closing GREATERTHAN, if found
        /// </summary>
        private GenericCallNode ParseGenericCall(bool force)
        {
            int beforeIndex = index;

            var genericResult = ParseGenericCall();
 
            if (genericResult.Success)
            {
                var next = LookAhead();
                var toMatch = TokenType.Seperator | TokenType.BinaryOperator;
                if (force)
                    toMatch |= TokenType.Identifier;

                if (MatchType(toMatch, next) && next != Seperator.OpenBracket)
                    return genericResult.Generic;
                else
                {
                    index = beforeIndex;
                    if (force)
                    {
                        sourceFile.PushException(genericResult.Exception);
                        return new InvalidGenericCallNode(genericResult.Exception, PanicModeSimple(SeperatorKind.None));
                    }
                    else
                        return null;
                }
            }
            else
            {
                index = beforeIndex;

                if (force)
                {
                    sourceFile.PushException(genericResult.Exception);
                    return new InvalidGenericCallNode(genericResult.Exception, PanicModeSimple(SeperatorKind.None));
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Parse Generic Declaration
        /// <para>• Force starts at beginning LESSERTHAN </para>
        /// • Ends one index after closing GREATERTHAN
        /// </summary>
        private GenericDeclarationNode ParseGenericDeclaration()
        {
            var generic = new GenericDeclarationNode();
            ForceMatchCurrent(BinaryOperator.LesserThan, true);

            while (index < sourceFile.TokenCount)
            {
                IdentifierNode variable = ParseIdentifier(true);

                if (MatchCurrent(Seperator.Colon))
                    generic.AddChild(new GenericVariableNode(variable, ParseInheritanceTypes(SeperatorKind.Semicolon)));
                else
                    generic.AddChild(new GenericVariableNode(variable));

                if (MatchCurrent(Seperator.Semicolon, true))
                    continue;
                else if (MatchCurrent(BinaryOperator.GreaterThan, true))
                    return generic;
                else
                    break;
            }

            return new InvalidGenericDeclarationNode(PushExpectedCurrentException(BinaryOperator.GreaterThan), PanicModeSimple(SeperatorKind.None));
        }
    }
}

