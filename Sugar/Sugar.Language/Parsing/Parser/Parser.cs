using System;
using System.Linq;
using System.Collections.Generic;

using Sugar.Language.Exceptions;
using Sugar.Language.Parsing.Parser.Enums;
using Sugar.Language.Parsing.Parser.Structure;

using Sugar.Language.Tokens;
using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Keywords;
using Sugar.Language.Tokens.Constants;
using Sugar.Language.Tokens.Operators;
using Sugar.Language.Tokens.Seperators;
using Sugar.Language.Tokens.Operators.Unary;
using Sugar.Language.Tokens.Operators.Binary;
using Sugar.Language.Tokens.Operators.Assignment;
using Sugar.Language.Tokens.Keywords.Subtypes.Loops;
using Sugar.Language.Tokens.Keywords.Subtypes.Types;
using Sugar.Language.Tokens.Keywords.Subtypes.Entities;
using Sugar.Language.Tokens.Keywords.Subtypes.Functions;
using Sugar.Language.Tokens.Keywords.Subtypes.Describers;
using Sugar.Language.Tokens.Keywords.Subtypes.Conditions;
using Sugar.Language.Tokens.Keywords.Subtypes.ControlStatements;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Types;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Loops;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.Describers;
using Sugar.Language.Parsing.Nodes.NodeGroups;
using Sugar.Language.Parsing.Nodes.UDDataTypes;
using Sugar.Language.Parsing.Nodes.Types.Enums;
using Sugar.Language.Parsing.Nodes.Types.Subtypes;
using Sugar.Language.Parsing.Nodes.CtrlStatements;
using Sugar.Language.Parsing.Nodes.TryCatchFinally;
using Sugar.Language.Parsing.Nodes.Values.Generics;
using Sugar.Language.Parsing.Nodes.Functions.Calling;
using Sugar.Language.Parsing.Nodes.UDDataTypes.Enums;
using Sugar.Language.Parsing.Nodes.Expressions.Lambdas;
using Sugar.Language.Parsing.Nodes.Functions.Properties;
using Sugar.Language.Parsing.Nodes.Expressions.Operative;
using Sugar.Language.Parsing.Nodes.TryCatchFinally.Blocks;
using Sugar.Language.Parsing.Nodes.Functions.Declarations;
using Sugar.Language.Parsing.Nodes.Expressions.Associative;
using Sugar.Language.Parsing.Nodes.UDDataTypes.Inheritance;
using Sugar.Language.Parsing.Nodes.Conditions.IfConditions;
using Sugar.Language.Parsing.Nodes.Statements.VariableCreation;
using Sugar.Language.Parsing.Nodes.Functions.Calling.Structure;
using Sugar.Language.Parsing.Nodes.Conditions.SwitchConditions;
using Sugar.Language.Parsing.Nodes.Values.Generics.Declarations;
using Sugar.Language.Parsing.Nodes.Functions.Properties.Accessors;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.OperatorOverloading;

namespace Sugar.Language.Parsing.Parser
{
    internal sealed partial class Parser
    {
        private readonly IList<Token> tokens;

        private int index;

        private Token Current { get => tokens[index]; }
        private Token Final { get => tokens[tokens.Count - 1]; }

        public Parser(List<Token> _tokens)
        {
            tokens = _tokens.AsReadOnly();
        }

        private Token LookAhead()
        {
            if (index + 1 >= tokens.Count)
                return null;

            return tokens[index + 1];
        }

        private void ForceMatchCurrentType(TokenType expected, bool increment = false)
        {
            if (index >= tokens.Count)
                throw new TokenExpectedException(expected, Final.Index);

            MatchType(expected, Current, true, increment);
        }

        private bool TryMatchCurrentType(TokenType expected, bool increment = false)
        {
            if (index >= tokens.Count)
                return false;

            return MatchType(expected, Current, false, increment);
        }

        private bool MatchType(TokenType expected, Token toMatch, bool throwError = false, bool increment = false)
        {
            if (toMatch != null && (expected & toMatch.Type) == toMatch.Type)
            {
                if (increment)
                    index++;

                return true;
            }

            if (throwError)
                throw new TokenExpectedException(expected, toMatch.Index);

            return false;
        }

        private bool TryMatchCurrent(Token matchTo, bool increment = false)
        {
            if (index >= tokens.Count)
                return false;

            return MatchToken(matchTo, Current, false, increment);
        }

        private void ForceMatchCurrent(Token matchTo, bool increment = false)
        {
            if (index >= tokens.Count)
                throw new TokenExpectedException(matchTo, Final.Index);

            MatchToken(matchTo, Current, true, increment);
        }

        private bool MatchToken(Token matchTo, Token toMatch, bool throwError = false, bool increment = false)
        {
            if (toMatch != matchTo)
            {
                if(throwError)
                    throw new TokenExpectedException(matchTo, toMatch, toMatch.Index);

                return false;
            }

            if (increment)
                index++;

            return true;
        }

        private bool MatchBreakOutSeperator(Token seperator, SeperatorType breakOutSeperators)
        {
            MatchType(TokenType.Seperator, seperator, true);

            var subType = (SeperatorType)seperator.SubType;
            return (subType & breakOutSeperators) == subType;
        }

        private void ForceMatchBreakOutSeperator(SeperatorType breakOutSeperators)
        {
            if(!MatchBreakOutSeperator(Current, breakOutSeperators))
                throw new TokenExpectedException(Current, $"{breakOutSeperators}", Current.Index);
        }

        public SyntaxTree Parse()
        {
            var nodes = new List<Node>();

            for (index = 0; index < tokens.Count; index++)
            {
                var toAdd = ParseStatement();

                if(toAdd.NodeType != NodeType.Empty)
                    nodes.Add(toAdd);
            }

            return nodes.Count == 0 ? null : new SyntaxTree(nodes.Count == 1 ? nodes[0] : new CompoundStatementNode(nodes));
        }

        private Node ParseStatement() => TryMatchCurrent(Seperator.FlowerOpenBracket) ? ParseScope() : ParseStatement(true, SeperatorType.Semicolon);

        private Node ParseStatement(bool readEmpty, SeperatorType breakOutSeperators)
        {
            switch (Current.Type)
            {
                case TokenType.Keyword:
                    return ParseKeyword(breakOutSeperators);
                case TokenType.Identifier:
                    return ParseIdentifier(breakOutSeperators);
                case TokenType.UnaryOperator:
                    var unaryOperator = Current as Operator;
                    index++;
                    return  new UnaryExpression(unaryOperator, ParseEntity(false, SeperatorType.Semicolon));
                case TokenType.Seperator:
                    if (MatchBreakOutSeperator(Current, breakOutSeperators))
                        return new EmptyNode();
                    else if (TryMatchCurrent(Seperator.BoxOpenBracket))
                        return ParseDescriberStatement(breakOutSeperators);
                    break;
            }

            throw new InvalidStatementException(Current.Index);
        }

