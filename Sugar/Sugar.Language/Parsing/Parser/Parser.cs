using System;
using System.Collections.Generic;

using Sugar.Language.Exceptions.Lexing;
using Sugar.Language.Exceptions.Parsing;

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
using Sugar.Language.Parsing.Nodes.Statements.VariableCreation.Delegates;
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
                throw new TokenExpectedException(expected, toMatch, toMatch.Index);

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

        private bool MatchBreakOutSeperator(Token seperator, SeperatorKind breakOutSeperators)
        {
            MatchType(TokenType.Seperator, seperator, true);

            var subType = (SeperatorKind)seperator.SyntaxKind;
            return (subType & breakOutSeperators) == subType;
        }

        private void ForceMatchBreakOutSeperator(SeperatorKind breakOutSeperators)
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

            return nodes.Count == 0 ? null : new SyntaxTree(new SugarFileGroupNode(nodes));
        }

        private Node ParseStatement() => TryMatchCurrent(Seperator.FlowerOpenBracket) ? ParseScope() : ParseStatement(true, SeperatorKind.Semicolon);

        private Node ParseStatement(bool readEmty, SeperatorKind breakOutSeperators)
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
                    return  new UnaryExpression(unaryOperator, ParseEntity(false, SeperatorKind.Semicolon));
                case TokenType.Seperator:
                    if (readEmty && MatchBreakOutSeperator(Current, breakOutSeperators))
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
            switch(current.SyntaxKind)
            {
                case SyntaxKind.Lambda:
                    index++;
                    var type = scopeType & ParseScopeType.Lambda;

                    if (type == ParseScopeType.LambdaStatement)
                        return new LambdaStatement(ParseStatement(false, SeperatorKind.Semicolon));
                    else if (type == ParseScopeType.LambdaExpression)
                        return new LambdaExpression(ParseExpression(false, false, SeperatorKind.Semicolon));
                    else
                        throw new InvalidTokenException(current, current.Index);
                case SyntaxKind.FlowerOpenBracket:
                    if ((scopeType & ParseScopeType.Scope) != ParseScopeType.Scope)
                        throw new InvalidTokenException(current, current.Index);

                    return ParseScope();
                default:
                    return ParseStatement(true, SeperatorKind.Semicolon);
            }
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

        private Node ParseDescriberStatement(SeperatorKind breakOutSeperators)
        {
            Node toReturn;
            DescriberNode describer = ParseDescribers();

            switch (Current.Type)
            {
                case TokenType.Identifier:
                    var type = ParseType(SeperatorKind.Colon);
                    index--;
                    toReturn = ParseDeclaration(describer, type);

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
                            switch(keyword.SyntaxKind)
                            {
                                case SyntaxKind.Var:
                                    index++;
                                    ForceMatchCurrent(Seperator.Colon, true);
                                    toReturn = ParseVariableDeclaration(new DescriberNode(), new AnonymousTypeNode());
                                    break;
                                case SyntaxKind.Action:
                                    toReturn = ParseActionDelegateDeclaration(new DescriberNode());
                                    break;
                                case SyntaxKind.Function:
                                    toReturn = ParseFunctionDelegateDeclaration(new DescriberNode());
                                    break;
                                default:
                                    throw new TokenExpectedException(keyword, "Type", keyword.Index);
                            }
                            break;
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

        private Node ParseKeyword(SeperatorKind breakOutSeperators)
        {
            Node toReturn;

            var current = Current;
            switch(current.SyntaxKind)
            {
                case SyntaxKind.Try:
                    return ParseTryCatchBlock();
                case SyntaxKind.Import:
                    toReturn = ParseImportStatement();
                    break;
                case SyntaxKind.Print:
                    toReturn = ParsePrintCall();
                    index++;
                    break;
                case SyntaxKind.Input:
                    toReturn = ParseInputCall();
                    index++;
                    break;
                case SyntaxKind.Throw:
                    index++;
                    toReturn = new ThrowExceptionNode(ParseExpression(false, false, SeperatorKind.Semicolon));
                    break;
                case SyntaxKind.Var:
                    index++;
                    ForceMatchCurrent(Seperator.Colon, true);
                    toReturn = ParseVariableDeclaration(new DescriberNode(), new AnonymousTypeNode());
                    break;
                case SyntaxKind.Create:
                    toReturn = ParseConstructorCall(false);
                    index++;
                    break;
                case SyntaxKind.Return:
                    toReturn = ParseReturn();
                    break;
                case SyntaxKind.Break:
                    toReturn = new BreakNode();
                    index++;
                    break;
                case SyntaxKind.Continue:
                    toReturn = new ContinueNode();
                    index++;
                    break;
                case SyntaxKind.For:
                    return ParseForLoop();
                case SyntaxKind.While:
                    return ParseWhileLoop();
                case SyntaxKind.Foreach:
                    return ParseForeachLoop();
                case SyntaxKind.Do:
                    return ParseDoWhileLoop();
                case SyntaxKind.If:
                    return ParseIfCondition();
                case SyntaxKind.Switch:
                    return ParseSwitchCase();
                case SyntaxKind.Action:
                    toReturn = ParseActionDelegateDeclaration(new DescriberNode());
                    break;
                case SyntaxKind.Function:
                    toReturn = ParseFunctionDelegateDeclaration(new DescriberNode());
                    break;
                default:
                    switch (((Keyword)current).KeywordType)
                    {
                        case KeywordType.Entity:
                            return ParseScopedEntity(new DescriberNode(), (Keyword)current);
                        case KeywordType.Type:
                            toReturn = ParseDeclaration(new DescriberNode());

                            if (MatchFunctionPropertyDeclaration(toReturn.NodeType == NodeType.FunctionDeclaration))
                                return toReturn;
                            break;
                        case KeywordType.Function:
                            return ParseFunctionKeyword(new DescriberNode());
                        default:
                            throw new InvalidTokenException(current, current.Index);
                    }
                    break;
            }

            ForceMatchBreakOutSeperator(breakOutSeperators);
            return toReturn;
        }

        private Node ParseFunctionKeyword(Node describer)
        {
            switch(Current.SyntaxKind)
            {
                case SyntaxKind.Operator:
                    return ParseOperatorOverload(describer);
                case SyntaxKind.Indexer:
                    return ParseIndexerDeclaration(describer);
                case SyntaxKind.Void:
                    index++;
                    return ParseFunctionDeclaration(describer, new VoidNode());
                case SyntaxKind.Constructor:
                    index++;
                    ForceMatchCurrentType(TokenType.Identifier);

                    return ParseFunctionDeclaration(describer, new ConstructorTypeNode(new CustomTypeNode(new IdentifierNode((Identifier)Current))));
                default:
                    return ParseConversionOverload(describer);
            }
        }

        private Node ParseIdentifier(SeperatorKind breakOutSeperators)
        {
            var variable = ParseEntity(false, SeperatorKind.Any);
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
                    switch(next.Type)
                    {
                        case TokenType.UnaryOperator:
                            toReturn = new UnaryExpression((UnaryOperator)next, variable);
                            index += 2;
                            break;
                        case TokenType.AssignmentOperator:
                            index++;
                            toReturn = ParseVariableAssignment(variable, next as AssignmentOperator, breakOutSeperators);
                            break;
                        default:
                            toReturn = ParseDeclaration(new DescriberNode(), new CustomTypeNode(variable));

                            if (MatchFunctionPropertyDeclaration(toReturn.NodeType == NodeType.FunctionDeclaration))
                                return toReturn;
                            break;
                    }
                    break;
            }

            ForceMatchBreakOutSeperator(breakOutSeperators);
            return toReturn;
        }

        private bool MatchFunctionPropertyDeclaration(bool isFunction) => isFunction || (!isFunction && TryMatchCurrent(Seperator.FlowerCloseBracket, false));

        private Node ParseDeclaration(DescriberNode describer) => ParseDeclaration(describer, ParseType(SeperatorKind.Any));

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
                    return new CompoundVariableDeclarationNode(assignNodes);
            }
        }

        private Node ParseVariableInitialisation(DescriberNode describer, TypeNode _type, Node _variableNode, bool isProperty)
        {
            var next = LookAhead();
            if (next != AssignmentOperator.Assignment)
            {
                if (isProperty || !isProperty && next == Seperator.Comma || next == Seperator.Semicolon)
                {
                    if (!isProperty)
                        index++;

                    return new DeclarationNode(describer, _type, _variableNode);
                }

                throw new TokenExpectedException(AssignmentOperator.Assignment, next.Index);
            }

            index += 2;
            return new InitializeNode(describer, _type, _variableNode, ParseExpression(false, false, SeperatorKind.Semicolon | (isProperty ? SeperatorKind.Any : SeperatorKind.Comma)));
        }

        private Node ParseVariableAssignment(Node variable, AssignmentOperator assignmentOperator, SeperatorKind breakOutSeperators)
        {
            index++;
            var expresssion = ParseExpression(false, true, breakOutSeperators);

            if (Current.Type == TokenType.AssignmentOperator)
            {
                ForceMatchCurrent(AssignmentOperator.Assignment);

                var node = expresssion;
                while (true)
                    if (node.NodeType == NodeType.Dot)
                        node = ((DotExpression)node).RHS;
                    else
                        break;

                if (node.NodeType != NodeType.Variable)
                    throw new InvalidStatementException(Current.Index);
                
                expresssion = ParseVariableAssignment(expresssion, AssignmentOperator.Assignment, breakOutSeperators);
            }

            expresssion = assignmentOperator.BaseOperator == null ? expresssion : new BinaryExpression(assignmentOperator.BaseOperator, variable, expresssion);
            return new AssignmentNode(variable, expresssion);
        }

        private Node ParseImportStatement()
        {
            index++;
            UDDataType entityType = UDDataType.Namespace;

            var current = Current;
            switch(current.SyntaxKind)
            {
                case SyntaxKind.Class:
                    entityType = UDDataType.Class;
                    break;
                case SyntaxKind.String:
                    entityType = UDDataType.Struct;
                    break;
                case SyntaxKind.Interface:
                    entityType = UDDataType.Interface;
                    break;
                case SyntaxKind.Enum:
                    entityType = UDDataType.Enum;
                    break;
            }

            if (entityType != UDDataType.Namespace)
                index++;

            ForceMatchCurrentType(TokenType.Identifier);
            return new ImportNode(entityType, ParseEntity(false, SeperatorKind.Semicolon));
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

        private Node ParseActionDelegateDeclaration(Node describer)
        {
            ForceMatchCurrent(Keyword.Action, true);
            ForceMatchCurrent(Seperator.Colon, true);

            ForceMatchCurrentType(TokenType.Identifier, false);
            var name = new IdentifierNode((Identifier)Current);

            index++;
            ForceMatchCurrent(AssignmentOperator.Assignment, true);

            var arguments = new FunctionDeclarationArgumentsNode(ParseDeclarationArguments());
            index++;

            ForceMatchCurrent(Seperator.Lambda, true);
            var body = ParseBlock(ParseScopeType.Scope);

            index++;
            return new ActionDelegateNode(describer, name, arguments, body);
        }

        private Node ParseFunctionDelegateDeclaration(Node describer)
        {
            ForceMatchCurrent(Keyword.Function);
            var type = new FunctionTypeNode(ParseGenericCall(true));

            index++;
            ForceMatchCurrent(Seperator.Colon, true);

            ForceMatchCurrentType(TokenType.Identifier, false);
            var name = new IdentifierNode((Identifier)Current);

            index++;
            ForceMatchCurrent(AssignmentOperator.Assignment, true);

            var arguments = new FunctionDeclarationArgumentsNode(ParseDeclarationArguments());
            index++;

            ForceMatchCurrent(Seperator.Lambda, true);
            var body = ParseBlock(ParseScopeType.Scope);

            index++;
            return new FunctionDelegateNode(describer, type, name, arguments, body);
        }

        //Expressions and Entities
        private Node ParseExpression(bool readEmpty, bool readAssignment, SeperatorKind breakOutSeperators)
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

                        switch (current.SyntaxKind)
                        {
                            case SyntaxKind.Create:
                                output.Add(ParseConstructorCall(true));
                                break;
                            case SyntaxKind.This:
                                output.Add(ParseEntity(true, SeperatorKind.Any));
                                break;
                            case SyntaxKind.Input:
                                output.Add(ParseInputCall());
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
                                    output.Add(ParseEntity(true, SeperatorKind.Any));
                                    break;
                                }
                                throw new InvalidTokenException(current, current.Index);
                        }

                        expected = TokenType.Seperator | TokenType.Operator;
                        break;
                    case TokenType.Constant:
                        MatchType(expected, current, true);

                        output.Add(ParseEntity(true, SeperatorKind.Any));
                        expected = TokenType.Seperator | TokenType.BinaryOperator;
                        break;
                    case TokenType.Identifier:
                        MatchType(expected, current, true);

                        output.Add(ParseEntity(true, SeperatorKind.Any));
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
                            output.Add(ParseExpression(false, false, SeperatorKind.CloseBracket));
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
                            case TokenType.AssignmentOperator:
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

            var trueExpression = ParseExpression(false, false, SeperatorKind.Colon);

            index++;
            var falseExpression = ParseExpression(false, false, SeperatorKind.Semicolon | SeperatorKind.CloseBracket);

            index--;
            return new TernaryExpression(condition, trueExpression, falseExpression);
        }

        private IndexerExpression ParseIndexer(Node operhand)
        {
            index++;
            var arguments = new List<Node>();
            while (index < tokens.Count)
            {
                arguments.Add(ParseExpression(false, false, SeperatorKind.BoxCloseBracket | SeperatorKind.Comma));

                if (TryMatchCurrent(Seperator.BoxCloseBracket))
                    break;

                ForceMatchCurrent(Seperator.Comma, true);
            }

            ForceMatchCurrent(Seperator.BoxCloseBracket);
            return new IndexerExpression(operhand, new IndexerArgumentsNode(arguments));
        }

        private Node ParseReturn()
        {
            ForceMatchCurrent(ControlKeyword.Return, true);

            if (TryMatchCurrent(Seperator.Semicolon))
                return new ReturnKeyword();
            else
                return new ReturnKeyword(ParseExpression(false, false, SeperatorKind.Semicolon));
        }

        private Node ParseDefault()
        {
            ForceMatchCurrent(Keyword.Default, false);

            if(MatchToken(Seperator.OpenBracket, LookAhead()))
            {
                index += 2;
                return new DefaultTypeNode(ParseType(SeperatorKind.CloseBracket));
            }

            return new DefaultValueNode();
        }

        private Node ParseAsType()
        {
            ForceMatchCurrent(Keyword.AsType, true);
            ForceMatchCurrent(Seperator.OpenBracket, true);

            return new AsTypeNode(ParseType(SeperatorKind.CloseBracket));
        }

        private Node ParseExpressionList(SeperatorKind finalBreakOut)
        {
            var nodes = new List<Node>();

            while(index < tokens.Count)
            {
                if (TryMatchCurrent(Seperator.FlowerCloseBracket))
                    return Extract();

                var expression = ParseExpression(false, true, SeperatorKind.Comma | finalBreakOut);
                if (TryMatchCurrent(AssignmentOperator.Assignment, true))
                    expression = new AssignmentNode(expression, ParseExpression(false, false, SeperatorKind.Comma | finalBreakOut));

                nodes.Add(expression);

                ForceMatchCurrentType(TokenType.Seperator);
                if (TryMatchCurrent(Seperator.Comma, true))
                    continue;

                ForceMatchBreakOutSeperator(finalBreakOut);
                return Extract();
            }

            throw new TokenExpectedException(Seperator.FlowerCloseBracket, Final.Index);
            Node Extract()
            {
                if (nodes.Count == 0)
                    return new EmptyNode();
                else
                    return new ExpressionListNode(nodes);
            }
        }

        private Node ParseEntity(bool inExpression, SeperatorKind breakOutSeperators)
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

                        switch (current.SyntaxKind)
                        {
                            case SyntaxKind.Dot:
                                stack.Add(BinaryOperator.DotOperator);
                                expected = TokenType.Identifier;
                                break;
                            case SyntaxKind.BoxOpenBracket:
                                output.Add(ParseIndexer(output[output.Count - 1]));
                                CleanUp();
                                break;
                            case SyntaxKind.OpenBracket:
                                output.Add(ParseFunctionCall(inExpression, false, output[output.Count - 1]));
                                CleanUp();
                                break;
                            default:
                                breakOut = subtract = true;
                                break;
                        }

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

                        switch(current.SyntaxKind)
                        {
                            case SyntaxKind.This:
                                output.Add(new ThisNode());
                                expected = TokenType.Seperator;
                                break;
                            case SyntaxKind.Array:
                                if (inExpression)
                                    output.Add(new TypeKeywordNode(TypeKeyword.Array));
                                else
                                    output.Add(new ArrayTypeNode(ParseGenericCall(true)));

                                expected = TokenType.Seperator;
                                break;
                            default:
                                var keyword = (Keyword)current;
                                if (keyword.KeywordType == KeywordType.Type)
                                {
                                    output.Add(new TypeKeywordNode(keyword));
                                    expected = TokenType.Seperator;
                                    break;
                                }

                                throw new InvalidTokenException(current, current.Index);
                        }
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

            if (output.Count == 0)
                throw new TokenExpectedException(expected, Current.Index);

            return output[0];
        }

        private Node ParseVariable(SeperatorKind breakOutSeperators)
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

        private TypeNode ParseType(SeperatorKind breakOutSeperators)
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
                {
                    var dot = (DotExpression)node;

                    switch(dot.LHS.NodeType)
                    {
                        case NodeType.Type:
                        case NodeType.Variable:
                            break;
                        default:
                            throw new InvalidTokenException(token, token.Index);
                    }

                    node = dot.RHS;
                }
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
                types.Add(new InheritsTypeNode(ParseType(SeperatorKind.Comma)));

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

                        switch(current.SyntaxKind)
                        {
                            case SyntaxKind.LesserThan:
                                output.Add(new GenericCallNode());
                                expected = TokenType.Identifier | TokenType.Keyword;
                                break;
                            case SyntaxKind.GreaterThan:
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
                            default:
                                return new ParseGenericResult(null, new Exception("Binary Operator detected but not expected"));
                        }
                        break;
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
            var initialise = ParseMultiLine(Seperator.Semicolon, SeperatorKind.Semicolon | SeperatorKind.Semicolon | SeperatorKind.Comma);

            var expression = ParseExpression(true, false, SeperatorKind.Semicolon);
            index++;
            var increment = ParseMultiLine(Seperator.CloseBracket, SeperatorKind.CloseBracket | SeperatorKind.Comma);

            var body = ParseBlock(ParseScopeType.Scope | ParseScopeType.SingleLine);
            return new ForLoopNode(initialise, expression, increment, body);

            Node ParseMultiLine(Seperator finalBreakOut, SeperatorKind breakOutSeperators)
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

            var type = ParseType(SeperatorKind.Colon);
            ForceMatchCurrent(Seperator.Colon, true);

            ForceMatchCurrentType(TokenType.Identifier);
            var variable = new IdentifierNode((Identifier)Current);
            index++;

            ForceMatchCurrent(DescriberKeyword.In, true);
            var array = ParseVariable(SeperatorKind.CloseBracket);
            index++;

            var body = ParseBlock(ParseScopeType.Scope | ParseScopeType.SingleLine);

            return new ForeachLoopNode(new DeclarationNode(new DescriberNode(), type, variable), array, body);
        }

        private Node ParseWhileLoop()
        {
            ForceMatchCurrent(LoopKeyword.While, true);
            ForceMatchCurrent(Seperator.OpenBracket, true);
            var condition = ParseExpression(false, false, SeperatorKind.CloseBracket);

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

            var condition = ParseExpression(false, false, SeperatorKind.CloseBracket);
            return new DoWhileNode(condition, body);
        }

        //Conditions
        private Node ParseIfCondition()
        {
            var nodes = new List<Node>();

            bool breakOut = false;
            while (index < tokens.Count)
            {
                var current = Current;
                switch(current.SyntaxKind)
                {
                    case SyntaxKind.If:
                        index++;
                        ForceMatchCurrent(Seperator.OpenBracket, true);

                        var expression = ParseExpression(false, false, SeperatorKind.CloseBracket);
                        index++;
                        var body = ParseBlock(ParseScopeType.Scope | ParseScopeType.SingleLine);

                        nodes.Add(new IfNode(expression, body));
                        break;
                    case SyntaxKind.Else:
                        index++;
                        if (TryMatchCurrent(ConditionKeyword.If))
                            continue;

                        breakOut = true;
                        nodes.Add(new ElseNode(ParseBlock(ParseScopeType.Scope | ParseScopeType.SingleLine)));
                        break;
                    default:
                        breakOut = true;
                        break;

                }

                if (breakOut)
                    break;

                index++;
            }

            return nodes.Count == 1 ? nodes[0] : new IfElseChainNode(nodes);
        }

        private Node ParseSwitchCase()
        {
            ForceMatchCurrent(ConditionKeyword.Switch, true);
            ForceMatchCurrent(Seperator.OpenBracket, true);

            var valueToCheck = ParseExpression(false, false, SeperatorKind.CloseBracket);
            index++;

            ForceMatchCurrent(Seperator.FlowerOpenBracket, true);

            var cases = new List<Node>();
            while (index < tokens.Count)
            {
                var current = Current;
                if (MatchToken(Seperator.FlowerCloseBracket, current))
                    break;

                switch(current.SyntaxKind)
                {
                    case SyntaxKind.Case:
                        index++;

                        Node value;
                        current = Current;
                        switch (current.Type)
                        {
                            case TokenType.Identifier:
                                value = ParseEntity(false, SeperatorKind.Colon);

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
                                var type = ParseType(SeperatorKind.Colon);

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
                        break;
                    case SyntaxKind.Default:
                        index++;

                        ForceMatchCurrent(Seperator.Colon, true);

                        cases.Add(new DefaultNode(ParseBody(), ParseControlStatement()));
                        index++;

                        ForceMatchCurrent(Seperator.FlowerCloseBracket);
                        continue;
                    default:
                        throw new InvalidTokenException(current, current.Index);
                }

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
                var expression = ParseExpression(false, false, SeperatorKind.Colon);

                return new WhenNode(new DeclarationNode(new DescriberNode(new DescriberKeywordNode(DescriberKeyword.Const)), typeNode, variable), expression);
            }
        }

        //Functions and Properties
        private Node ParseProperty(Node typeNode, Node variableNode)
        {
            index++;
            ForceMatchCurrent(Seperator.FlowerOpenBracket, true);

            Node getNode = null, setNode = null;

            Node describer = TryParseDescriber();
            if (describer == null)
                describer = new DescriberNode();

            Console.WriteLine(describer == null);
            if (TryMatchCurrent(Keyword.Get, true))
            {
                getNode = new GetNode(describer, ParseAccessorBody(ParseScopeType.Scope | ParseScopeType.LambdaExpression));
                describer = TryParseDescriber();
            }

            if (describer != null)
            {  
                ForceMatchCurrent(Keyword.Set, true);
                setNode = ParseSetNode(describer);
            }
            else if(TryMatchCurrent(Keyword.Set, true))
                    setNode = ParseSetNode(new DescriberNode());

            ForceMatchCurrent(Seperator.FlowerCloseBracket);

            if (getNode != null && setNode != null)
                return new PropertyGetSetNode(variableNode, typeNode, getNode, setNode);
            else if (getNode != null)
                return new PropertyGetNode(variableNode, typeNode, getNode);
            else
                return new PropertySetNode(variableNode, typeNode, setNode);

            Node TryParseDescriber()
            {
                if (TryMatchCurrent(Seperator.BoxOpenBracket))
                    return ParseDescribers();
                else
                    return null;
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
            var functionCall = ParseFunctionCall(true, true, ParseEntity(inExpression, SeperatorKind.OpenBracket));

            if (MatchToken(Seperator.FlowerOpenBracket, LookAhead(), false, true))
            {
                index++;
                functionCall.AddChild(ParseExpressionList(SeperatorKind.FlowerCloseBracket));
            }

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

                        ForceMatchCurrent(Seperator.Colon, true);
                    }

                    if (describer == null)
                        arguments.Add(new FunctionCallArgumentNode(ParseExpression(false, false, SeperatorKind.Comma | SeperatorKind.CloseBracket)));
                    else
                        arguments.Add(new FunctionCallArgumentNode(describer, ParseEntity(false, SeperatorKind.Comma | SeperatorKind.CloseBracket)));

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

            var argument = new FunctionCallArgumentNode(ParseExpression(false, false, SeperatorKind.CloseBracket));
            return new PrintNode(new FunctionCallArgumentsNode(argument));
        }

        private Node ParseInputCall()
        {
            ForceMatchCurrent(Keyword.Input, true);
            ForceMatchCurrent(Seperator.OpenBracket, true);

            var argument = new FunctionCallArgumentNode(ParseExpression(true, false, SeperatorKind.CloseBracket));
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
                {
                    ForceMatchCurrent(Keyword.Parent, true);
                    ForceMatchCurrent(Seperator.OpenBracket, true);
                    extensionTypeNode = new ParentFunctionArgumentNode(ParseExpressionList(SeperatorKind.CloseBracket));

                    index++;
                }
                else
                    extensionTypeNode = ParseType(SeperatorKind.FlowerOpenBracket| SeperatorKind.Lambda);
            }

            var type = returnType.Type == TypeNodeEnum.Constructor || returnType.Type == TypeNodeEnum.Void ? ParseScopeType.LambdaStatement : ParseScopeType.LambdaExpression;
            var body = ParseBlock(ParseScopeType.Scope | type);

            Node toReturn;
            if (returnType.Type == TypeNodeEnum.Constructor)
            {
                if(extensionTypeNode == null)
                    toReturn = new ConstructorDeclarationNode(describer, returnType, arguments, body);
                else
                    toReturn = new ConstructorDeclarationNode(describer, returnType, arguments, body, extensionTypeNode);
            }
            else
            {
                if (extensionTypeNode == null)
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
            var returnType = ParseType(SeperatorKind.Any);

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
            var toReturn = new OperatorOverloadFunctionDeclarationNode(describer, returnType, arguments, body, operatorToOverload);

            if (generic != null)
                toReturn.AddChild(generic);

            return toReturn;
        }

        private Node ParseConversionOverload(Node describer)
        {
            var keyword = Current;
            ForceMatchCurrentType(TokenType.Keyword, true);

            ForceMatchCurrentType(TokenType.Identifier | TokenType.Keyword, false);
            var returnType = ParseType(SeperatorKind.OpenBracket);

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
            var returnType = ParseType(SeperatorKind.OpenBracket);

            var arguments = new FunctionDeclarationArgumentsNode(ParseDeclarationArguments());

            Node generic = null;
            if (MatchToken(BinaryOperator.LesserThan, LookAhead(), false, true))
                generic = ParseGenericDeclaration();

            var body = ParseProperty(returnType, returnType);

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
                        type = ParseType(SeperatorKind.Colon);
                        break;
                    default:
                        throw new TokenExpectedException(Current, "Type", Current.Index);
                }

                ForceMatchCurrent(Seperator.Colon, true);
                ForceMatchCurrentType(TokenType.Identifier, false);
                var variable = new IdentifierNode((Identifier)Current);

                Node defaultValue = null;
                if(LookAhead() == AssignmentOperator.Assignment)
                {
                    index += 2;
                    defaultValue = ParseExpression(false, false, SeperatorKind.Comma | SeperatorKind.CloseBracket);

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

            index++;
            var name = ParseEntity(false, SeperatorKind.FlowerOpenBracket);
            var node = name;
            while (true)
                if (node.NodeType == NodeType.Dot)
                {
                    var dot = (DotExpression)node;

                    if(dot.LHS.NodeType != NodeType.Variable)
                        throw new InvalidTokenException(token, token.Index);

                    node = dot.RHS;
                }
                else
                    break;

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

            Node body;
            if(keyword == EntityKeyword.Enum)
            {
                ForceMatchCurrent(Seperator.FlowerOpenBracket, true);
                body = ParseExpressionList(SeperatorKind.FlowerCloseBracket);
            }
            else
                body = ParseScope();

            Node toReturn = keyword.SyntaxKind switch
            {
                SyntaxKind.Enum => new EnumNode(describer, name, body),
                SyntaxKind.Class => new ClassNode(describer, name, body),
                SyntaxKind.Struct => new StructNode(describer, name, body),
                SyntaxKind.Interface => new InterfaceNode(describer, name, body),
                _ => throw new InvalidTokenException(keyword, keyword.Index)
            };

            if (generic != null)
                toReturn.AddChild(generic);
            if (inherits != null)
                toReturn.AddChild(inherits);

            return toReturn;
        }
    }
}