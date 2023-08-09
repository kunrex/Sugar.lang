using System;
using System.Collections.Generic;

using Sugar.Language.Services;

using Sugar.Language.Exceptions;
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
using Sugar.Language.Parsing.Nodes.InvalidNodes;
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
using Sugar.Language.Parsing.Nodes.InvalidNodes.Structure;
using Sugar.Language.Parsing.Nodes.Statements.VariableCreation;
using Sugar.Language.Parsing.Nodes.Functions.Calling.Structure;
using Sugar.Language.Parsing.Nodes.Conditions.SwitchConditions;
using Sugar.Language.Parsing.Nodes.Values.Generics.Declarations;
using Sugar.Language.Parsing.Nodes.Functions.Properties.Accessors;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;
using Sugar.Language.Parsing.Nodes.Statements.VariableCreation.Delegates;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.OperatorOverloading;

//Inlcusive: Ends at the mentioned character, does not skip over it.
//Exclusive: Ends at one index, after the expected character, does skip over it.

namespace Sugar.Language.Parsing.Parser
{
    internal sealed class Parser : SingletonService<Parser>
    {
        private int index;

        private SugarFile sourceFile;
        private Token Current { get => sourceFile[index]; }
        private Token Final { get => sourceFile[sourceFile.TokenCount - 1]; }

        /// <summary>
        /// Returns the next Token. Returns `null` if none exists.
        /// </summary>
        private Token LookAhead()
        {
            if (index + 1 >= sourceFile.TokenCount)
                return null;

            return sourceFile[index + 1];
        }


        /// <summary>
        /// Throws an exception when the `current token` is not an expected one.
        /// </summary>
        private void ForceMatchCurrentType(TokenType expected, bool increment = false)
        {
            if (index >= sourceFile.TokenCount)
                throw new TokenExpectedException(expected, Final);

            if (!MatchType(expected, Current, increment))
                throw new TokenExpectedException(expected, Current);
        }