        private Node ParseBlock(ParseScopeType scopeType)
        {
            var current = Current;
            if (MatchToken(Seperator.Lambda, current, false, true))
            {
                var type = scopeType & ParseScopeType.Lambda;

                if (type == ParseScopeType.LambdaStatement)
                    return new LambdaStatement(ParseStatement(false, SeperatorType.Semicolon));
                else if (type == ParseScopeType.LambdaExpression)
                    return new LambdaExpression(ParseExpression(false, false, SeperatorType.Semicolon));
                else
                    throw new InvalidTokenException(current, current.Index);
            }
            else if (MatchToken(Seperator.FlowerOpenBracket, current))
            {
                if((scopeType & ParseScopeType.Scope) != ParseScopeType.Scope)
                    throw new InvalidTokenException(current, current.Index);

                return ParseScope();
            }
            else
                return ParseStatement(true, SeperatorType.Semicolon);
        }

        private Node ParseScope()
        {
            var nodes = new List<Node>();
            ForceMatchCurrent(Seperator.FlowerOpenBracket, true);

            for (; index < tokens.Count; index++)
            {
                if (TryMatchCurrent(Seperator.FlowerCloseBracket))
                    break;

                var stmt = ParseStatement();
                if (stmt.NodeType != NodeType.Empty)
                    nodes.Add(stmt);
            }

            ForceMatchCurrent(Seperator.FlowerCloseBracket);
            switch(nodes.Count)
            {
                case 0:
                    return new EmptyNode();
                case 1:
                    return nodes[0];
                default:
                    return new ScopeNode(nodes);
            }
        }

        private Node ParseDescriberStatement(SeperatorType breakOutSeperators)
        {
            Node toReturn;
            DescriberNode describer = ParseDescribers();

            switch (Current.Type)
            {
                case TokenType.Identifier:
                    toReturn = ParseDeclaration(describer, ParseType(SeperatorType.Colon));

                    if (MatchFunctionPropertyDeclaration(toReturn.NodeType == NodeType.FunctionDeclaration))
                        return toReturn;
                    break;
                case TokenType.Keyword:
                    var keyword = Current as Keyword;
                    switch (keyword.KeywordType)
                    {
                        case KeywordType.Entity:
                            return ParseScopedEntity(describer, keyword);
                        case KeywordType.Function:
                            return ParseFunctionKeyword(describer);
                        case KeywordType.General:
                            if (TryMatchCurrent(Keyword.Var, true))
                            {
                                ForceMatchCurrent(Seperator.Colon, true);
                                toReturn = ParseVariableDeclaration(new DescriberNode(), new AnonymousTypeNode());
                                break;
                            }

                            throw new TokenExpectedException(keyword, "Type", keyword.Index);
                        case KeywordType.Type:
                            toReturn = ParseDeclaration(describer);

                            if (MatchFunctionPropertyDeclaration(toReturn.NodeType == NodeType.FunctionDeclaration))
                                return toReturn;
                            break;
                        default:
                            throw new TokenExpectedException(keyword, "Type", keyword.Index);
                    }
                    break;
                default:
                    throw new TokenExpectedException(Current, "Type", Current.Index);
            }

            ForceMatchBreakOutSeperator(breakOutSeperators);
            return toReturn;
        }

        private DescriberNode ParseDescribers()
        {
            var describers = new List<Node>();

            while (index < tokens.Count)
            {
                ForceMatchCurrent(Seperator.BoxOpenBracket, true);
                foreach (var describer in ParseDescriber())
                    describers.Add(describer);

                index++;
                if (!TryMatchCurrent(Seperator.BoxOpenBracket))
                    break;
            }

            return new DescriberNode(describers);
        }

        private List<Node> ParseDescriber()
        {
            var keywords = new List<Node>();

            while (index < tokens.Count)
            {
                ForceMatchCurrentType(TokenType.Keyword);
                keywords.Add(new DescriberKeywordNode(Current as Keyword));

                index++;
                ForceMatchCurrentType(TokenType.Seperator);
                if (TryMatchCurrent(Seperator.Comma))
                {
                    index += 2;
                    continue;
                }

                ForceMatchCurrent(Seperator.BoxCloseBracket);
                return keywords;
            }

            throw new TokenExpectedException(Seperator.BoxCloseBracket, Final.Index);
        }

        private Node ParseKeyword(SeperatorType breakOutSeperators)
        {
            Node toReturn = null;

            var current = Current;
            if (MatchToken(Keyword.Create, current))
            {
                toReturn = ParseConstructorCall(false);
                index++;
            }
            else if (MatchToken(Keyword.Throw, current, false, true))
                toReturn = new ThrowExceptionNode(ParseExpression(false, false, SeperatorType.Semicolon));
            else if (MatchToken(Keyword.Import, current))
                toReturn = ParseImportStatement();
            else if (MatchToken(Keyword.Try, current))
            {
               
                return ParseTryCatchBlock();
            }
            else if (MatchToken(Keyword.Print, current))
            {
                toReturn = ParsePrintCall();
                index++;
            }
            else if (MatchToken(Keyword.Input, current))
            {
                toReturn = ParseInputCall();
                index++;
            }
            else if (MatchToken(Keyword.Var, current, false, true))
            {
                ForceMatchCurrent(Seperator.Colon, true);
                toReturn = ParseVariableDeclaration(new DescriberNode(), new AnonymousTypeNode());
            }
            else
            {
                switch ((KeywordType)current.SubType)
                {
                    case KeywordType.ControlStatement:
                        if (current == ControlKeyword.Return)
                            toReturn = ParseReturn();
                        else
                        {
                            if (current == ControlKeyword.Break)
                                toReturn = new BreakNode();
                            else
                                toReturn = new ContinueNode();

                            index++;
                        }
                        break;
                    case KeywordType.Loop:
                        if (current == LoopKeyword.For)
                            return ParseForLoop();
                        else if (current == LoopKeyword.While)
                            return ParseWhileLoop();
                        else if (current == LoopKeyword.Foreach)
                            return ParseForeachLoop();
                        else
                            return ParseDoWhileLoop();
                    case KeywordType.Condition:
                        if (current == ConditionKeyword.If)
                            return ParseIfCondition();
                        else if (current == ConditionKeyword.Switch)
                            return ParseSwitchCase();

                        throw new InvalidTokenException(current, current.Index);
                    case KeywordType.Entity:
                        return ParseScopedEntity(new DescriberNode(), (Keyword)current);
                    case KeywordType.Type:
                        toReturn = ParseDeclaration(new DescriberNode());

                        if (MatchFunctionPropertyDeclaration(toReturn.NodeType == NodeType.FunctionDeclaration))
                            return toReturn;
                        break;
                    case KeywordType.Function:
                        return ParseFunctionKeyword(new DescriberNode());
                }
            }

            ForceMatchBreakOutSeperator(breakOutSeperators);
            return toReturn;
        }

        private Node ParseFunctionKeyword(Node describer)
        {
            var current = Current;

            if (MatchToken(FunctionKeyword.Operator, current))
                return ParseOperatorOverload(describer);
            else if (MatchToken(FunctionKeyword.Indexer, current))
                return ParseIndexerDeclaration(describer);
            else if (MatchToken(FunctionKeyword.Void, current, false, true))
                return ParseFunctionDeclaration(describer, new VoidNode());
            else if (MatchToken(FunctionKeyword.Constructor, current, false, true))
                return ParseFunctionDeclaration(describer, ParseType(SeperatorType.Any));
            else
                return ParseConversionOverload(describer);
        }

