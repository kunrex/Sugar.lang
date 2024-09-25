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
        /// </summary>
        private ParseNode ParseExpression(SeperatorKind breakOutSeperators)
        {
            var stack = new Stack<Operator>();
            var output = new Stack<ParseNodeCollection>();
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
                                    output.Push(ParseConstructorCall(true));
                                    break;
                                case SyntaxKind.This:
                                    index++;
                                    TryMatchCurrent(Seperator.Dot, true);
                                    output.Push(new ThisNode(ParseEntity(true, SeperatorKind.None)));
                                    break;
                                case SyntaxKind.Input:
                                    output.Push(ParseInputCall(true));
                                    break;
                                case SyntaxKind.Default:
                                    output.Push(ParseDefault());
                                    break;
                                case SyntaxKind.AsType:
                                    output.Push(ParseAsType());
                                    break;
                                default:
                                    if (((Keyword)current).KeywordType == KeywordType.Type)
                                    {
                                        output.Push(ParseType(SeperatorKind.None, true));
                                        break;
                                    }
                                    invalid = breakOut = true;
                                    break;
                            }

                            expected = TokenType.Seperator | TokenType.Operator;
                            break;
                        case TokenType.Constant:
                            output.Push(ParseEntity(true, SeperatorKind.None));
                            expected = TokenType.Seperator | TokenType.BinaryOperator;
                            break;
                        case TokenType.Identifier:
                            output.Push(ParseEntity(true, SeperatorKind.None));
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
                                output.Push(ParseNonEmptyExpression(SeperatorKind.CloseBracket));
                                expected = TokenType.Operator | TokenType.Seperator;
                            }
                            else if (MatchToken(Seperator.Questionmark, current, true))
                            {
                                output.Push(ParseTernary(output, stack, breakOutSeperators));
                                expected = TokenType.Seperator | TokenType.Operator;
                            }
                            else
                                invalid = breakOut = true;
                            break;
                        case TokenType.UnaryOperator:
                            if (stack.Count > 0 && MatchType(TokenType.UnaryOperator, stack.Peek(), false))
                            {
                                invalid = breakOut = true;
                                break;
                            }

                            switch (sourceFile[index - 1].Type)
                            {
                                case TokenType.Identifier:
                                    PushPostfix();
                                    break;
                                case TokenType.BinaryOperator:
                                case TokenType.AssignmentOperator:
                                    PushPrefix();
                                    break;
                                case TokenType.Seperator:
                                    var prev = sourceFile[index - 1];
                                    if (prev == Seperator.CloseBracket || prev == Seperator.BoxCloseBracket)
                                        PushPostfix();
                                    else
                                        PushPrefix();
                                    break;
                            }
                            
                            void PushPrefix()
                            {
                                stack.Push(Operator.ConvertUnaryToPrefix((Operator)current));
                                expected = TokenType.Identifier | TokenType.Constant;

                                if (LookAhead() == Seperator.OpenBracket)
                                    expected |= TokenType.Seperator;
                            }
                            void PushPostfix()
                            {
                                stack.Push((Operator)current);
                                expected = TokenType.BinaryOperator | TokenType.Seperator;
                            }
                            break;
                        case TokenType.AssignmentOperator:
                            stack.Push((Operator)current);
                            expected = TokenType.Identifier | TokenType.Constant | TokenType.UnaryOperator | TokenType.Seperator;
                            break;
                        case TokenType.BinaryOperator:
                            var asOperator = (Operator)current;

                            if (asOperator == BinaryOperator.GreaterThan && LookAhead() == BinaryOperator.GreaterThan)
                            {
                                asOperator = BinaryOperator.RightShift.Clone(asOperator.Index) as Operator;
                                index++;
                            }

                            while (stack.Count > 0)
                            {
                                var node = ParseExpression(asOperator, output, stack);
                                if (node == null)
                                    break;

                                output.Push(node);
                                stack.Pop();
                            }

                            stack.Push(asOperator);
                            expected = TokenType.Identifier | TokenType.Keyword | TokenType.Constant | TokenType.UnaryOperator;
                            if (LookAhead() == Seperator.OpenBracket)
                                expected |= TokenType.Seperator;
                            break;
                    }
                }

                if (breakOut)
                {
                    if (invalid)
                    {
                        if ((expected & TokenType.Seperator) == TokenType.Seperator)
                            output.Push(new InvalidExpressionNode(PushExpectedTokenException($"{breakOutSeperators}", current), PanicModeSimple(breakOutSeperators)));
                        else
                            output.Push(new InvalidExpressionNode(PushInvalidCurrentException(), PanicModeSimple(breakOutSeperators)));
                    }
                    
                    ClearStack(output, stack);
                    break;
                }

                index++;
            }

            if (breakOut && output.Count == 0)
                return new EmptyNode();
            if (index >= sourceFile.TokenCount)
                PushExpectedCurrentException($"{breakOutSeperators}");
            
            return output.Pop();
        }

        private ParseNodeCollection ParseExpression(Operator top, Stack<ParseNodeCollection> output)
        {
            if (top.Type == TokenType.UnaryOperator)
            {
                return new UnaryExpression(top, output.Pop());
            }
            
            var rhs = output.Pop();
            var lhs = output.Pop();
            
            if (top.Type == TokenType.AssignmentOperator)
            {
                var assignment = (AssignmentOperator)top;
                if (assignment.BaseOperator == null)
                    return new AssignmentNode(lhs, rhs);
                
                return new AssignmentNode(lhs, new BinaryExpression(assignment.BaseOperator, lhs, rhs));
            }
            if (top == BinaryOperator.DotOperator)
                return new DotExpression(lhs, rhs); 
            if (top == BinaryOperator.AsCastOperator)
                return new AssignmentNode(lhs, rhs);
            
            return new BinaryExpression(top, lhs, rhs);
        }

        private ParseNodeCollection ParseExpression(Operator current, Stack<ParseNodeCollection> output, Stack<Operator> stack)
        {
            var top = stack.Peek();
            if (current.Precedence < top.Precedence || (top.Precedence == current.Precedence && !current.LeftAssociative))
                return null;

            return ParseExpression(top, output);
        }

        /// <summary>
        /// Parse Expression (non-empty)
        /// <para>• Ends on same index as <paramref name="breakOutSeperators"/></para>
        /// </summary>
        private ParseNodeCollection ParseNonEmptyExpression(SeperatorKind breakOutSeperators)
        {
            var expression = ParseExpression(breakOutSeperators);

            if (expression.NodeType == ParseNodeType.Empty)
                return new EmptyExpressionNode(PushInvalidCurrentException());
            
            return (ParseNodeCollection)expression;
        }

        private void ClearStack(Stack<ParseNodeCollection> output, Stack<Operator> stack)
        {
            while (stack.Count > 0)
                output.Push(ParseExpression(stack.Pop(), output));
        }

        /// <summary>
        /// Parse Condition (non empty)
        /// <para>• Starts at opening (</para>
        /// • Ends one index after closing )
        /// </summary>
        private ParseNodeCollection ParseCondition()
        {
            TryMatchCurrent(Seperator.OpenBracket, true);
            var condition = ParseNonEmptyExpression(SeperatorKind.CloseBracket);

            TryMatchCurrent(Seperator.CloseBracket, true);
            return condition;
        }

        /// <summary>
        /// Parse Ternary (non empty)
        /// <para>• Ends on same index as <paramref name="breakOutSeperators"/></para>
        /// </summary>
        private TernaryExpression ParseTernary(Stack<ParseNodeCollection> output, Stack<Operator> stack, SeperatorKind breakOutSeperators)
        {
            ParseNodeCollection condition;
            if (output.Count == 0)
                condition = new EmptyExpressionNode(PushInvalidCurrentException());
            else
            {
                ClearStack(output, stack);

                condition = output.Pop();
            }

            var trueExpression = ParseNonEmptyExpression(SeperatorKind.Colon);
            index++;
            var falseExpression = ParseNonEmptyExpression(SeperatorKind.Semicolon | breakOutSeperators);

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
            ForceMatchCurrent(Seperator.BoxOpenBracket);
            return new IndexerExpression(operhand, ParseExpressionList(SeperatorKind.BoxCloseBracket));
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

            return new ReturnKeyword(ParseNonEmptyExpression(SeperatorKind.Semicolon));
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
        private ExpressionListNode ParseExpressionList(SeperatorKind finalBreakOut, bool readEmpty = false, bool increment = false)
        {
            index++;
            var nodes = new ExpressionListNode();

            if (MatchBreakOutSeperator(Current, finalBreakOut, increment))
            {
                if(!readEmpty)
                    nodes.AddChild(new EmptyEntityNode(new TokenExpectedException(TokenType.Identifier | TokenType.Constant, index), TokenType.Identifier | TokenType.Constant));
                
                return nodes;
            }

            while (index < sourceFile.TokenCount)
            {
                var expression = ParseNonEmptyExpression(SeperatorKind.Comma | finalBreakOut);
                nodes.AddChild(expression);

                if (MatchCurrent(Seperator.Comma, true))
                    continue;
                if (MatchBreakOutSeperator(Current, finalBreakOut, increment))
                    break;
                
                nodes.AddChild(new InvalidEntityNode(PushExpectedCurrentException($"{finalBreakOut}"), PanicModeSimple(finalBreakOut)));
                break;
            }

            return nodes;
        }
        
        /// <summary>
        /// Parse Entity
        /// <para>• Ends on index of last valid entity or <paramref name="breakOutSeperators"/></para>
        /// </summary>
        private ParseNodeCollection ParseEntity(bool inExpression, SeperatorKind breakOutSeperators, bool parseGeneric = true)
        {
            var stack = new Stack<Operator>();
            var output = new Stack<ParseNodeCollection>();
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

                        output.Push(new ConstantValueNode((Constant)current));
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
                                output.Push(identifier);
                            else
                                output.Push(new CreatedTypeNode(identifier, generic));
                        }
                        else
                            output.Push(identifier);
                        
                        expected = TokenType.Seperator;
                        break;
                    case TokenType.Seperator:
                        if (!MatchType(expected, current))
                        {
                            breakOut = subtract = true;
                            break;
                        }
                        if (MatchBreakOutSeperator(current, breakOutSeperators))
                        {
                            breakOut = true;
                            break;
                        }

                        switch (current.SyntaxKind)
                        {
                            case SyntaxKind.Dot:
                                stack.Push(BinaryOperator.DotOperator);
                                expected = TokenType.Identifier;
                                break;
                            case SyntaxKind.BoxOpenBracket:
                                output.Push(ParseIndexer(output.Pop()));
                                break;
                            case SyntaxKind.OpenBracket:
                                output.Push(ParseFunctionCall(inExpression, ParseVariable(output.Pop())));
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
                        
                        if (current.SyntaxKind == SyntaxKind.Array)
                            output.Push(new ArrayTypeNode(ParseGenericCall(true)));
                        else
                        {
                            var keyword = (Keyword)current;

                            if (keyword.KeywordType == KeywordType.Type)
                            {
                                output.Push(new TypeKeywordNode(keyword));
                                expected = TokenType.Seperator;
                                break;
                            }

                            breakOut = subtract = true;
                        }
                        break;
                    default:
                        breakOut = subtract = true;
                        break;
                }

                if (breakOut)
                {
                    if (output.Count % 2 == stack.Count % 2)
                        output.Push(new InvalidEntityNode(PushInvalidTokenException(current), new TokenCollection(current)));
                    
                    if (subtract)
                        index--;
  
                    ClearStack(output, stack);
                    break;
                }

                index++;
            }

            if (output.Count == 1)
                return output.Pop();
            
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
           
            return new InvalidIdentifierNode(Current, PushInvalidCurrentException());
        }

        /// <summary>
        /// Force Parse a Variable
        /// </summary>
        private ParseNodeCollection ParseVariable(ParseNodeCollection entity)
        {
            if (entity.NodeType == ParseNodeType.Identifier)
                return entity;

            var node = entity;
            while (true)
            {
                if (node.NodeType == ParseNodeType.Dot)
                    node = ((DotExpression)node).RHS;
                else if (node.NodeType == ParseNodeType.Identifier)
                    break;
                else
                    return new InvalidVariableNode(PushExpectedCurrentException(TokenType.Identifier), entity);
            }

            return new LongIdentiferNode((DotExpression)entity);
        }

        /// <summary>
        /// Force Parse a Variable
        /// <para>• Ends on index of closing <paramref name="breakOutSeperators"/> </para>
        /// </summary>
        private ParseNodeCollection ParseVariable(SeperatorKind breakOutSeperators) => ParseVariable(ParseEntity(false, breakOutSeperators));
        
        /// <summary>
        /// Force Parse a Type
        /// <para>• Ends on index of closing <paramref name="breakOutSeperators"/> </para>
        /// </summary>
        private TypeNode ParseType(SeperatorKind breakOutSeperators, bool force = true)
        {
            var current = Current;
            if (MatchToken(Keyword.Var,current, true))
            {
                TryMatchBreakOutSeperator(breakOutSeperators);
                return new AnonymousTypeNode();
            }

            return ValidateType(Current, ParseEntity(false, breakOutSeperators), force);
        }
        /// <summary>
        /// Force Parses a Type, excluding a generic defintion
        /// <para>• Ends on index of closing <paramref name="breakOutSeperators"/> </para>
        /// </summary>
        private TypeNode ParseNonGenericType(SeperatorKind breakOutSeperators, bool force = true)
        {
            var current = Current;
            if (MatchToken(current, Keyword.Var, true))
            {
                TryMatchBreakOutSeperator(breakOutSeperators);
                return new AnonymousTypeNode();
            }
            if (MatchType(TokenType.Keyword, current, true))
            {
                TryMatchBreakOutSeperator(breakOutSeperators);
                return new TypeKeywordNode((Keyword)current);
            }

            return ValidateType(current, ParseVariable(breakOutSeperators), true);
        }

        private TypeNode ValidateType(Token token, ParseNodeCollection entity, bool force)
        {
            var node = entity;
            
            while (true) 
            {
                if (node.NodeType == ParseNodeType.Dot)
                {
                    var dot = (DotExpression)node;
                    var lhs = dot.LHS.NodeType;

                    if (lhs != ParseNodeType.Type && lhs != ParseNodeType.Identifier)
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
            
            while (index < sourceFile.TokenCount)
            {
                bool breakOut = false;
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

                        if (current.SyntaxKind == SyntaxKind.GreaterThan)
                            return new ParseGenericResult(generic);

                        return new ParseGenericResult(new InvalidTokenException(current));
                    case TokenType.Keyword:
                        if (MatchType(expected, current) && ((Keyword)current).KeywordType == KeywordType.Type)
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
                            
                            expected = TokenType.BinaryOperator;
                        }
                        break;
                    case TokenType.Seperator:
                        if (!MatchType(expected, current) || !MatchToken(Seperator.Comma, current))
                            return new ParseGenericResult(new InvalidTokenException(current));
                        
                        expected = TokenType.Identifier | TokenType.Keyword;
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
                
                if (next == null || (MatchType(toMatch, next) && next != Seperator.OpenBracket))
                    return genericResult.Generic;
                
                index = beforeIndex;
                if (force)
                {
                    sourceFile.PushException(genericResult.Exception);
                    return new InvalidGenericCallNode(genericResult.Exception, PanicModeSimple(SeperatorKind.None));
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
            }
            
            return null;
        }

        /// <summary>
        /// Parse Generic Declaration
        /// <para>• Force starts at beginning LESSER THAN </para>
        /// • Ends one index after closing GREATER THAN
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
                if (MatchCurrent(BinaryOperator.GreaterThan, true))
                    return generic;
                
                break;
            }

            return new InvalidGenericDeclarationNode(PushExpectedCurrentException(BinaryOperator.GreaterThan), PanicModeSimple(SeperatorKind.None));
        }
    }
}