        /// <summary>
        /// Pushes an exception when the `current token` is not an expected one. Returns weather or not the expected token was found.
        /// </summary>
        private bool TryMatchCurrentType(TokenType expected, bool increment = false)
        {
            if (index >= sourceFile.TokenCount)
            {
                sourceFile.PushException(new TokenExpectedException(expected, Final));
                return false;
            }

            if (!MatchCurrentType(expected, increment))
            {
                sourceFile.PushException(new TokenExpectedException(expected, Current));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns weather or not the curent token is an expected token.
        /// </summary>
        private bool MatchCurrentType(TokenType expected, bool increment = false)
        {
            if (index >= sourceFile.TokenCount)
                return false;

            return MatchType(expected, Current, increment);
        }

        /// <summary>
        /// Matches a token to a certain expected type.
        /// </summary>
        private bool MatchType(TokenType expected, Token toMatch, bool increment = false)
        {
            if (toMatch != null && (expected & toMatch.Type) == toMatch.Type)
            {
                if (increment)
                    index++;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Throws an exception if the `current token` does not match a certain token.
        /// </summary>
        private void ForceMatchCurrent(Token matchTo, bool increment = false)
        {
            if (index >= sourceFile.TokenCount)
                throw new TokenExpectedException(matchTo, Final);

            if(!MatchCurrent(matchTo, increment))
                throw new TokenExpectedException(matchTo, Current);
        }

        /// <summary>
        /// Pushes an exception if the `current token` does not match a certain token. Returns weather or not it was a match.
        /// </summary>
        private bool TryMatchCurrent(Token matchTo, bool increment = false)
        {
            if (index >= sourceFile.TokenCount)
            {
                sourceFile.PushException(new TokenExpectedException(matchTo, Final));
                return false;
            }

            if(!MatchCurrent(matchTo, increment))
            {
                sourceFile.PushException(new TokenExpectedException(matchTo, Current));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns weather or not the `current token` is a certain token.
        /// </summary>
        private bool MatchCurrent(Token matchTo, bool increment = false)
        {
            if (index >= sourceFile.TokenCount)
                return false;

            return MatchToken(matchTo, Current, increment);
        }

        /// <summary>
        /// Pushes an exception if a certain token
        /// (<paramref name="matchTo"/>)
        /// does not match a certain base token
        /// (<paramref name="toMatch"/>). Returns weather or not it was a match.
        /// </summary>
        private bool TryMatchToken(Token matchTo, Token toMatch, bool increment = false)
        {
            if (index >= sourceFile.TokenCount)
            {
                sourceFile.PushException(new TokenExpectedException(matchTo, Final));
                return false;
            }

            if (!MatchToken(matchTo, toMatch, increment))
            {
                sourceFile.PushException(new TokenExpectedException(matchTo, toMatch));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns if a certain token
        /// (<paramref name="matchTo"/>)
        /// does match a certain base token
        /// (<paramref name="toMatch"/>).
        /// </summary>
        private bool MatchToken(Token matchTo, Token toMatch, bool increment = false)
        {
            if(toMatch == matchTo)
            {
                if (increment)
                    index++;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Pushes an exception if the `current seperator` matches a certain expected seperator. Returns weather or not the match was found.
        /// </summary>
        private bool TryMatchBreakOutSeperator(SeperatorKind breakOutSeperators)
        {
            if (index >= sourceFile.TokenCount)
            {
                sourceFile.PushException(new TokenExpectedException($"{breakOutSeperators}", Final));
                return false;
            }

            if (!MatchBreakOutSeperator(Current, breakOutSeperators))
            {
                sourceFile.PushException(new TokenExpectedException($"{breakOutSeperators}", Current));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns weather or a certain token matches a certain seperator was found.
        /// </summary>
        private bool MatchBreakOutSeperator(Token seperator, SeperatorKind breakOutSeperators)
        {
            if (!MatchType(TokenType.Seperator, seperator))
                return false;

            var subType = (SeperatorKind)seperator.SyntaxKind;
            return (subType & breakOutSeperators) == subType;
        }

        public void Parse(SugarFile _source)
        {
            sourceFile = _source;

            var nodes = new List<Node>();
            for (index = 0; index < sourceFile.TokenCount; index++)
            {
                var toAdd = ParseStatement();

                Console.WriteLine(sourceFile.Exceptions.Count);

                if (toAdd.NodeType != NodeType.Empty)
                    nodes.Add(toAdd);
            }

            if (nodes.Count == 0)
                sourceFile.WithSyntaxTree(new SyntaxTree(new EmptyNode()));
            else
                sourceFile.WithSyntaxTree(new SyntaxTree(new SugarFileGroupNode(nodes)));
        }

        private Node ParseStatement() => ParseStatement(StatementEnum.Scope | StatementEnum.GeneralStatement);

        private Node ParseStatement(StatementEnum statementType)
        {
            var current = Current;

            switch(current.SyntaxKind)
            {
                case SyntaxKind.FlowerOpenBracket:
                    if (!Match(StatementEnum.Scope))
                        return ReturnInvalidTokenNode(SeperatorKind.Any);

                    return ParseScope();
                case SyntaxKind.Lambda:
                    index++;
                    if (Match(StatementEnum.LambdaStatement))
                        return ParseStatement(false, SeperatorKind.Semicolon);
                    else if (Match(StatementEnum.LambdaExpression))
                        return new LambdaExpression(ParseExpression(false, false, SeperatorKind.Semicolon));
                    else
                        return ReturnInvalidTokenNode(SeperatorKind.Any);
                default:
                    if(!Match(StatementEnum.StandaloneStatement))
                        return ReturnInvalidTokenNode(SeperatorKind.Any);

                    return ParseStatement(Match(StatementEnum.EmptyStatement), SeperatorKind.Semicolon);
            }

            bool Match(StatementEnum match) => (statementType & match) == match;            
        }

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
                    return new UnaryExpression(unaryOperator, ParseEntity(false, SeperatorKind.Semicolon));
                case TokenType.Seperator:
                    if (readEmty && MatchBreakOutSeperator(Current, breakOutSeperators))
                        return new EmptyNode();
                    else if (TryMatchCurrent(Seperator.BoxOpenBracket))
                        return ParseDescriberStatement(breakOutSeperators);
                    else
                        return ReturnInvalidTokenNode(breakOutSeperators);
                default:
                    return ReturnInvalidTokenNode(breakOutSeperators);
            }
        }

        private Node ParseScope()
        {
            var nodes = new List<Node>();
            ForceMatchCurrent(Seperator.FlowerOpenBracket, true);

            for (; index < sourceFile.TokenCount; index++)
            {
                if (MatchCurrent(Seperator.FlowerCloseBracket))
                    break;

                var stmt = ParseStatement();
                if (stmt.NodeType != NodeType.Empty)
                    nodes.Add(stmt);
            }

            TryMatchCurrent(Seperator.FlowerCloseBracket);
            switch (nodes.Count)
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

                    if (MatchFunctionPropertyDeclaration(toReturn))
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
                            switch (keyword.SyntaxKind)
                            {
                                case SyntaxKind.Var:
                                    index++;
                                    TryMatchCurrent(Seperator.Colon, true);
                                    toReturn = ParseVariableDeclaration(new DescriberNode(), new AnonymousTypeNode());
                                    break;
                                case SyntaxKind.Action:
                                    toReturn = ParseActionDelegateDeclaration(new DescriberNode());
                                    break;
                                case SyntaxKind.Function:
                                    toReturn = ParseFunctionDelegateDeclaration(new DescriberNode());
                                    break;
                                default:
                                    return ReturnInvalidTokenNode(breakOutSeperators);
                            }
                            break;
                        case KeywordType.Type:
                            toReturn = ParseDeclaration(describer);

                            if (MatchFunctionPropertyDeclaration(toReturn))
                                return toReturn;
                            break;
                        default:
                            return ReturnInvalidTokenNode(breakOutSeperators);
                    }
                    break;
                default:
                    return ReturnInvalidTokenNode(breakOutSeperators);
            }

            TryMatchBreakOutSeperator(breakOutSeperators);
            return toReturn;
        }

        private DescriberNode ParseDescribers()
        {
            var describers = new List<Node>();
            ForceMatchCurrent(Seperator.BoxOpenBracket, true);

            while (index < sourceFile.TokenCount)
            {
                var current = Current;

                if(MatchType(TokenType.Keyword, current))
                {
                    var keyword = (Keyword)Current;

                    if (keyword.KeywordType == KeywordType.Describer)
                        describers.Add(new DescriberKeywordNode(Current as Keyword));
                    else
                        describers.Add(PushInvalidToken(current));
                }
                else
                    describers.Add(PanicModeSimple(PushInvalidTokenException(current), SeperatorKind.BoxCloseBracket));

                index++;
                if (MatchCurrent(Seperator.Comma, true))
                    continue;
                else if (MatchCurrent(Seperator.BoxCloseBracket, true))
                {
                    if (MatchCurrent(Seperator.BoxOpenBracket, true))
                        continue;

                    break;
                }
            }

            return new DescriberNode(describers);
        }

        private Node ParseKeyword(SeperatorKind breakOutSeperators)
        {
            Node toReturn;

            var current = Current;
            switch (current.SyntaxKind)
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

                            if (MatchFunctionPropertyDeclaration(toReturn))
                                return toReturn;
                            break;
                        case KeywordType.Function:
                            return ParseFunctionKeyword(new DescriberNode());
                        default:
                            return ReturnInvalidTokenNode(breakOutSeperators);
                    }
                    break;
            }

            TryMatchBreakOutSeperator(breakOutSeperators);
            return toReturn;
        }

        private Node ParseFunctionKeyword(Node describer)
        {
            switch (Current.SyntaxKind)
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
            switch (node.NodeType)
            {
                case NodeType.FunctionCall:
                    index++;
                    toReturn = variable;
                    break;
                case NodeType.Type:
                    toReturn = ParseDeclaration(new DescriberNode(), variable as TypeNode);

                    if (MatchFunctionPropertyDeclaration(toReturn))
                        return toReturn;
                    break;
                case NodeType.Indexer:
                case NodeType.Variable:
                    var next = LookAhead();
                    switch (next.Type)
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

                            if (MatchFunctionPropertyDeclaration(toReturn))
                                return toReturn;
                            break;
                    }
                    break;
            }

            TryMatchBreakOutSeperator(breakOutSeperators);
            return toReturn;
        }

        private bool MatchFunctionPropertyDeclaration(Node node)
        {
            switch(node.NodeType)
            {
                case NodeType.MethodDeclaration:
                case NodeType.PropertyInitialise:
                case NodeType.PropertyDeclaration:
                    return true;
                default:
                    return false;
            }
        }

        private Node ParseDeclaration(DescriberNode describer) => ParseDeclaration(describer, ParseType(SeperatorKind.Any));

        private Node ParseDeclaration(DescriberNode describer, Node typeNode)
        {
            index++;

            if (MatchCurrent(Seperator.Colon, true))
                return ParseVariableDeclaration(describer, typeNode);
            else if (MatchCurrentType(TokenType.Identifier))
                return ParseFunctionDeclaration(describer, (TypeNode)typeNode);
            else
                return ReturnInvalidTokenNode(SeperatorKind.Any);
        }

        private Node ParseVariableDeclaration(DescriberNode _describer, Node _type)
        {
            List<Node> assignNodes = new List<Node>();

            if (LookAhead() == Seperator.FlowerOpenBracket)
                assignNodes.Add(ParseVariableInitialisation(_describer, _type, ParseProperty(_type, ParseIdentifier()), true));
            else
            {
                while (index < sourceFile.TokenCount)
                {
                    assignNodes.Add(ParseVariableInitialisation(_describer, _type, ParseIdentifier(), false));

                    if (MatchCurrent(Seperator.Comma, true))
                        continue;

                    break;
                }
            }

            switch (assignNodes.Count)
            {
                case 1:
                    return assignNodes[0];
                default:
                    return new CompoundVariableDeclarationNode(assignNodes);
            }
        }

        private Node ParseVariableInitialisation(DescriberNode describer, Node _type, Node _variableNode, bool isProperty)
        {
            var next = LookAhead();

            if (next != AssignmentOperator.Assignment)
            {
                if (!isProperty)
                    index++;

                return new DeclarationNode(describer, _type, _variableNode);
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
                TryMatchCurrent(AssignmentOperator.Assignment);

                if(!CheckVariable(expresssion))
                    expresssion = PushInvalidEntityNodeCurrent(expresssion, NodeType.Variable);

                expresssion = ParseVariableAssignment(expresssion, AssignmentOperator.Assignment, breakOutSeperators);
            }

            expresssion = assignmentOperator.BaseOperator == null ? expresssion : new BinaryExpression(assignmentOperator.BaseOperator, variable, expresssion);
            return new AssignmentNode(variable, expresssion);
        }

        private Node ParseImportStatement()
        {
            index++;
            UDDataType entityType;

            var current = Current;
            switch (current.SyntaxKind)
            {
                case SyntaxKind.Enum:
                    entityType = UDDataType.Enum;
                    break;
                case SyntaxKind.Class:
                    entityType = UDDataType.Class;
                    break;
                case SyntaxKind.String:
                    entityType = UDDataType.Struct;
                    break;
                case SyntaxKind.Interface:
                    entityType = UDDataType.Interface;
                    break;
                default:
                    entityType = UDDataType.Namespace;
                    break;
            }

            if (entityType != UDDataType.Namespace)
                index++;

            return new ImportNode(UDDataType.Namespace, ParseVariable(SeperatorKind.Semicolon));
        }

        private Node ParseTryCatchBlock()
        {
            ForceMatchCurrent(Keyword.Try, true);
            var tryBlock = new TryBlockNode(ParseStatement(StatementEnum.GeneralStatement | StatementEnum.Scope));
            index++;

            Node catchBlock = null, finallyBlock = null;
            if (MatchCurrent(Keyword.Catch, true))
            {
                Node arguments;
                if (MatchCurrent(Seperator.OpenBracket))
                {
                    arguments = new CatchBlockArgumentsNode(ParseDeclarationArguments());
                    index++;
                }
                else
                    arguments = new CatchBlockArgumentsNode();

                catchBlock = new CatchBlockNode(arguments, ParseStatement(StatementEnum.GeneralStatement | StatementEnum.Scope));
                index++;
            }

            if (MatchCurrent(Keyword.Finally, true))
                finallyBlock = new FinallyBlockNode(ParseStatement(StatementEnum.GeneralStatement | StatementEnum.Scope));

            return new TryCatchFinallyBlockNode(tryBlock, catchBlock, finallyBlock);
        }

        private Node ParseActionDelegateDeclaration(Node describer)
        {
            ForceMatchCurrent(Keyword.Action, true);
            TryMatchCurrent(Seperator.Colon, true);

            var name = ParseIdentifier();

            index++;
            if (MatchCurrent(AssignmentOperator.Assignment, true))
            {
                if (MatchCurrent(Seperator.OpenBracket))
                {
                    var arguments = new FunctionDeclarationArgumentsNode(ParseDeclarationArguments());
                    index++;

                    ForceMatchCurrent(Seperator.Lambda, true);

                    var body = ParseStatement(StatementEnum.Scope);

                    index++;
                    return new ActionDelegateNode(describer, name, arguments, body);
                }
                else
                    return new ActionDelegateNode(describer, name, ParseVariable(SeperatorKind.Semicolon), new EmptyNode());
            }
            else
            {
                CompileException exception;
                if (index >= sourceFile.TokenCount)
                    exception = new TokenExpectedException(AssignmentOperator.Assignment, Final);
                else
                    exception = new TokenExpectedException(AssignmentOperator.Assignment, Current);

                return PanicModeSimple(exception, SeperatorKind.Any);
            }
        }

        private Node ParseFunctionDelegateDeclaration(Node describer)
        {
            ForceMatchCurrent(Keyword.Function);
            var type = new FunctionTypeNode(ParseGenericCall(true));
            index++;
 
            TryMatchCurrent(Seperator.Colon, true);

            var name = ParseIdentifier();
            index++;

            if (MatchCurrent(AssignmentOperator.Assignment, true))
            {
                if (MatchCurrent(Seperator.OpenBracket))
                {
                    var arguments = new FunctionDeclarationArgumentsNode(ParseDeclarationArguments());
                    index++;

                    ForceMatchCurrent(Seperator.Lambda, true);

                    var body = ParseStatement(StatementEnum.Scope);

                    index++;
                    return new FunctionDelegateNode(describer, type, name, arguments, body);
                }
                else
                    return new FunctionDelegateNode(describer, type, name, ParseVariable(SeperatorKind.Semicolon), new EmptyNode());
            }
            else
            {
                CompileException exception;
                if (index >= sourceFile.TokenCount)
                    exception = new TokenExpectedException(AssignmentOperator.Assignment, Final);
                else
                    exception = new TokenExpectedException(AssignmentOperator.Assignment, Current);

                return PanicModeSimple(exception, SeperatorKind.Any);
            }
        }

        private Node ReturnInvalidTokenNode(SeperatorKind breakOutSeperators)
        {
            var current = Current;
            var exception = new InvalidTokenException(current);
            sourceFile.PushException(exception);

            return PanicModeSimple(exception, breakOutSeperators);
        }

        //Expressions and Entities
        private Node ParseExpression(bool readEmpty, bool readAssignment, SeperatorKind breakOutSeperators)
        {
            var output = new List<Node>();
            var stack = new List<Token>();
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
                                    ForceMatchCurrent(Seperator.Dot, true);
                                    output.Add(new ThisNode(ParseEntity(true, SeperatorKind.Any)));
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
                                    invalid = breakOut = true;
                                    break;
                            }

                            expected = TokenType.Seperator | TokenType.Operator;
                            break;
                        case TokenType.Constant:
                            output.Add(ParseEntity(true, SeperatorKind.Any));
                            expected = TokenType.Seperator | TokenType.BinaryOperator;
                            break;
                        case TokenType.Identifier:
                            output.Add(ParseEntity(true, SeperatorKind.Any));
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
                                output.Add(ParseExpression(false, false, SeperatorKind.CloseBracket));
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

                            var asOperator = (Operator)current;
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
                            if (!readAssignment)
                                invalid = true;

                            breakOut = true;
                            break;
                        case TokenType.BinaryOperator:
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
                }

                if(breakOut)
                {
                    ClearStack(output, stack);

                    if(invalid)
                        output.Add(ParseInvalidExpression(expected, current, breakOutSeperators));

                    break;
                }

                index++;
            }

            if (breakOut)
            {
                switch(output.Count)
                {
                    case 0:
                        if (readEmpty)
                        {
                            output.Add(new EmptyNode());
                            break;
                        }

                        CompileException exception;
                        if (index >= sourceFile.TokenCount)
                            exception = new TokenExpectedException($"{breakOutSeperators}", Final);
                        else
                            exception = new TokenExpectedException(expected, Current);

                        sourceFile.PushException(exception);
                        output.Add(new InvalidExpressionNode(exception));
                        break;
                    case 2:
                        if(invalid)
                        {
                            var invalidNode = output.Pop();

                            var invalidExpression = new InvalidExpressionNode(new UnparsedTokenCollectionNode(stack), output.Pop(), invalidNode);
                            output.Add(invalidExpression);
                        }
                        break;
                }
            }
            else
            {
                if (index >= sourceFile.TokenCount)
                    sourceFile.PushException(new TokenExpectedException($"{breakOutSeperators}", Final));
                else
                    sourceFile.PushException(new TokenExpectedException($"{breakOutSeperators}", Current));
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

        private Node ParseCondition()
        {
            Node condition;
            if (TryMatchCurrent(Seperator.OpenBracket, true))
            {
                condition = ParseExpression(false, false, SeperatorKind.CloseBracket);
                index++;
            }
            else
                condition = new InvalidExpressionNode(PushInvalidCurrentException());

            return condition;
        }

        private Node ParseInvalidExpression(TokenType expected, Token current, SeperatorKind breakOutSeperators)
        {
            CompileException exception;
            if ((expected & TokenType.Seperator) == TokenType.Seperator)
                exception = new TokenExpectedException($"{breakOutSeperators}", current);
            else
                exception = new InvalidTokenException(current);

            sourceFile.PushException(exception);
            return PanicModeSimple(exception, breakOutSeperators);
        }

        private Node ParseTernary(List<Node> output, List<Token> stack, SeperatorKind breakOutSeperators)
        {
            if (output.Count == 0)
            {
                var invalid = ParseInvalidExpression(TokenType.Seperator, Current, breakOutSeperators);

                index--;
                return invalid;
            }

            ClearStack(output, stack);

            var condition = output[output.Count - 1];
            output.RemoveAt(output.Count - 1);

            var trueExpression = ParseExpression(false, false, SeperatorKind.Colon);

            index++;
            var falseExpression = ParseExpression(false, false, SeperatorKind.Semicolon | breakOutSeperators);

            index--;
            return new TernaryExpression(condition, trueExpression, falseExpression);
        }

        private Node ParseIndexer(Node operhand)
        {
            index++;
            var arguments = new List<Node>();
            while (index < sourceFile.TokenCount)
            {
                arguments.Add(ParseExpression(false, false, SeperatorKind.BoxCloseBracket | SeperatorKind.Comma));

                if (MatchCurrent(Seperator.Comma, true))
                    continue;
                else if(MatchCurrent(Seperator.BoxCloseBracket))
                    return new IndexerExpression(operhand, new IndexerArgumentsNode(arguments));
                else
                    break;
            }

            CompileException exception;
            if(index >= sourceFile.TokenCount)
                exception = new TokenExpectedException(Seperator.BoxCloseBracket, Final);
            else
                exception = new TokenExpectedException(Seperator.BoxCloseBracket, Current);

            sourceFile.PushException(exception);
            return PanicModeSimple(exception, SeperatorKind.BoxCloseBracket);
        }

        private Node ParseReturn()
        {
            ForceMatchCurrent(ControlKeyword.Return, true);

            if (MatchCurrent(Seperator.Semicolon))
                return new ReturnKeyword();
            else
                return new ReturnKeyword(ParseExpression(false, false, SeperatorKind.Semicolon));
        }

        private Node ParseDefault()
        {
            ForceMatchCurrent(Keyword.Default, false);

            if (MatchToken(Seperator.OpenBracket, LookAhead()))
            {
                index += 2;
                return new DefaultTypeNode(ParseType(SeperatorKind.CloseBracket));
            }

            return new DefaultValueNode();
        }

        private Node ParseAsType()
        {
            ForceMatchCurrent(Keyword.AsType, true);
            TryMatchCurrent(Seperator.OpenBracket, true);

            return new AsTypeNode(ParseType(SeperatorKind.CloseBracket));
        }

        private Node ParseExpressionList(SeperatorKind finalBreakOut)
        {
            var nodes = new List<Node>();

            index++;
            while (index < sourceFile.TokenCount)
            {
                if (MatchBreakOutSeperator(Current, finalBreakOut))
                    break;

                var expression = ParseExpression(false, true, SeperatorKind.Comma | finalBreakOut);
                if (MatchCurrent(AssignmentOperator.Assignment, true))
                    expression = new AssignmentNode(expression, ParseExpression(false, false, SeperatorKind.Comma | finalBreakOut));

                nodes.Add(expression);
                if (MatchCurrent(Seperator.Comma, true))
                    continue;
                else if (MatchBreakOutSeperator(Current, finalBreakOut))
                    return Extract();
                else
                    break;
            }

            CompileException exception;
            if (index >= sourceFile.TokenCount)
                exception = new TokenExpectedException(Seperator.BoxCloseBracket, Final);
            else
                exception = new TokenExpectedException(Seperator.BoxCloseBracket, Current);

            sourceFile.PushException(exception);
            nodes.Add(PanicModeSimple(exception, finalBreakOut));

            return Extract();
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
                                output.Add(ParseFunctionCall(inExpression, false, ParseVariable(output.Pop())));
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

                        switch (current.SyntaxKind)
                        {
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

                                var exception = new InvalidTokenException(current);
                                sourceFile.PushException(exception);

                                output.Add(PanicModeSimple(exception, breakOutSeperators));
                                break;
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
            {
                CompileException exception;
                if (index >= sourceFile.TokenCount)
                    exception = new TokenExpectedException($"{breakOutSeperators}", Final);
                else
                    exception = new TokenExpectedException(expected, Current);

                sourceFile.PushException(exception);
                output.Add(new InvalidExpressionNode(exception));
            }

            return output[0];
        }

        private Node ParseIdentifier(bool increment = false)
        {
            if (TryMatchCurrentType(TokenType.Identifier))
            {
                var node = new IdentifierNode((Identifier)Current);

                if (increment)
                    index++;
                return node;
            }
            else
            {
                var exception = new TokenExpectedException(TokenType.Identifier, Current);
                sourceFile.PushException(exception);

                return new TokenExpectedNode(exception);
            }
        }

        private Node ParseVariable(Node entity)
        {
            var token = Current;

            Node toReturn;
            if (CheckVariable(entity))
                toReturn = entity;
            else
            {
                var exception = new TokenExpectedException(TokenType.Identifier, token.Index);
                toReturn = new InvalidEntityNode(exception, entity, NodeType.Variable);

                sourceFile.PushException(exception);
            }

            return toReturn;
        }

        private Node ParseVariable(SeperatorKind breakOutSeperators)
        {
            var entity = ParseEntity(false, breakOutSeperators);

            return ParseVariable(entity);
        }

        private bool CheckVariable(Node entity)
        {
            var node = entity;
            while (true)
            {
                if (node.NodeType == NodeType.Dot)
                    node = ((DotExpression)node).RHS;
                else if (node.NodeType == NodeType.Variable)
                    break;
                else
                    return false;
            }

            return node.NodeType == NodeType.Variable;
        }

        private Node ParseType(SeperatorKind breakOutSeperators)
        {
            if (MatchCurrent(Keyword.Var, true))
            {
                TryMatchBreakOutSeperator(breakOutSeperators);
                return new AnonymousTypeNode();
            }

            return ValidateType(Current, ParseEntity(false, breakOutSeperators));
        }

        private Node ValidateType(Token token, Node entity)
        {
            var node = entity;

            while (true)
                if (node.NodeType == NodeType.Dot)
                {
                    var dot = (DotExpression)node;
                    var lhs = dot.LHS.NodeType;

                    if (lhs != NodeType.Type || lhs != NodeType.Variable)
                        break;

                    node = dot.RHS;
                }
                else
                {
                    switch (node.NodeType)
                    {
                        case NodeType.Type:
                            return (TypeNode)entity;
                        case NodeType.Variable:
                            return new CustomTypeNode(entity);
                        default:
                            break;
                    }
                }

            var exception = new TokenExpectedException("Type", token);
            sourceFile.PushException(exception);

            return new InvalidEntityNode(exception, entity, NodeType.Type);
        }

        private Node ParseInheritanceTypes()
        {
            ForceMatchCurrent(Seperator.Colon, true);

            var types = new List<Node>();
            while (index < sourceFile.TokenCount)
            {
                types.Add(new InheritsTypeNode(ParseType(SeperatorKind.Comma)));

                if (MatchCurrent(Seperator.Comma, true))
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

            while (index < sourceFile.TokenCount)
            {
                var current = Current;

                switch (current.Type)
                {
                    case TokenType.BinaryOperator:
                        if (!MatchType(expected, current))
                            return new ParseGenericResult(null, new TokenExpectedException(expected, 0));

                        switch (current.SyntaxKind)
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
                                    return new ParseGenericResult(null, new TokenExpectedException(BinaryOperator.GreaterThan, current));
                                else if (output.Count == 1)
                                    return new ParseGenericResult(output[0], null);
                                break;
                            default:
                                return new ParseGenericResult(null, new InvalidTokenException(current));
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

                        return new ParseGenericResult(null, new InvalidTokenException(current));
                    case TokenType.Identifier:
                        if (!MatchType(expected, current))
                            return new ParseGenericResult(null, new InvalidTokenException(current));
                        else
                        {
                            output.Add(new CustomTypeNode(new IdentifierNode((Identifier)current)));
                            expected = TokenType.Seperator | TokenType.Operator;
                        }
                        break;
                    case TokenType.Seperator:
                        if (!MatchType(expected, current) || !MatchToken(Seperator.Comma, current))
                            return new ParseGenericResult(null, new InvalidTokenException(current));
                        else
                            expected = TokenType.Identifier | TokenType.Keyword;
                        break;
                }

                index++;
            }

            return new ParseGenericResult(null, new TokenExpectedException(BinaryOperator.GreaterThan, Current));
        }

        private Node ParseGenericCall(bool force)
        {
            int beforeIndex = index;

            var genericResult = ParseGenericCall();
            if (!genericResult.Success)
            {
                index = beforeIndex;

                if (force)
                {
                    sourceFile.PushException(genericResult.Exception);
                    return PanicModeSimple(genericResult.Exception, SeperatorKind.Any);
                }
                else
                    return null;
            }
            else
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
                        return PanicModeSimple(genericResult.Exception, SeperatorKind.Any);
                    }
                    else
                        return null;
                }
            }
        }

        private Node ParseGenericDeclaration()
        {
            var types = new List<Node>();
            ForceMatchCurrent(BinaryOperator.LesserThan, true);

            while (index < sourceFile.TokenCount)
            {
                Node variable = ParseIdentifier(), enforcement = null;

                index++;
                if (MatchCurrent(Seperator.Colon))
                    enforcement = ParseInheritanceTypes();

                types.Add(enforcement == null ? new GenericDeclarationVariableNode(variable) : new GenericDeclarationVariableNode(variable, enforcement));
                if (MatchCurrent(Seperator.Semicolon, true))
                    continue;
                else if (MatchCurrent(BinaryOperator.GreaterThan))
                    return new GenericDeclarationNode(types);
                else
                    break;
            }

            CompileException exception;
            if (index >= sourceFile.TokenCount)
                exception = new TokenExpectedException(Seperator.BoxCloseBracket, Final);
            else
                exception = new TokenExpectedException(Seperator.BoxCloseBracket, Current);

            sourceFile.PushException(exception);
            return PanicModeSimple(exception, SeperatorKind.Any);
        }

        //Loops
        private Node ParseForLoop()
        {
            ForceMatchCurrent(LoopKeyword.For, true);
            TryMatchCurrent(Seperator.OpenBracket, true);
            var initialise = ParseStatement(StatementEnum.StandaloneStatement | StatementEnum.EmptyStatement);

            index++;
            var expression = ParseExpression(true, false, SeperatorKind.Semicolon);
  
            index++;
            var increment = ParseMultiLine(Seperator.CloseBracket, SeperatorKind.CloseBracket | SeperatorKind.Comma);
            var body = ParseStatement(StatementEnum.Scope | StatementEnum.GeneralStatement);

            return new ForLoopNode(initialise, expression, increment, body);

            Node ParseMultiLine(Seperator finalBreakOut, SeperatorKind breakOutSeperators)
            {
                var subStatements = new List<Node>();

                while (index < sourceFile.TokenCount)
                {
                    subStatements.Add(ParseStatement(true, breakOutSeperators));
                    if (Current == finalBreakOut)
                    {
                        index++;
                        return subStatements.Count == 1 ? subStatements[0] : new CompoundStatementNode(subStatements);
                    }

                    index++;
                }

                throw new TokenExpectedException(finalBreakOut, Final);
            }
        }

        private Node ParseForeachLoop()
        {
            ForceMatchCurrent(LoopKeyword.Foreach, true);
            TryMatchCurrent(Seperator.OpenBracket, true);

            var type = ParseType(SeperatorKind.Colon);
            TryMatchCurrent(Seperator.Colon, true);

            var variable = ParseIdentifier(true);

            TryMatchCurrent(DescriberKeyword.In, true);
            var array = ParseVariable(SeperatorKind.CloseBracket);
            index++;

            var body = ParseStatement(StatementEnum.Scope | StatementEnum.GeneralStatement);

            return new ForeachLoopNode(new DeclarationNode(new DescriberNode(), type, variable), array, body);
        }

        private Node ParseWhileLoop()
        {
            ForceMatchCurrent(LoopKeyword.While, true);
            Node condition = ParseCondition();

            var body = ParseStatement(StatementEnum.Scope | StatementEnum.GeneralStatement);
            return new WhileLoopNode(condition, body);
        }

        private Node ParseDoWhileLoop()
        {
            ForceMatchCurrent(LoopKeyword.Do, true);

            var body = ParseStatement(StatementEnum.Scope | StatementEnum.GeneralStatement);
            index++;

            TryMatchCurrent(LoopKeyword.While, true);

            Node condition = ParseCondition();
            return new DoWhileNode(condition, body);
        }

        //Conditions
        private Node ParseIfCondition()
        {
            var nodes = new List<Node>();

            bool breakOut = false;
            while (index < sourceFile.TokenCount)
            {
                var current = Current;
                switch (current.SyntaxKind)
                {
                    case SyntaxKind.If:
                        index++;
                        var condition = ParseCondition();
                        var body = ParseStatement();

                        nodes.Add(new IfNode(condition, body));
                        break;
                    case SyntaxKind.Else:
                        index++;
                        if (MatchCurrent(ConditionKeyword.If))
                            continue;

                        breakOut = true;
                        nodes.Add(new ElseNode(ParseStatement()));
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

            var valueToCheck = ParseCondition();
            TryMatchCurrent(Seperator.FlowerOpenBracket, true);

            Token current;
            bool breakOut = false;
            var cases = new List<Node>();
            while (index < sourceFile.TokenCount)
            {
                current = Current;
                if (MatchToken(Seperator.FlowerCloseBracket, current))
                    break;

                switch (current.SyntaxKind)
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
                                    index++;
                                    if (LookAhead() == Keyword.When)
                                        value = ParseWhenStatement(ValidateType(current, value));
                                    else
                                    {
                                        PushInvalidCurrentToken();
                                        index++;
                                    }
                                }
                                break;
                            case TokenType.Keyword:
                                var type = ParseType(SeperatorKind.Colon);

                                TryMatchCurrent(Seperator.Colon, true);
                                value = ParseWhenStatement(type);
                                break;
                            case TokenType.Constant:
                                value = new ConstantValueNode((Constant)current);
                                index++;
                                break;
                            default:
                                value = PushInvalidCurrentToken();
                                break;
                        }

                        TryMatchCurrent(Seperator.Colon, true);
                        if (MatchCurrent(ConditionKeyword.Case, false))
                        {
                            cases.Add(new CaseNode(value));
                            continue;
                        }

                        cases.Add(new CaseNode(value, ParseBody(), ParseControlStatement()));
                        break;
                    case SyntaxKind.Default:
                        index++;

                        TryMatchCurrent(Seperator.Colon, true);

                        cases.Add(new DefaultNode(ParseBody(), ParseControlStatement()));
                        breakOut = true;
                        break;
                    default:
                        PushInvalidCurrentException();
                        break;
                }

                index++;
                if (breakOut)
                    break;
            }

            TryMatchCurrent(Seperator.FlowerCloseBracket);
            return new SwitchNode(valueToCheck, cases);

            Node ParseBody()
            {
                if (MatchCurrent(ControlKeyword.Break) || MatchCurrent(ControlKeyword.Return))
                    return new EmptyNode();

                var stmt = ParseStatement(StatementEnum.GeneralStatement | StatementEnum.Scope);

                index++;
                return stmt;
            }

            Node ParseControlStatement()
            {
                Node controlStatement;

                if (MatchCurrent(ControlKeyword.Return))
                    controlStatement = ParseReturn();
                else
                {
                    if (TryMatchCurrent(ControlKeyword.Break, true))
                        controlStatement = new BreakNode();
                    else
                        controlStatement = PushInvalidCurrentToken();
                }

                TryMatchCurrent(Seperator.Semicolon);
                return controlStatement;
            }

            Node ParseWhenStatement(Node typeNode)
            {
                var variable = ParseIdentifier();

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

            Node getNode = null, setNode = null, describer;

            if (MatchCurrent(Seperator.BoxOpenBracket))
                describer = ParseDescribers();
            else
                describer = new DescriberNode();

            if (MatchCurrent(Keyword.Get, true))
                getNode = new GetNode(describer, ParseAccessorBody(StatementEnum.LambdaExpression));

           

            if (MatchCurrent(Seperator.BoxOpenBracket))
            {
                describer = ParseDescribers();

                TryMatchCurrent(Keyword.Set, true);
                setNode = ParseSetNode(describer);
            }
            else if (MatchCurrent(Keyword.Set, true))
            {
                describer = new DescriberNode();
                setNode = ParseSetNode(describer);
            }

            ForceMatchCurrent(Seperator.FlowerCloseBracket);

            if (getNode != null && setNode != null)
                return new PropertyGetSetNode(variableNode, typeNode, getNode, setNode);
            else if (getNode != null)
                return new PropertyGetNode(variableNode, typeNode, getNode);
            else
                return new PropertySetNode(variableNode, typeNode, setNode);

            Node ParseAccessorBody(StatementEnum lambdaType)
            {
                Node body; 
                if(MatchCurrent(Seperator.Semicolon))
                    body = new EmptyNode();
                else
                    body = ParseStatement(lambdaType | StatementEnum.Scope);

                index++;
                return body;
            }

            Node ParseSetNode(Node describer)
            {
                var setvalueDeclaration = new DeclarationNode(new DescriberNode(new DescriberKeywordNode(DescriberKeyword.Const)), typeNode, new IdentifierNode(new Identifier("value", 0)));
                return new SetNode(describer, setvalueDeclaration, ParseAccessorBody(StatementEnum.LambdaStatement));
            }
        }

        private Node ParseConstructorCall(bool inExpression)
        {
            ForceMatchCurrent(Keyword.Create, true);
            var functionCall = ParseFunctionCall(true, true, ParseEntity(inExpression, SeperatorKind.OpenBracket));

            if (MatchToken(Seperator.FlowerOpenBracket, LookAhead(), true))
                functionCall.AddChild(ParseExpressionList(SeperatorKind.FlowerCloseBracket));

            return functionCall;
        }

        private Node ParseFunctionCall(bool inExpression, bool isContructor, Node functionName)
        {
            var arguments = new List<Node>();
            TryMatchCurrent(Seperator.OpenBracket, true);

            if (!MatchCurrent(Seperator.CloseBracket))
            {
                while (index < sourceFile.TokenCount)
                {
                    Node describer = null;
                    if (MatchCurrentType(TokenType.Keyword))
                    {
                        var keyword = (Keyword)Current;
                        if (keyword.KeywordType == KeywordType.Describer)
                            describer = new DescriberKeywordNode(Current as Keyword);
                        else
                            describer = PushInvalidToken(keyword);
                        index++;

                        TryMatchCurrent(Seperator.Colon, true);
                    }

                    if (describer == null)
                        arguments.Add(new FunctionCallArgumentNode(ParseExpression(false, false, SeperatorKind.Comma | SeperatorKind.CloseBracket)));
                    else
                        arguments.Add(new FunctionCallArgumentNode(describer, ParseEntity(false, SeperatorKind.Comma | SeperatorKind.CloseBracket)));

                    if (MatchCurrent(Seperator.CloseBracket))
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
            TryMatchCurrent(Seperator.OpenBracket, true);

            var argument = new FunctionCallArgumentNode(ParseExpression(false, false, SeperatorKind.CloseBracket));
            return new PrintNode(new FunctionCallArgumentsNode(argument));
        }

        private Node ParseInputCall()
        {
            ForceMatchCurrent(Keyword.Input, true);
            TryMatchCurrent(Seperator.OpenBracket, true);

            var argument = new FunctionCallArgumentNode(ParseExpression(true, false, SeperatorKind.CloseBracket));
            return argument.NodeType == NodeType.Empty ? new InputNode(new FunctionCallArgumentsNode()) : new InputNode(new FunctionCallArgumentsNode(argument));
        }

        private Node ParseFunctionDeclaration(Node describer, TypeNode returnType)
        {
            Node name = ParseIdentifier(true);
            var arguments = new FunctionDeclarationArgumentsNode(ParseDeclarationArguments());

            Node generic = null;
            if (MatchToken(BinaryOperator.LesserThan, LookAhead(), true))
                generic = ParseGenericDeclaration();

            index++;
            Node extensionTypeNode = null;
            if (MatchCurrent(Seperator.Colon, true))
            {
                if (returnType.Type == TypeNodeEnum.Constructor)
                {
                    TryMatchCurrent(Keyword.Parent, true);
                    if (MatchCurrent(Seperator.OpenBracket, true))
                        extensionTypeNode = new ParentFunctionArgumentNode(ParseExpressionList(SeperatorKind.CloseBracket));
                    else
                    {
                        var exception = new TokenExpectedException(Seperator.OpenBracket, Current);
                        sourceFile.PushException(exception);

                        extensionTypeNode = PanicModeSimple(exception, SeperatorKind.CloseBracket);
                    }

                    index++;
                }
                else
                    extensionTypeNode = ParseType(SeperatorKind.FlowerOpenBracket | SeperatorKind.Lambda);
            }

            var type = returnType.Type == TypeNodeEnum.Constructor || returnType.Type == TypeNodeEnum.Void ? StatementEnum.LambdaStatement : StatementEnum.LambdaExpression;
            var body = ParseStatement(StatementEnum.Scope | type);

            Node toReturn;
            if (returnType.Type == TypeNodeEnum.Constructor)
            {
                if (extensionTypeNode == null)
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
            var returnType = ParseType(SeperatorKind.Any);

            index++;
            Operator operatorToOverload = null;
            if (TryMatchCurrentType(TokenType.Operator, false))
                operatorToOverload = Current as Operator;

            index++;
            var arguments = new FunctionDeclarationArgumentsNode(ParseDeclarationArguments());

            Node generic = null;
            if (MatchToken(BinaryOperator.LesserThan, LookAhead(), true))
                generic = ParseGenericDeclaration();

            index++;

            var body = ParseStatement(StatementEnum.Scope | StatementEnum.LambdaExpression);
            var toReturn = new OperatorOverloadFunctionDeclarationNode(describer, returnType, arguments, body, operatorToOverload);

            if (generic != null)
                toReturn.AddChild(generic);

            return toReturn;
        }

        private Node ParseConversionOverload(Node describer)
        {
            var keyword = Current;
            index++;

            var returnType = ParseType(SeperatorKind.OpenBracket);
            var arguments = new FunctionDeclarationArgumentsNode(ParseDeclarationArguments());

            Node generic = null;
            if (MatchToken(BinaryOperator.LesserThan, LookAhead(), true))
                generic = ParseGenericDeclaration();

            index++;
            var body = ParseStatement(StatementEnum.Scope | StatementEnum.LambdaExpression);

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
            var returnType = ParseType(SeperatorKind.OpenBracket);

            var arguments = new FunctionDeclarationArgumentsNode(ParseDeclarationArguments());

            Node generic = null;
            if (MatchToken(BinaryOperator.LesserThan, LookAhead(), true))
                generic = ParseGenericDeclaration();

            var body = ParseProperty(returnType, returnType);

            Node toReturn = new IndexerDeclarationNode(describer, returnType, arguments, body);

            if (generic != null)
                toReturn.AddChild(generic);

            return toReturn;
        }

        private List<Node> ParseDeclarationArguments()
        {
            TryMatchCurrent(Seperator.OpenBracket, true);

            var arguments = new List<Node>();
            if (MatchCurrent(Seperator.CloseBracket))
                return arguments;

            while (index < sourceFile.TokenCount)
            {
                Node describer = MatchCurrent(Seperator.BoxOpenBracket) ? ParseDescribers() : new DescriberNode();

                Node type = ParseType(SeperatorKind.Colon);
                TryMatchCurrent(Seperator.Colon, true);
                var variable = ParseIdentifier();

                Node defaultValue = null;
                if (LookAhead() == AssignmentOperator.Assignment)
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
                if (MatchCurrent(Seperator.Comma, true))
                    continue;

                break;
            }

            TryMatchCurrent(Seperator.CloseBracket);
            return arguments;
        }

        //User Defined Types and Namespaces
        private Node ParseScopedEntity(Node describer, Keyword keyword) => keyword == EntityKeyword.Namespace ? ParseNamespaceDeclaration(describer) : ParseTypeDeclaration(describer, keyword);

        private Node ParseNamespaceDeclaration(Node describer)
        {
            index++;
            var name = ParseVariable(SeperatorKind.FlowerOpenBracket);

            TryMatchCurrent(Seperator.FlowerOpenBracket);
            return new NamespaceNode(describer, name, ParseScope());
        }

        private Node ParseTypeDeclaration(Node describer, Keyword keyword)
        {
            index++;
            var name = ParseIdentifier(true);

            Node generic = null, inherits = null;
            if (MatchCurrent(BinaryOperator.LesserThan))
            {
                generic = ParseGenericDeclaration();
                index++;
            }

            if (MatchCurrent(Seperator.Colon))
                inherits = ParseInheritanceTypes();

            TryMatchCurrent(Seperator.FlowerOpenBracket);

            Node body;
            if (keyword == EntityKeyword.Enum)
                body = ParseExpressionList(SeperatorKind.FlowerCloseBracket);
            else
                body = ParseScope();

            Node toReturn = keyword.SyntaxKind switch
            {
                SyntaxKind.Enum => new EnumNode(describer, name, body),
                SyntaxKind.Class => new ClassNode(describer, name, body),
                SyntaxKind.Struct => new StructNode(describer, name, body),
                SyntaxKind.Interface => new InterfaceNode(describer, name, body),
                _ => throw new InvalidTokenException(keyword)
            };

            if (generic != null)
                toReturn.AddChild(generic);
            if (inherits != null)
                toReturn.AddChild(inherits);

            return toReturn;
        }

        //Error Handling
        private InvalidTokenNode PushInvalidCurrentToken() => PushInvalidToken(Current);
        private InvalidTokenNode PushInvalidToken(Token token) => new InvalidTokenNode(PushInvalidTokenException(token), token);

        private InvalidTokenException PushInvalidCurrentException() => PushInvalidTokenException(Current);
        private InvalidTokenException PushInvalidTokenException(Token token)
        {
            var exception = new InvalidTokenException(token);
            sourceFile.PushException(exception);

            return exception;
        }

        private InvalidEntityNode PushInvalidEntityNodeCurrent(Node invalid, NodeType expected) => new InvalidEntityNode(PushInvalidEntityException(Current.Index), invalid, expected);

        private InvalidStatementException PushCurrentInvalidStatement() => PushInvalidStatement(index);
        private InvalidStatementException PushInvalidStatement(int curIndex) => new InvalidStatementException(curIndex);

        private InvalidStatementException PushInvalidEntityException(int index)
        {
            var exception = new InvalidStatementException(index);
            sourceFile.PushException(exception);

            return exception;
        }

        private Node PanicModeSimple(CompileException exception, SeperatorKind breakOutSeperators)
        {
            var invalid = new List<Token>();
            breakOutSeperators = breakOutSeperators | SeperatorKind.Semicolon | SeperatorKind.FlowerOpenBracket | SeperatorKind.FlowerCloseBracket;

            while(index <= sourceFile.TokenCount)
            {
                var current = Current;

                if(MatchBreakOutSeperator(current, breakOutSeperators))
                    break;

                invalid.Add(current);
                index++;
            }

            if (index >= sourceFile.TokenCount)
                exception = new CompoundCompileException(exception, new TokenExpectedException($"{breakOutSeperators}", Final));

            return new InvalidTokenCollectionNode(exception, invalid);
        }
    }
}