        private Node ParseIdentifier(SeperatorType breakOutSeperators)
        {
            var variable = ParseEntity(false, SeperatorType.Any);
            var node = variable;

            while (true)
                if (node.NodeType == NodeType.Dot)
                    node = ((DotExpression)node).RHS;
                else
                    break;

            Node toReturn = null;
            switch(node.NodeType)
            {
                case NodeType.FunctionCall:
                    index++;
                    toReturn = variable;
                    break;
                case NodeType.Type:
                    toReturn = ParseDeclaration(new DescriberNode(), variable as TypeNode);

                    if (MatchFunctionPropertyDeclaration(toReturn.NodeType == NodeType.FunctionDeclaration))
                        return toReturn;
                    break;
                case NodeType.Indexer:
                case NodeType.Variable:
                    var next = LookAhead();
                    if (MatchType(TokenType.UnaryOperator, next))
                    {
                        toReturn = new UnaryExpression((UnaryOperator)next, variable);
                        index += 2;
                    }
                    else if (MatchType(TokenType.AssignmentOperator, next, false, true))
                        toReturn = ParseVariableAssignment(variable, next as AssignmentOperator, breakOutSeperators);
                    else 
                    {
                        if (variable.NodeType == NodeType.Variable)
                        {
                            toReturn = ParseDeclaration(new DescriberNode(), new CustomTypeNode(variable));

                            if (MatchFunctionPropertyDeclaration(toReturn.NodeType == NodeType.FunctionDeclaration))
                                return toReturn;
                        }
                        else
                            throw new InvalidStatementException(Current.Index);
                    }
                    break;
            }

            ForceMatchBreakOutSeperator(breakOutSeperators);
            return toReturn;
        }

        private bool MatchFunctionPropertyDeclaration(bool isFunction) => isFunction || (!isFunction && TryMatchCurrent(Seperator.FlowerCloseBracket, false));

        private Node ParseDeclaration(DescriberNode describer) => ParseDeclaration(describer, ParseType(SeperatorType.Any));

        private Node ParseDeclaration(DescriberNode describer, TypeNode typeNode)
        {
            index++;
            if (TryMatchCurrent(Seperator.Colon, true))
                return ParseVariableDeclaration(describer, typeNode);
            else if (TryMatchCurrentType(TokenType.Identifier))
                return ParseFunctionDeclaration(describer, typeNode);
            else
                throw new InvalidTokenException(Current, Current.Index);
        }

        private Node ParseVariableDeclaration(DescriberNode _describer, TypeNode _type)
        {
            ForceMatchCurrentType(TokenType.Identifier);
            List<Node> assignNodes = new List<Node>();

            if (LookAhead() == Seperator.FlowerOpenBracket)
                assignNodes.Add(ParseVariableInitialisation(_describer, _type, ParseProperty(_type, new IdentifierNode((Identifier)Current)), true));
            else
            {
                while (index < tokens.Count)
                {
                    ForceMatchCurrentType(TokenType.Identifier);
                    assignNodes.Add(ParseVariableInitialisation(_describer, _type, new IdentifierNode((Identifier)Current), false));

                    if (TryMatchCurrent(Seperator.Semicolon))
                        break;

                    index++;
                }

                ForceMatchCurrent(Seperator.Semicolon);
            }

            switch (assignNodes.Count)
            {
                case 0:
                    throw new TokenExpectedException(TokenType.Identifier, Current.Index);
                case 1:
                    return assignNodes[0];
                default:
                    return new CompoundStatementNode(assignNodes);
            }
        }

        private Node ParseVariableInitialisation(DescriberNode describer, TypeNode _type, Node _variableNode, bool isProperty)
        {
            var next = LookAhead();
            if (next != BinaryOperator.Assignment)
            {
                if (isProperty || !isProperty && next == Seperator.Comma || next == Seperator.Semicolon)
                {
                    if (!isProperty)
                        index++;

                    return new DeclarationNode(describer, _type, _variableNode);
                }

                throw new TokenExpectedException(BinaryOperator.Assignment, next.Index);
            }

            index += 2;
            return new InitializeNode(describer, _type, _variableNode, ParseExpression(false, false, SeperatorType.Semicolon | (isProperty ? SeperatorType.Any : SeperatorType.Comma)));
        }

        private Node ParseVariableAssignment(Node variable, AssignmentOperator assignmentOperator, SeperatorType breakOutSeperators)
        {
            index++;
            var expresssion = ParseExpression(false, true, breakOutSeperators);

            if (Current.Type == TokenType.AssignmentOperator)
            {
                ForceMatchCurrent(BinaryOperator.Assignment);

                var node = expresssion;
                while (true)
                    if (node.NodeType == NodeType.Dot)
                        node = ((DotExpression)node).RHS;
                    else
                        break;

                if (node.NodeType != NodeType.Variable)
                    throw new InvalidStatementException(Current.Index);
                
                expresssion = ParseVariableAssignment(expresssion, BinaryOperator.Assignment, breakOutSeperators);
            }

            expresssion = assignmentOperator.BaseOperator == null ? expresssion : new BinaryExpression(assignmentOperator.BaseOperator, variable, expresssion);
            return new AssignmentNode(variable, expresssion);
        }

        private Node ParseImportStatement()
        {
            index++;
            UDDataType entityType = UDDataType.Namespace;

            var current = Current;
            if (MatchToken(EntityKeyword.Class, current))
                entityType = UDDataType.Class;
            else if (MatchToken(EntityKeyword.Struct, current))
                entityType = UDDataType.Struct;
            else if (MatchToken(EntityKeyword.Interface, current))
                entityType = UDDataType.Interface;
            else if (MatchToken(EntityKeyword.Enum, current))
                entityType = UDDataType.Enum;

            if (entityType != UDDataType.Namespace)
                index++;

            ForceMatchCurrentType(TokenType.Identifier);
            return new ImportNode(entityType, ParseEntity(false, SeperatorType.Semicolon));
        }

        private Node ParseTryCatchBlock()
        {
            ForceMatchCurrent(Keyword.Try, true);
            var tryBlock = new TryBlockNode(ParseBlock(ParseScopeType.Scope | ParseScopeType.SingleLine));
            index++;

            Node catchBlock = null, finallyBlock = null;
            if(TryMatchCurrent(Keyword.Catch, true))
            {
                Node arguments;
                if (TryMatchCurrent(Seperator.OpenBracket))
                {
                    arguments = new CatchBlockArgumentsNode(ParseDeclarationArguments());
                    index++;
                }
                else
                    arguments = new CatchBlockArgumentsNode();

                catchBlock = new CatchBlockNode(arguments, ParseBlock(ParseScopeType.Scope | ParseScopeType.SingleLine));
                index++;
            }

            if (TryMatchCurrent(Keyword.Finally, true))
                finallyBlock = new FinallyBlockNode(ParseBlock(ParseScopeType.Scope | ParseScopeType.SingleLine));

            return new TryCatchFinallyBlockNode(tryBlock, catchBlock, finallyBlock);
        }

        //Expressions and Entities
        private Node ParseExpression(bool readEmpty, bool readAssignment, SeperatorType breakOutSeperators)
        {
            var output = new List<Node>();
            var stack = new List<Token>();
            TokenType expected = TokenType.Constant | TokenType.Identifier | TokenType.Seperator | TokenType.UnaryOperator | TokenType.Keyword;

            bool breakOut = false;
            while (index < tokens.Count)
            {
                var current = Current;

                switch (current.Type)
                {
                    case TokenType.Keyword:
                        MatchType(expected, current, true);

                        if (TryMatchCurrent(Keyword.Default, true))
                        {
                            output.Add(ParseDefault(output));
                            breakOut = true;
                            break;
                        }
                        else if (TryMatchCurrent(Keyword.Create))
                            output.Add(ParseConstructorCall(true));
                        else if (TryMatchCurrent(Keyword.This))
                            output.Add(ParseEntity(true, SeperatorType.Any));
                        else if (TryMatchCurrent(Keyword.Input))
                            output.Add(ParseInputCall());
                        else
                        {
                            if (((Keyword)current).KeywordType == KeywordType.Type)
                            {
                                output.Add(ParseEntity(true, SeperatorType.Any));
                                expected = TokenType.Seperator | TokenType.Operator;
                                break;
                            }

                            throw new InvalidTokenException(current, current.Index);
                        }
                        expected = TokenType.Seperator | TokenType.Operator;
                        break;
                    case TokenType.Constant:
                        MatchType(expected, current, true);

                        output.Add(ParseEntity(true, SeperatorType.Any));
                        expected = TokenType.Seperator | TokenType.BinaryOperator;
                        break;
                    case TokenType.Identifier:
                        MatchType(expected, current, true);

                        output.Add(ParseEntity(true, SeperatorType.Any));
                        expected = TokenType.Seperator | TokenType.Operator;
                        break;
                    case TokenType.Seperator:
                        MatchType(expected, current, true);

                        if(MatchBreakOutSeperator(current, breakOutSeperators))
                        {
                            breakOut = true;
                            break;
                        }

                        if (MatchToken(Seperator.OpenBracket, current, false, true))
                        {
                            output.Add(ParseExpression(false, false, SeperatorType.CloseBracket));
                            expected = TokenType.Operator | TokenType.Seperator;
                        }
                        else if (MatchToken(Seperator.Questionmark,current, false, true))
                        {
                            output.Add(ParseTernary(output, stack));
                            expected = TokenType.Seperator | TokenType.Operator;
                        }
                        else
                            throw new InvalidTokenException(current, current.Index);
                        break;
                    case TokenType.UnaryOperator:
                        MatchType(expected, current, true);

                        if (stack.Count > 0 && MatchType(TokenType.UnaryOperator, stack[stack.Count - 1], false, false))
                            throw new TokenExpectedException(expected, current, current.Index);

                        var asOperator = (Operator)current;
                        switch (tokens[index - 1].Type)
                        {
                            case TokenType.Identifier:
                                AddPostfix();
                                break;
                            case TokenType.BinaryOperator:
                                AddPrefix();
                                break;
                            case TokenType.Seperator:
                                var prev = tokens[index - 1];
                                if (prev == Seperator.CloseBracket || prev == Seperator.BoxCloseBracket)
                                    AddPostfix();
                                else
                                    AddPrefix();
                                break;
                        }

                        void AddPrefix()
                        {
                            stack.Add(Operator.ConvertUnaryToPrefix(asOperator));
                            expected = TokenType.Identifier | TokenType.Constant;

                            if (LookAhead() == Seperator.OpenBracket)
                                expected |= TokenType.Seperator;
                        }
                        void AddPostfix()
                        {
                            stack.Add(asOperator);
                            expected = TokenType.BinaryOperator | TokenType.Seperator;
                        }
                        break;
                    case TokenType.AssignmentOperator:
                        MatchType(expected, current, true);

                        if (!readAssignment)
                            throw new InvalidTokenException(current, current.Index);

                        breakOut = true;
                        break;
                    case TokenType.BinaryOperator:
                        MatchType(expected, current, true);

                        asOperator = (Operator)current;

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

                if (breakOut)
                {
                    ClearStack(output, stack);
                    break;
                }

                index++;
            }

            if (!breakOut)
            {
                if (index >= tokens.Count)
                    throw new TokenExpectedException(Final, $"{breakOutSeperators}", Final.Index);
                else
                    throw new TokenExpectedException(Current, $"{breakOutSeperators}", Current.Index);
            }
            else if (output.Count == 0)
            {
                if (readEmpty)
                    return new EmptyNode();

                if (index >= tokens.Count)
                    throw new TokenExpectedException(Final, $"{breakOutSeperators}", Final.Index);
                else
                    throw new InvalidTokenException(Current, Current.Index);
            }
           
            return output[0];
        }

        private Node ParseExpression(Operator top, List<Node> output)
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
                    return new CastExpression(lhs, rhs);
                else
                    return new BinaryExpression(top, lhs, rhs);
            }
        }

        private Node ParseExpression(Operator current, List<Node> output, List<Token> stack)
        {
            var top = (Operator)stack[stack.Count - 1];
            if (current.Precedence < top.Precedence && !(top.Precedence == current.Precedence && current.LeftAssociative))
                return null;

            return ParseExpression(top, output);
        }

        private void ClearStack(List<Node> output, List<Token> stack)
        {
            while (stack.Count > 0)
            {
                output.Add(ParseExpression((Operator)stack[stack.Count - 1], output));
                stack.RemoveAt(stack.Count - 1);
            }
        }

        private Node ParseTernary(List<Node> output, List<Token> stack)
        {
            if (output.Count == 0)
                throw new InvalidTokenException(Current, Current.Index);

            ClearStack(output, stack);

            var condition = output[output.Count - 1];
            output.RemoveAt(output.Count - 1);

            var trueExpression = ParseExpression(false, false, SeperatorType.Colon);

            index++;
            var falseExpression = ParseExpression(false, false, SeperatorType.Semicolon | SeperatorType.CloseBracket);

            index--;
            return new TernaryExpression(condition, trueExpression, falseExpression);
        }

        private IndexerExpression ParseIndexer(Node operhand)
        {
            index++;
            var arguments = new List<Node>();
            while (index < tokens.Count)
            {
                arguments.Add(ParseExpression(false, false, SeperatorType.BoxCloseBracket | SeperatorType.Comma));

                if (TryMatchCurrent(Seperator.BoxCloseBracket))
                    break;
                else if (TryMatchCurrent(Seperator.Comma, true))
                    continue;
            }

            ForceMatchCurrent(Seperator.BoxCloseBracket);
            return new IndexerExpression(operhand, new IndexerArgumentsNode(arguments));
        }

        private Node ParseDefault(List<Node> output)
        {
            if (output.Count > 0)
                throw new InvalidTokenException(Current, Current.Index);

            return new DefaultValueNode();
        }

        private Node ParseReturn()
        {
            index++;
            if (TryMatchCurrent(Seperator.Semicolon, false))
                return new ReturnKeyword();
            else
                return new ReturnKeyword(ParseExpression(false, false, SeperatorType.Semicolon));
        }

        private Node ParseExpressionList()
        {
            var nodes = new List<Node>();
            ForceMatchCurrent(Seperator.FlowerOpenBracket, true);

            while(index < tokens.Count)
            {
                if (TryMatchCurrent(Seperator.FlowerCloseBracket))
                    return Extract();

                var expression = ParseExpression(false, true, SeperatorType.Comma | SeperatorType.FlowerCloseBracket);
                if (TryMatchCurrent(BinaryOperator.Assignment, true))
                    expression = new AssignmentNode(expression, ParseExpression(false, false, SeperatorType.Comma | SeperatorType.FlowerCloseBracket));

                nodes.Add(expression);

                ForceMatchCurrentType(TokenType.Seperator);
                if (TryMatchCurrent(Seperator.Comma, true))
                    continue;

                ForceMatchCurrent(Seperator.FlowerCloseBracket);
                return Extract();
            }

            throw new TokenExpectedException(Seperator.FlowerCloseBracket, Final.Index);
            Node Extract()
            {
                if (nodes.Count == 0)
                    return new EmptyNode();
                else
                    return new CompoundStatementNode(nodes);
            }
        }

        private Node ParseEntity(bool inExpression, SeperatorType breakOutSeperators)
        {
            var output = new List<Node>();
            var stack = new List<Token>();
            TokenType expected = TokenType.Constant | TokenType.Identifier | TokenType.Keyword;

            bool breakOut = false, subtract = false;
            while (index < tokens.Count)
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

                        Node identifier = new IdentifierNode((Identifier)current);
                        if (MatchToken(BinaryOperator.LesserThan, LookAhead()))
                        {
                            Node generic = ParseGenericCall(!inExpression);
                            identifier = generic == null ? identifier : new CustomTypeNode(identifier).AddChild(generic);
                        }

                        output.Add(identifier);
                        expected = TokenType.Seperator;
                        break;
                    case TokenType.Seperator:
                        MatchType(expected, current, true);

                        if (MatchBreakOutSeperator(current, breakOutSeperators))
                        {
                            breakOut = true;
                            break;
                        }

                        if (MatchToken(Seperator.Dot, current))
                        {
                            stack.Add(BinaryOperator.DotOperator);
                            expected = TokenType.Identifier;
                            break;
                        }
                        else if (MatchToken(Seperator.BoxOpenBracket, current))
                        {
                            output.Add(ParseIndexer(output[output.Count - 1]));
                            CleanUp();
                            break;
                        }
                        else if (MatchToken(Seperator.OpenBracket, current))
                        {
                            output.Add(ParseFunctionCall(inExpression, false, output[output.Count - 1]));
                            CleanUp();
                            break;
                        }

                        breakOut = subtract = true;
                        void CleanUp()
                        {
                            output.RemoveAt(output.Count - 2);

                            var next = LookAhead();
                            if (next == Seperator.OpenBracket)
                                throw new InvalidTokenException(next, next.Index);
                        }
                        break;
                    case TokenType.Keyword:
                        if (!MatchType(expected, current))
                        {
                            breakOut = subtract = true;
                            break;
                        }

                        if (MatchToken(Keyword.This, current))
                        {
                            output.Add(new ThisNode());
                            expected = TokenType.Seperator;
                            break;
                        }
                        else
                        {
                            var keyword = (Keyword)current;
                            if (keyword.KeywordType == KeywordType.Type)
                            {
                                if (MatchToken(TypeKeyword.Array, current))
                                {
                                    if (inExpression)
                                        output.Add(new TypeKeywordNode(TypeKeyword.Array));
                                    else
                                        output.Add(new ArrayTypeNode(ParseGenericCall(true)));
                                }
                                else
                                    output.Add(new TypeKeywordNode(keyword));

                                expected = TokenType.Seperator;
                                break;
                            }

                            throw new InvalidTokenException(current, current.Index);
                        }
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

            if (output.Count == 0)
                throw new TokenExpectedException(expected, Current.Index);

            return output[0];
        }

        private Node ParseVariable(SeperatorType breakOutSeperators)
        {
            var token = Current;
            var entity = ParseEntity(false, breakOutSeperators);

            var node = entity;
            while (true)
                if (node.NodeType == NodeType.Dot)
                    node = ((DotExpression)node).RHS;
                else
                    break;

            if (node.NodeType != NodeType.Variable)
                throw new TokenExpectedException(TokenType.Identifier, token.Index);

            return entity;
        }

        private TypeNode ParseType(SeperatorType breakOutSeperators)
        {
            if (TryMatchCurrent(Keyword.Var, true))
            {
                ForceMatchBreakOutSeperator(breakOutSeperators);
                return new AnonymousTypeNode();
            }

            return ValidateType(Current, ParseEntity(false, breakOutSeperators));
        }

        private TypeNode ValidateType(Token token, Node entity)
        {
            var node = entity;
            while (true)
                if (node.NodeType == NodeType.Dot)
                    node = ((DotExpression)node).RHS;
                else
                    break;

            switch (node.NodeType)
            {
                case NodeType.Type:
                    return (TypeNode)entity;
                case NodeType.Variable:
                    return new CustomTypeNode(entity);
                default:
                    throw new TokenExpectedException(token, "Type", token.Index);
            }
        }

        private Node ParseInheritanceTypes()
        {
            ForceMatchCurrent(Seperator.Colon, true);

            var types = new List<Node>();
            while (index < tokens.Count)
            {
                types.Add(new InheritsTypeNode(ParseType(SeperatorType.Comma)));

                if (TryMatchCurrent(Seperator.Comma, true))
                    continue;

                break;
            }

            index++;
            return new InheritanceNode(types);
        }

        private ParseGenericResult ParseGenericCall()
        {
            index++;
            var output = new List<Node>();
            TokenType expected = TokenType.BinaryOperator;

            while (index < tokens.Count)
            {
                var current = Current;

                switch (current.Type)
                {
                    case TokenType.BinaryOperator:
                        if (!MatchType(expected, current))
                            return new ParseGenericResult(null, new TokenExpectedException(expected, 0));

                        if (TryMatchCurrent(BinaryOperator.LesserThan, false))
                        {
                            output.Add(new GenericCallNode());
                            expected = TokenType.Identifier | TokenType.Keyword;
                            break;
                        }
                        else if (TryMatchCurrent(BinaryOperator.GreaterThan, false))
                        {
                            bool found = false;
                            var types = new List<Node>();

                            while (output.Count > 0)
                            {
                                var top = output[output.Count - 1];
                                if (top.NodeType == NodeType.Generic)
                                {
                                    found = true;
                                    for (int i = types.Count - 1; i >= 0; i--)
                                        top.AddChild(types[i]);

                                    if (output.Count > 1)
                                    {
                                        output[output.Count - 2].AddChild(top);
                                        output.RemoveAt(output.Count - 1);
                                    }
                                    break;
                                }

                                types.Add(top);
                                output.RemoveAt(output.Count - 1);
                            }

                            if (!found)
                                return new ParseGenericResult(null, new Exception("> expected"));
                            else if (output.Count == 1)
                                return new ParseGenericResult(output[0], null);
                            break;
                        }

                        return new ParseGenericResult(null, new Exception("Binary Operator detected but not expected"));
                    case TokenType.Keyword:
                        if (!MatchType(expected, current))
                        { }
                        else if ((current as Keyword).KeywordType == KeywordType.Type)
                        {
                            output.Add(new TypeKeywordNode((Keyword)current));
                            expected = TokenType.Seperator | TokenType.Operator;
                            break;
                        }

                        return new ParseGenericResult(null, new Exception("Keyword detected but not expected"));
                    case TokenType.Identifier:
                        if (!MatchType(expected, current))
                            return new ParseGenericResult(null, new Exception("Variable detected but not expected"));
                        else
                        {
                            output.Add(new CustomTypeNode(new IdentifierNode((Identifier)current)));
                            expected = TokenType.Seperator | TokenType.Operator;
                        }
                        break;
                    case TokenType.Seperator:
                        if (!MatchType(expected, current) || !MatchToken(Seperator.Comma, current))
                            return new ParseGenericResult(null, new Exception("Seperator detected but not expected"));
                        else
                            expected = TokenType.Identifier | TokenType.Keyword;
                        break;
                }

                index++;
            }

            return new ParseGenericResult(null, new Exception("> expected"));
        }

        private Node ParseGenericCall(bool force)
        {
            int beforeIndex = index;

            var result = ParseGenericCall();
            if (!result.Success)
            {
                if (force)
                    throw result.Exception;

                index = beforeIndex;
                return null;
            }
            else
            {
                var next = LookAhead();
                var toMatch = TokenType.Seperator | TokenType.BinaryOperator;
                if (force)
                    toMatch |= TokenType.Identifier;

                if (MatchType(toMatch, next) && next != Seperator.OpenBracket)
                    return result.Generic;
                else
                {
                    if (force)
                        throw result.Exception;

                    index = beforeIndex;
                    return null;
                }
            }
        }

        private Node ParseGenericDeclaration()
        {
            var types = new List<Node>();
            ForceMatchCurrent(BinaryOperator.LesserThan, true);

            while (index < tokens.Count)
            {
                var current = Current;
                MatchType(TokenType.Identifier, current, true);
                Node variable = new IdentifierNode((Identifier)current), enforcement = null;

                index++;
                if (TryMatchCurrent(Seperator.Colon))
                    enforcement = ParseInheritanceTypes();

                types.Add(enforcement == null ? new GenericDeclarationVariableNode(variable) : new GenericDeclarationVariableNode(variable, enforcement));
                if (TryMatchCurrent(Seperator.Semicolon, true))
                    continue;

                break;
            }

            ForceMatchCurrent(BinaryOperator.GreaterThan);
            return new GenericDeclarationNode(types);
        }

        //Loops
        private Node ParseForLoop()
        {
            ForceMatchCurrent(LoopKeyword.For, true);
            ForceMatchCurrent(Seperator.OpenBracket, true);
            var initialise = ParseMultiLine(Seperator.Semicolon, SeperatorType.Semicolon | SeperatorType.Semicolon | SeperatorType.Comma);

            var expression = ParseExpression(true, false, SeperatorType.Semicolon);
            index++;
            var increment = ParseMultiLine(Seperator.CloseBracket, SeperatorType.CloseBracket | SeperatorType.Comma);

            var body = ParseBlock(ParseScopeType.Scope | ParseScopeType.SingleLine);
            return new ForLoopNode(initialise, expression, increment, body);

            Node ParseMultiLine(Seperator finalBreakOut, SeperatorType breakOutSeperators)
            {
                var subStatements = new List<Node>();

                while (index < tokens.Count)
                {
                    subStatements.Add(ParseStatement(true, breakOutSeperators));
                    if (Current == finalBreakOut)
                    {
                        index++;
                        return subStatements.Count == 1 ? subStatements[0] : new CompoundStatementNode(subStatements);
                    }

                    index++;
                }

                throw new TokenExpectedException(finalBreakOut, Final.Index);
            }
        }

        private Node ParseForeachLoop()
        {
            ForceMatchCurrent(LoopKeyword.Foreach, true);
            ForceMatchCurrent(Seperator.OpenBracket, true);

            var type = ParseType(SeperatorType.Colon);
            ForceMatchCurrent(Seperator.Colon, true);

            ForceMatchCurrentType(TokenType.Identifier);
            var variable = new IdentifierNode((Identifier)Current);
            index++;

            ForceMatchCurrent(DescriberKeyword.In, true);
            var array = ParseVariable(SeperatorType.CloseBracket);
            index++;

            var body = ParseBlock(ParseScopeType.Scope | ParseScopeType.SingleLine);

            return new ForeachLoopNode(new DeclarationNode(new DescriberNode(), type, variable), array, body);
        }

        private Node ParseWhileLoop()
        {
            ForceMatchCurrent(LoopKeyword.While, true);
            ForceMatchCurrent(Seperator.OpenBracket, true);
            var condition = ParseExpression(false, false, SeperatorType.CloseBracket);

            index++;
            var body = ParseBlock(ParseScopeType.Scope | ParseScopeType.SingleLine);
            return new WhileLoopNode(condition, body);
        }

        private Node ParseDoWhileLoop()
        {
            ForceMatchCurrent(LoopKeyword.Do, true);
            var body = ParseBlock(ParseScopeType.Scope | ParseScopeType.SingleLine);
            index++;

            ForceMatchCurrent(LoopKeyword.While, true);
            ForceMatchCurrent(Seperator.OpenBracket, true);

            var condition = ParseExpression(false, false, SeperatorType.CloseBracket);
            return new DoWhileNode(condition, body);
        }

        //Conditions
        private Node ParseIfCondition()
        {
            var nodes = new List<Node>();

            while (index < tokens.Count)
            {
                var current = Current;
                if (MatchToken(ConditionKeyword.If, current, false, true))
                {
                    ForceMatchCurrent(Seperator.OpenBracket, true);

                    var expression = ParseExpression(false, false, SeperatorType.CloseBracket);
                    index++;
                    var body = ParseBlock(ParseScopeType.Scope | ParseScopeType.SingleLine);

                    nodes.Add(new IfNode(expression, body));
                }
                else if (MatchToken(ConditionKeyword.Else, current, false, true))
                {
                    if (TryMatchCurrent(ConditionKeyword.If))
                        continue;

                    nodes.Add(new ElseNode(ParseBlock(ParseScopeType.Scope | ParseScopeType.SingleLine)));
                    break;
                }
                else
                    break;

                index++;
            }

            return nodes.Count == 1 ? nodes[0] : new IfElseChainNode(nodes);
        }

        private Node ParseSwitchCase()
        {
            ForceMatchCurrent(ConditionKeyword.Switch, true);
            ForceMatchCurrent(Seperator.OpenBracket, true);

            var valueToCheck = ParseExpression(false, false, SeperatorType.CloseBracket);
            index++;

            ForceMatchCurrent(Seperator.FlowerOpenBracket, true);

            var cases = new List<Node>();
            while (index < tokens.Count)
            {
                var current = Current;
                if (MatchToken(Seperator.FlowerCloseBracket, current))
                    break;

                if (MatchToken(ConditionKeyword.Case, current, false, true))
                {
                    Node value;
                    current = Current;
                    switch (current.Type)
                    {
                        case TokenType.Identifier:
                            value = ParseEntity(false, SeperatorType.Colon);

                            if (MatchType(TokenType.Identifier, LookAhead()))
                            {
                                int before = index;

                                index++;
                                if (LookAhead() == Keyword.When)
                                    value = ParseWhenStatement(ValidateType(current, value));
                                else
                                    index = before;
                            }
                            break;
                        case TokenType.Keyword:
                            var type = ParseType(SeperatorType.Colon);

                            ForceMatchCurrent(Seperator.Colon, true);
                            ForceMatchCurrentType(TokenType.Identifier);

                            value = ParseWhenStatement(type);
                            break;
                        case TokenType.Constant:
                            value = new ConstantValueNode((Constant)current);
                            index++;
                            break;
                        default:
                            throw new TokenExpectedException(TokenType.Constant, current, current.Index);
                    }

                    ForceMatchCurrent(Seperator.Colon, true);
                    if (TryMatchCurrent(ConditionKeyword.Case, false))
                    {
                        cases.Add(new CaseNode(value));
                        continue;
                    }

                    cases.Add(new CaseNode(value, ParseBody(), ParseControlStatement()));
                }
                else if (MatchToken(Keyword.Default, current, false, true))
                {
                    ForceMatchCurrent(Seperator.Colon, true);

                    cases.Add(new DefaultNode(ParseBody(), ParseControlStatement()));
                    index++;

                    ForceMatchCurrent(Seperator.FlowerCloseBracket);
                    continue;
                }
                else
                    throw new InvalidTokenException(current, current.Index);

                index++;
            }

            return new SwitchNode(valueToCheck, cases);

            Node ParseBody()
            {
                if (TryMatchCurrent(ControlKeyword.Break) || TryMatchCurrent(ControlKeyword.Return))
                    return new EmptyNode();

                var body = ParseBlock(ParseScopeType.Scope | ParseScopeType.SingleLine);
                index++;
                return body;
            }

            Node ParseControlStatement()
            {
                Node controlStatement;
                ForceMatchCurrentType(TokenType.Keyword, false);

                if (TryMatchCurrent(ControlKeyword.Return))
                    controlStatement = ParseReturn();
                else 
                {
                    ForceMatchCurrent(ControlKeyword.Break, true);
                    controlStatement = new BreakNode();
                }

                ForceMatchCurrent(Seperator.Semicolon);
                return controlStatement;
            }

            Node ParseWhenStatement(Node typeNode)
            {
                var variable = new IdentifierNode((Identifier)Current);

                index++;
                ForceMatchCurrent(Keyword.When, true);
                var expression = ParseExpression(false, false, SeperatorType.Colon);

                return new WhenNode(new DeclarationNode(new DescriberNode(new DescriberKeywordNode(DescriberKeyword.Const)), typeNode, variable), expression);
            }
        }

        //Functions and Properties
        private Node ParseProperty(Node typeNode, Node variableNode)
        {
            index++;
            ForceMatchCurrent(Seperator.FlowerOpenBracket, true);

            Node getNode = null, setNode = null, describer = TryParseDescriber();
            if (TryMatchCurrent(Keyword.Get, true))
            {
                getNode = new GetNode(describer, ParseAccessorBody(ParseScopeType.LambdaExpression));
                describer = TryParseDescriber();
            }

            if (describer.ChildCount != 0)
            {  
                ForceMatchCurrent(Keyword.Set, true);
                setNode = ParseSetNode(describer);
            }
            else if(TryMatchCurrent(Keyword.Set, true))
                    setNode = ParseSetNode(describer);

            ForceMatchCurrent(Seperator.FlowerCloseBracket);

            if (getNode != null && setNode != null)
                return new PropertyGetSetNode(variableNode, getNode, setNode);
            else if (getNode != null)
                return new PropertyGetNode(variableNode, getNode);
            else
                return new PropertySetNode(variableNode, setNode);

            Node TryParseDescriber()
            {
                if (TryMatchCurrent(Seperator.BoxOpenBracket))
                    return ParseDescribers();
                else
                    return new DescriberNode();
            }

            Node ParseAccessorBody(ParseScopeType lambdaType)
            {
                var body = TryMatchCurrent(Seperator.Semicolon) ? new EmptyNode() : ParseBlock(lambdaType | ParseScopeType.Scope);

                index++;
                return body;
            }

            Node ParseSetNode(Node describer)
            {
                var setvalueDeclaration = new DeclarationNode(new DescriberNode(new DescriberKeywordNode(DescriberKeyword.Const)), typeNode, new IdentifierNode(new Identifier("value", 0)));
                return new SetNode(describer, setvalueDeclaration, ParseAccessorBody(ParseScopeType.LambdaStatement));
            }
        }

        private Node ParseConstructorCall(bool inExpression)
        {
            ForceMatchCurrent(Keyword.Create, true);
            var functionCall = ParseFunctionCall(true, true, ParseEntity(inExpression, SeperatorType.OpenBracket));

            if (MatchToken(Seperator.FlowerOpenBracket, LookAhead(), false, true))
                functionCall.AddChild(ParseExpressionList());

            return functionCall;
        }

        private Node ParseFunctionCall(bool inExpression, bool isContructor, Node functionName)
        {
            var arguments = new List<Node>();
            ForceMatchCurrent(Seperator.OpenBracket, true);

            if (!TryMatchCurrent(Seperator.CloseBracket))
            {
                while (index < tokens.Count)
                {
                    Node describer = null;
                    if (TryMatchCurrentType(TokenType.Keyword))
                    {
                        describer = new DescriberNode(new DescriberKeywordNode(Current as Keyword));
                        index++;
                    }

                    if(describer == null)
                        arguments.Add(new FunctionCallArgumentNode(ParseExpression(false, false, SeperatorType.Comma | SeperatorType.CloseBracket)));
                    else
                        arguments.Add(new FunctionCallArgumentNode(describer, ParseExpression(false, false, SeperatorType.Comma | SeperatorType.CloseBracket)));

                    if (TryMatchCurrent(Seperator.CloseBracket))
                        break;
                    index++;
                }
            }

            Node generic = null;
            if (LookAhead() == BinaryOperator.LesserThan)
                generic = ParseGenericCall(!inExpression);

            Node toReturn;
            if (isContructor)
                toReturn = new ConstructorCallNode(new FunctionNameCallNode(functionName), new FunctionCallArgumentsNode(arguments));
            else
                toReturn = new FunctionCallNode(new FunctionNameCallNode(functionName), new FunctionCallArgumentsNode(arguments));

            if (generic != null)
                toReturn.AddChild(generic);

            return toReturn;
        }

        private Node ParsePrintCall()
        {
            ForceMatchCurrent(Keyword.Print, true);
            ForceMatchCurrent(Seperator.OpenBracket, true);

            var argument = new FunctionCallArgumentNode(ParseExpression(false, false, SeperatorType.CloseBracket));
            return new PrintNode(new FunctionCallArgumentsNode(argument));
        }

        private Node ParseInputCall()
        {
            ForceMatchCurrent(Keyword.Input, true);
            ForceMatchCurrent(Seperator.OpenBracket, true);

            var argument = new FunctionCallArgumentNode(ParseExpression(true, false, SeperatorType.CloseBracket));
            return argument.NodeType == NodeType.Empty ? new InputNode(new FunctionCallArgumentsNode()) : new InputNode(new FunctionCallArgumentsNode(argument));
        }

        private Node ParseFunctionDeclaration(Node describer, TypeNode returnType)
        {
            ForceMatchCurrentType(TokenType.Identifier, false);
            Node name = new IdentifierNode((Identifier)Current);
            index++;

            var arguments = new FunctionDeclarationArgumentsNode(ParseDeclarationArguments());

            Node generic = null;
            if (MatchToken(BinaryOperator.LesserThan, LookAhead(), false, true))
                generic = ParseGenericDeclaration();

            index++;
            Node extensionTypeNode = null;
            if (TryMatchCurrent(Seperator.Colon, true))
            {
                if (returnType.Type == TypeNodeEnum.Constructor)
                    throw new TokenExpectedException(tokens[index - 1], Seperator.FlowerOpenBracket, tokens[index - 1].Index);

                extensionTypeNode = ParseType(SeperatorType.FlowerOpenBracket| SeperatorType.Lambda);
            }

            var type = returnType.Type == TypeNodeEnum.Constructor || returnType.Type == TypeNodeEnum.Void ? ParseScopeType.LambdaStatement : ParseScopeType.LambdaExpression;
            var body = ParseBlock(ParseScopeType.Scope | type);

            Node toReturn;
            if (returnType.Type == TypeNodeEnum.Constructor)
                toReturn = new ConstructorDeclarationNode(describer, returnType, arguments, body);
            else
            {
                if(extensionTypeNode == null)
                    toReturn = new FunctionDeclarationNode(describer, returnType, name, arguments, body);
                else
                    toReturn = new ExtensionFunctionDeclarationNode(describer, returnType, name, arguments, body, extensionTypeNode);
            }

            if (generic != null)
                toReturn.AddChild(generic);

            return toReturn;
        }

        private Node ParseOperatorOverload(Node describer)
        {
            ForceMatchCurrent(FunctionKeyword.Operator, true);
            ForceMatchCurrentType(TokenType.Identifier | TokenType.Keyword, false);
            var returnType = ParseType(SeperatorType.Any);

            index++;
            ForceMatchCurrentType(TokenType.Operator, false);
            var operatorToOverload = Current as Operator;

            index++;
            var arguments = new FunctionDeclarationArgumentsNode(ParseDeclarationArguments());

            Node generic = null;
            if (MatchToken(BinaryOperator.LesserThan, LookAhead(), false, true))
                generic = ParseGenericDeclaration();

            index++;
            var body = ParseBlock(ParseScopeType.Scope | ParseScopeType.LambdaExpression);
            var toReturn = new OperatorOverloadFunctionDeclarationLoad(describer, returnType, arguments, body, operatorToOverload);

            if (generic != null)
                toReturn.AddChild(generic);

            return toReturn;
        }

        private Node ParseConversionOverload(Node describer)
        {
            var keyword = Current;
            ForceMatchCurrentType(TokenType.Keyword, true);

            ForceMatchCurrentType(TokenType.Identifier | TokenType.Keyword, false);
            var returnType = ParseType(SeperatorType.OpenBracket);

            var arguments = new FunctionDeclarationArgumentsNode(ParseDeclarationArguments());

            Node generic = null;
            if (MatchToken(BinaryOperator.LesserThan, LookAhead(), false, true))
                generic = ParseGenericDeclaration();

            index++;
            var body = ParseBlock(ParseScopeType.Scope | ParseScopeType.LambdaExpression);

            Node toReturn;
            if (keyword == FunctionKeyword.Explicit)
                toReturn = new ExplicitCastDeclarationNode(describer, returnType, arguments, body);
            else
                toReturn = new ImplicitCastDeclarationNode(describer, returnType, arguments, body);

            if (generic != null)
                toReturn.AddChild(generic);

            return toReturn;
        }

        private Node ParseIndexerDeclaration(Node describer)
        {
            ForceMatchCurrent(FunctionKeyword.Indexer, true);
            ForceMatchCurrentType(TokenType.Identifier | TokenType.Keyword, false);
            var returnType = ParseType(SeperatorType.OpenBracket);

            var arguments = new FunctionDeclarationArgumentsNode(ParseDeclarationArguments());

            Node generic = null;
            if (MatchToken(BinaryOperator.LesserThan, LookAhead(), false, true))
                generic = ParseGenericDeclaration();

            index++;
            var body = ParseBlock(ParseScopeType.Scope | ParseScopeType.LambdaExpression);

            Node toReturn = new IndexerDeclarationNode(describer, returnType, arguments, body);

            if (generic != null)
                toReturn.AddChild(generic);

            return toReturn;
        }

        private List<Node> ParseDeclarationArguments()
        {
            ForceMatchCurrent(Seperator.OpenBracket, true);

            var arguments = new List<Node>();
            if (TryMatchCurrent(Seperator.CloseBracket))
                return arguments;

            while (index < tokens.Count)
            {
                Node describer = TryMatchCurrent(Seperator.BoxOpenBracket) ? ParseDescribers() : new DescriberNode();

                Node type;
                switch (Current.Type)
                {
                    case TokenType.Identifier:
                    case TokenType.Keyword:
                        type = ParseType(SeperatorType.Colon);
                        break;
                    default:
                        throw new TokenExpectedException(Current, "Type", Current.Index);
                }

                ForceMatchCurrent(Seperator.Colon, true);
                ForceMatchCurrentType(TokenType.Identifier, false);
                var variable = new IdentifierNode((Identifier)Current);

                Node defaultValue = null;
                if(LookAhead() == BinaryOperator.Assignment)
                {
                    index += 2;
                    defaultValue = ParseExpression(false, false, SeperatorType.Comma | SeperatorType.CloseBracket);

                    index--;
                }

                if (defaultValue == null)
                    arguments.Add(new FunctionDeclarationArgumentNode(describer, type, variable));
                else
                    arguments.Add(new FunctionDeclarationArgumentNode(describer, type, variable, defaultValue));

                index++;
                ForceMatchCurrentType(TokenType.Seperator);

                if (TryMatchCurrent(Seperator.Comma, true))
                    continue;

                ForceMatchCurrent(Seperator.CloseBracket, false);
                return arguments;
            }

            throw new TokenExpectedException(Seperator.CloseBracket, Final.Index);
        }

        //User Defined Types and Namespaces
        private Node ParseScopedEntity(Node describer, Keyword keyword) => keyword == EntityKeyword.Namespace ? ParseNamespaceDeclaration(describer) : ParseTypeDeclaration(describer, keyword);

        private Node ParseNamespaceDeclaration(Node describer)
        {
            var token = Current;

            var name = ParseEntity(false, SeperatorType.FlowerOpenBracket);
            var node = name;
            while (true)
                if (node.NodeType == NodeType.Dot)
                    node = ((DotExpression)node).RHS;
                else
                    break;

            if (node.NodeType != NodeType.Variable)
                throw new InvalidTokenException(token, token.Index);

            ForceMatchCurrent(Seperator.FlowerOpenBracket);
            return new NamespaceNode(describer, name, ParseScope());
        }

        private Node ParseTypeDeclaration(Node describer, Keyword keyword)
        {
            index++;
            ForceMatchCurrentType(TokenType.Identifier, false);
            var name = new IdentifierNode((Identifier)Current);

            Node generic = null;
            if (MatchToken(BinaryOperator.LesserThan, LookAhead(), false, true))
                generic = ParseGenericDeclaration();

            index++;
            Node inherits = null;
            if (TryMatchCurrent(Seperator.Colon))
                inherits = ParseInheritanceTypes();

            ForceMatchCurrent(Seperator.FlowerOpenBracket);
            var body = keyword == EntityKeyword.Enum ? ParseExpressionList() : ParseScope();

            Node toReturn;
            if (keyword == EntityKeyword.Class)
                toReturn = new ClassNode(describer, name, body);
            else if (keyword == EntityKeyword.Struct)
                toReturn = new StructNode(describer, name, body);
            else if (keyword == EntityKeyword.Enum)
                toReturn = new EnumNode(describer, name, body);
            else
                toReturn = new InterfaceNode(describer, name, body);

            if (generic != null)
                toReturn.AddChild(generic);
            if (inherits != null)
                toReturn.AddChild(inherits);

            return toReturn;
        }
    }
}