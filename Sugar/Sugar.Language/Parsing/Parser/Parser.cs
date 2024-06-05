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
using Sugar.Language.Parsing.Nodes.Expressions;
using Sugar.Language.Parsing.Nodes.Values.Invalid;
using Sugar.Language.Parsing.Nodes.DataTypes;
using Sugar.Language.Parsing.Nodes.Types.Enums;
using Sugar.Language.Parsing.Nodes.Types.Subtypes;
using Sugar.Language.Parsing.Nodes.CtrlStatements;
using Sugar.Language.Parsing.Nodes.TryCatchFinally;
using Sugar.Language.Parsing.Nodes.Values.Generics;
using Sugar.Language.Parsing.Nodes.Functions.Calling;
using Sugar.Language.Parsing.Nodes.DataTypes.Enums;
using Sugar.Language.Parsing.Nodes.Expressions.Invalid;
using Sugar.Language.Parsing.Nodes.Functions.Properties;
using Sugar.Language.Parsing.Nodes.Expressions.Operative;
using Sugar.Language.Parsing.Nodes.TryCatchFinally.Blocks;
using Sugar.Language.Parsing.Nodes.Functions.Declarations;
using Sugar.Language.Parsing.Nodes.Expressions.Associative;
using Sugar.Language.Parsing.Nodes.Conditions.IfConditions;
using Sugar.Language.Parsing.Nodes.Statements.VariableCreation;
using Sugar.Language.Parsing.Nodes.Functions.Calling.Structure;
using Sugar.Language.Parsing.Nodes.Conditions.SwitchConditions;
using Sugar.Language.Parsing.Nodes.Functions.Properties.Accessors;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

using Sugar.Language.Parsing.Nodes.Functions.Declarations.OperatorOverloading;
using Sugar.Language.Parsing.Nodes.Values.Generics.Invalid;
using Sugar.Language.Parsing.Nodes.Types.Invalid;
using Sugar.Language.Parsing.Nodes.Statements.VariableCreation.Properties;
using Sugar.Language.Parsing.Nodes.Declarations.Delegates;

namespace Sugar.Language.Parsing.Parser
{
    internal sealed class Parser : SingletonService<Parser>
    {
        private int index;

        private SugarFile sourceFile;
        private Token Current
        {
            get
            {
                if (index >= sourceFile.TokenCount)
                    return Final;

                return sourceFile[index];
            }
        }
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

            var nodes = new List<ParseNode>();
            for (index = 0; index < sourceFile.TokenCount; index++)
            {
                
                var toAdd = ParseStatement();

                if (toAdd.NodeType != ParseNodeType.Empty)
                    nodes.Add(toAdd);
            }

            if (nodes.Count == 0)
                sourceFile.WithSyntaxTree(new SyntaxTree(new EmptyNode()));
            else
                sourceFile.WithSyntaxTree(new SyntaxTree(new SugarFileGroupNode(nodes)));
        }

        private ParseNode ParseStatement() => ParseStatement(StatementEnum.Scope | StatementEnum.GeneralStatement);

        private ParseNode ParseStatement(StatementEnum statementType)
        {
            var current = Current;

            switch(current.SyntaxKind)
            {
                case SyntaxKind.FlowerOpenBracket:
                    if (!Match(StatementEnum.Scope))
                        return new InvalidEntityNode(PushInvalidCurrentException(), PanicModeSimple(SeperatorKind.None));

                    return ParseScope();
                case SyntaxKind.Lambda:
                    index++;
                    if (Match(StatementEnum.LambdaStatement))
                        return ParseStatement(false, SeperatorKind.Semicolon);
                    else if (Match(StatementEnum.LambdaExpression))
                        return new LambdaNode(ParseNonEmptyExpression(false, SeperatorKind.Semicolon));
                    else
                        return new InvalidStatementNode(PushInvalidStatementException(), PanicModeSimple(SeperatorKind.None));
                default:
                    if(!Match(StatementEnum.StandaloneStatement))
                        return new InvalidStatementNode(PushInvalidStatementException(), PanicModeSimple(SeperatorKind.None));

                    return ParseStatement(Match(StatementEnum.EmptyStatement), SeperatorKind.Semicolon);
            }

            bool Match(StatementEnum match) => (statementType & match) == match;            
        }

        private ParseNode ParseStatement(bool readEmty, SeperatorKind breakOutSeperators)
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
                    var expression = new UnaryExpression(unaryOperator, ParseEntity(false, SeperatorKind.Semicolon));

                    TryMatchCurrent(Seperator.Semicolon);
                    return expression;
                case TokenType.Seperator:
                    if (readEmty && MatchBreakOutSeperator(Current, breakOutSeperators))
                        return new EmptyNode();
                    else if (TryMatchCurrent(Seperator.BoxOpenBracket))
                        return ParseDescriberStatement(breakOutSeperators);
                    else
                        return new InvalidStatementNode(PushInvalidStatementException(), PanicModeSimple(breakOutSeperators));
                default:
                    return new InvalidStatementNode(PushInvalidStatementException(), PanicModeSimple(breakOutSeperators));
            }
        }

        //Expressions and Entities

        /// <summary>
        /// Parse Scope (can be empty)
        /// <para>• Force starts at opening { </para>
        /// • Ends on same index as closing } 
        /// </summary>
        private ParseNode ParseScope()
        {
            var nodes = new List<ParseNode>();
            ForceMatchCurrent(Seperator.FlowerOpenBracket, true);

            for (; index < sourceFile.TokenCount; index++)
            {
                if (MatchCurrent(Seperator.FlowerCloseBracket))
                    break;

                var stmt = ParseStatement();
                if (stmt.NodeType != ParseNodeType.Empty)
                    nodes.Add(stmt);
            }

            TryMatchCurrent(Seperator.FlowerCloseBracket);

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

        private ParseNode ParseDescriberStatement(SeperatorKind breakOutSeperators)
        {
            ParseNode toReturn;
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
                                    toReturn = ParseVariableDeclaration(describer, new AnonymousTypeNode());
                                    break;
                                case SyntaxKind.Action:
                                    toReturn = ParseActionDelegateDeclaration(describer);
                                    break;
                                case SyntaxKind.Function:
                                    toReturn = ParseActionDelegateDeclaration(describer);
                                    break;
                                default:
                                    return new InvalidEntityNode(PushInvalidCurrentException(), PanicModeSimple(breakOutSeperators));
                            }
                            break;
                        case KeywordType.Type:
                            toReturn = ParseDeclaration(describer);

                            if (MatchFunctionPropertyDeclaration(toReturn))
                                return toReturn;
                            break;
                        default:
                            return new InvalidEntityNode(PushInvalidCurrentException(), PanicModeSimple(breakOutSeperators));
                    }
                    break;
                default:
                    return new InvalidEntityNode(PushInvalidCurrentException(), PanicModeSimple(breakOutSeperators));
            }

            TryMatchBreakOutSeperator(breakOutSeperators);
            return toReturn;
        }

        /// <summary>
        /// Parse Describer
        /// <para>• Force starts at opening [ </para>
        /// • Ends one index after closing ]
        /// </summary>
        private DescriberNode ParseDescribers()
        {
            var describer = new DescriberNode();
            ForceMatchCurrent(Seperator.BoxOpenBracket, true);

            while (index < sourceFile.TokenCount)
            {
                var current = Current;

                if(MatchType(TokenType.Keyword, current))
                {
                    var keyword = (Keyword)Current;

                    if (keyword.KeywordType == KeywordType.Describer)
                    {
                        describer.AddChild(new DescriberKeywordNode(keyword));

                        index++;
                        if (MatchCurrent(Seperator.BoxCloseBracket, true))
                        {
                            if (MatchCurrent(Seperator.BoxOpenBracket, true))
                                continue;

                            break;
                        }

                        TryMatchCurrent(Seperator.Comma, true);
                        continue;
                    }
                }

                describer.AddChild(new InvalidDescriberNode(PanicModeSimple(SeperatorKind.BoxCloseBracket), PushInvalidTokenException(current)));
                MatchCurrent(Seperator.BoxCloseBracket, true);
                break;
            }

            return describer;
        }

        private ParseNode ParseKeyword(SeperatorKind breakOutSeperators)
        {
            ParseNode toReturn;

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
                    break;
                case SyntaxKind.Input:
                    toReturn = ParseInputCall(false);
                    break;
                case SyntaxKind.Throw:
                    index++;
                    toReturn = new ThrowExceptionNode(ParseNonEmptyExpression(false, SeperatorKind.Semicolon));
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
                case SyntaxKind.Constructor:
                    index++;
                    toReturn = ParseConstructorDeclaration(new DescriberNode(), new ConstructorTypeNode(ParseType(SeperatorKind.OpenBracket)));
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
                            return new InvalidEntityNode(PushInvalidCurrentException(), PanicModeSimple(breakOutSeperators));
                    }
                    break;
            }

            TryMatchBreakOutSeperator(breakOutSeperators);
            return toReturn;
        }

        private ParseNode ParseFunctionKeyword(DescriberNode describer)
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
                    return ParseConstructorDeclaration(describer, new ConstructorTypeNode(ParseType(SeperatorKind.OpenBracket)));
                default:
                    return ParseConversionOverload(describer);
            }
        }

        private ParseNode ParseIdentifier(SeperatorKind breakOutSeperators)
        {
            var variable = ParseEntity(false, SeperatorKind.None);
            var node = variable;

            while (true)
                if (node.NodeType == ParseNodeType.Dot)
                    node = ((DotExpression)node).RHS;
                else
                    break;

            ParseNode toReturn = null;
            switch (node.NodeType)
            {
                case ParseNodeType.FunctionCall:
                    toReturn = variable;
                    index++;
                    break;
                case ParseNodeType.Type:
                    toReturn = ParseDeclaration(new DescriberNode(), variable as TypeNode);

                    if (MatchFunctionPropertyDeclaration(toReturn))
                        return toReturn;
                    break;
                case ParseNodeType.Indexer:
                case ParseNodeType.Variable:
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
                            if(next.SyntaxKind == SyntaxKind.Lambda)
                            {
                                index += 2;
                                toReturn = new AssignmentNode(variable, ParseDelegateDeclaration(new DescriberNode()));

                                break;
                            }

                            toReturn = ParseDeclaration(new DescriberNode(), new CreatedTypeNode(variable));

                            if (MatchFunctionPropertyDeclaration(toReturn))
                                return toReturn;
                            break;
                    }
                    break;
            }

            TryMatchBreakOutSeperator(breakOutSeperators);
            return toReturn;
        }

        private bool MatchFunctionPropertyDeclaration(ParseNode node)
        {
            switch(node.NodeType)
            {
                case ParseNodeType.PropertyInitialise:
                case ParseNodeType.PropertyDeclaration:
                case ParseNodeType.FunctionDeclaration:
                    return true;
                default:
                    return false;
            }
        }

        private ParseNode ParseDeclaration(DescriberNode describer) => ParseDeclaration(describer, ParseType(SeperatorKind.None));

        private ParseNode ParseDeclaration(DescriberNode describer, TypeNode typeNode)
        {
            index++;

            if (MatchCurrent(Seperator.Colon, true))
                return ParseVariableDeclaration(describer, typeNode);
            else if (MatchCurrentType(TokenType.Identifier))
                return ParseFunctionDeclaration(describer, typeNode);
            else
                return new InvalidEntityNode(PushInvalidCurrentException(), PanicModeSimple(SeperatorKind.None));
        }

        private ParseNode ParseVariableDeclaration(DescriberNode describer, TypeNode type)
        {
            if (LookAhead() == Seperator.FlowerOpenBracket)
            {
                var identifier = ParseIdentifier(true);
                var property = ParseProperty(describer, type);

                if (LookAhead() == AssignmentOperator.Assignment)
                {
                    index += 2;
                    return new PropertyInitialisationNode(describer, type, identifier, property, ParseNonEmptyExpression(false, SeperatorKind.Semicolon));

                }
                else
                    return new PropertyDeclarationNode(describer, type, identifier, property);
            }
            else
            {
                List<DeclarationNode> declarations = new List<DeclarationNode>();

                IdentifierNode identifier;
                while (index < sourceFile.TokenCount)
                {
                    identifier = ParseIdentifier(true);

                    if (MatchCurrent(AssignmentOperator.Assignment))
                    {
                        index++;
                        declarations.Add(new InitializeNode(describer, type, identifier, ParseNonEmptyExpression(false, SeperatorKind.Semicolon | SeperatorKind.Comma)));
                    }
                    else
                        declarations.Add(new DeclarationNode(describer, type, identifier));
                    
                    if (MatchCurrent(Seperator.Comma, true))
                        continue;

                    break;
                }

               
                switch (declarations.Count)
                {
                    case 1:
                        return declarations[0];
                    default:
                        return new CompoundVariableDeclarationNode(declarations);
                }
            }
        }

        private ParseNodeCollection ParseVariableAssignment(ParseNodeCollection variable, AssignmentOperator assignmentOperator, SeperatorKind breakOutSeperators)
        {
            index++;
            if(assignmentOperator.BaseOperator == null)
                return ParseMultiVariableAssignment(variable, breakOutSeperators);
            else
            {
                var expression = ParseNonEmptyExpression(false, breakOutSeperators);
                return new AssignmentNode(variable, expression); 
            }
        }

        private ParseNodeCollection ParseMultiVariableAssignment(ParseNodeCollection variable, SeperatorKind breakOutSeperators)
        {
            var expression = ParseNonEmptyExpression(true, breakOutSeperators);

            if (MatchCurrent(AssignmentOperator.Assignment))
            {
                if (!CheckVariable(expression))
                    expression = new InvalidVariableNode(PushExpectedCurrentException("Variable"), expression);

                index++;
                expression = ParseMultiVariableAssignment(expression, breakOutSeperators);
            }

            return new AssignmentNode(variable, expression);
        }

        private ImportNode ParseImportStatement()
        {
            index++;
            CreationType entityType;

            var current = Current;
            switch (current.SyntaxKind)
            {
                case SyntaxKind.Enum:
                    entityType = CreationType.Enum;
                    break;
                case SyntaxKind.Class:
                    entityType = CreationType.Class;
                    break;
                case SyntaxKind.String:
                    entityType = CreationType.Struct;
                    break;
                case SyntaxKind.Interface:
                    entityType = CreationType.Interface;
                    break;
                default:
                    entityType = CreationType.Namespace;
                    break;
            }

            if (entityType != CreationType.Namespace)
                index++;

            return new ImportNode(entityType, ParseVariable(SeperatorKind.Semicolon));
        }

        private TryCatchFinallyBlockNode ParseTryCatchBlock()
        {
            ForceMatchCurrent(Keyword.Try, true);
            TryBlockNode tryBlock = new TryBlockNode(ParseStatement(StatementEnum.GeneralStatement | StatementEnum.Scope));
            index++;

            CatchBlockNode catchBlock = null;
            FinallyBlockNode finallyBlock = null;
            if (MatchCurrent(Keyword.Catch, true))
            {
                FunctionParamatersNode arguments;
                if (MatchCurrent(Seperator.OpenBracket))
                    arguments = ParseFunctionParameters();
                else
                    arguments = new FunctionParamatersNode();

                catchBlock = new CatchBlockNode(arguments, ParseStatement(StatementEnum.GeneralStatement | StatementEnum.Scope));
                index++;
            }

            if (MatchCurrent(Keyword.Finally, true))
                finallyBlock = new FinallyBlockNode(ParseStatement(StatementEnum.GeneralStatement | StatementEnum.Scope));

            if (catchBlock == null)
            {
                if (finallyBlock == null)
                    return new InvalidTryCatchFinallyNode(PushExpectedCurrentException("catch of finally"), tryBlock);
                else
                    return new TryCatchFinallyBlockNode(tryBlock, finallyBlock);
            }
            else if (finallyBlock == null)
                return new TryCatchFinallyBlockNode(tryBlock, catchBlock);
            else
                return new TryCatchFinallyBlockNode(tryBlock, catchBlock, finallyBlock);
        }

        private DelegateDeclarationNode ParseActionDelegateDeclaration(DescriberNode describer)
        {
            ForceMatchCurrent(Keyword.Action, true);

            TryMatchCurrent(Seperator.Colon, true);
            var identifer = ParseIdentifier(true);

            TryMatchCurrent(Seperator.Lambda, true);
            return new DelegateDeclarationNode(describer, new ActionTypeNode(), identifer, ParseDelegateDeclaration(describer));
        }

        private DelegateDeclarationNode ParseFunctionDelegateDeclaration(DescriberNode describer)
        {
            ForceMatchCurrent(Keyword.Function);
            var type = new FunctionTypeNode(ParseGenericCall(true));

            index++;
            TryMatchCurrent(Seperator.Colon, true);
            var identifer = ParseIdentifier(true);

            TryMatchCurrent(Seperator.Lambda, true);
            return new DelegateDeclarationNode(describer, type, identifer, ParseDelegateDeclaration(describer));
        }

        private DelegateNode ParseDelegateDeclaration(DescriberNode describer)
        {
            FunctionParamatersNode paramaters = ParseFunctionParameters();

            GenericDeclarationNode generic = null;
            if (MatchToken(BinaryOperator.LesserThan, LookAhead(), true))
                generic = ParseGenericDeclaration();

            DelegateNode toReturn;
            if (generic == null)
                toReturn =  new DelegateNode(describer, paramaters, ParseStatement(StatementEnum.Scope));
            else
                toReturn = new DelegateNode(describer, paramaters, ParseStatement(StatementEnum.Scope), generic);

            MatchCurrent(Seperator.FlowerCloseBracket, true);
            return toReturn;
        }

        //Expressions and Entities

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
                else if(MatchCurrent(Seperator.BoxCloseBracket))
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
 
            if(MatchBreakOutSeperator(Current, finalBreakOut))
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
                        else if(MatchBreakOutSeperator(current, breakOutSeperators))
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
                        else if(current.SyntaxKind == SyntaxKind.Array)
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
                return entity;
            else
                return new InvalidVariableNode(PushExpectedCurrentException(TokenType.Identifier), entity);
        }

        /// <summary>
        /// Force Parse a Variable
        /// <para>• Ends on index of closing <paramref name="breakOutSeperators"/> </para>
        /// </summary>
        private ParseNodeCollection ParseVariable(SeperatorKind breakOutSeperators)
        {
            var entity = ParseEntity(false, breakOutSeperators);

            return ParseVariable(entity);
        }

        private bool CheckVariable(ParseNodeCollection entity)
        {
            var node = entity;
            while (true)
            {
                if (node.NodeType == ParseNodeType.Dot)
                    node = ((DotExpression)node).RHS;
                else if (node.NodeType == ParseNodeType.Variable)
                    break;
                else
                    return false;
            }

            return node.NodeType == ParseNodeType.Variable;
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

                    if (lhs != ParseNodeType.Type || lhs != ParseNodeType.Variable)
                        break;

                    node = dot.RHS;
                }
                else
                {
                    switch (node.NodeType)
                    {
                        case ParseNodeType.Type:
                            return (TypeNode)entity;
                        case ParseNodeType.Variable:
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

        //Loops
        private ParseNodeCollection ParseForLoop()
        {
            ForceMatchCurrent(LoopKeyword.For, true);
            TryMatchCurrent(Seperator.OpenBracket, true);
            var initialise = ParseStatement(StatementEnum.StandaloneStatement | StatementEnum.EmptyStatement);
   
            index++;
            var expression = ParseNonEmptyExpression(false, SeperatorKind.Semicolon);

            index++;            
            var increment = ParseMultiLine();
            var body = ParseStatement(StatementEnum.Scope | StatementEnum.GeneralStatement);

            return new ForLoopNode(initialise, expression, increment, body);

            ParseNode ParseMultiLine()
            {
                var subStatements = new List<ParseNode>();

                while (index < sourceFile.TokenCount)
                {
                    subStatements.Add(ParseStatement(true, SeperatorKind.CloseBracket | SeperatorKind.Comma));

                    switch(Current.SyntaxKind)
                    {
                        case SyntaxKind.Comma:
                            index++;
                            continue;
                        case SyntaxKind.CloseBracket:
                            index++;
                            return subStatements.Count == 1 ? subStatements[0] : new CompoundStatementNode(subStatements);
                        default:
                            if(subStatements.Peek().NodeType == ParseNodeType.Invalid)
                                return subStatements.Count == 1 ? subStatements[0] : new CompoundStatementNode(subStatements);

                            index++;
                            break;
                    }
                }

                PushExpectedCurrentException(Seperator.CloseBracket);
                return subStatements.Count == 1 ? subStatements[0] : new CompoundStatementNode(subStatements);
            }
        }

        private ParseNodeCollection ParseForeachLoop()
        {
            ForceMatchCurrent(LoopKeyword.Foreach, true);
            TryMatchCurrent(Seperator.OpenBracket, true);

            var type = ParseType(SeperatorKind.Colon);
            TryMatchCurrent(Seperator.Colon, true);

            var variable = ParseIdentifier(true);

            TryMatchCurrent(DescriberKeyword.In, true);
            var collection = ParseEntity(false, SeperatorKind.CloseBracket);

            TryMatchCurrent(Seperator.CloseBracket, true);
            var body = ParseStatement(StatementEnum.Scope | StatementEnum.GeneralStatement);

            return new ForeachLoopNode(new DeclarationNode(new DescriberNode(), type, variable), collection, body);
        }

        private ParseNodeCollection ParseWhileLoop()
        {
            ForceMatchCurrent(LoopKeyword.While, true);
            ParseNodeCollection condition = ParseCondition();

            var body = ParseStatement(StatementEnum.Scope | StatementEnum.GeneralStatement);
            return new WhileLoopNode(condition, body);
        }

        private ParseNodeCollection ParseDoWhileLoop()
        {
            ForceMatchCurrent(LoopKeyword.Do, true);

            var body = ParseStatement(StatementEnum.Scope | StatementEnum.GeneralStatement);
            index++;

            TryMatchCurrent(LoopKeyword.While, true);

            ParseNodeCollection condition = ParseCondition();
            return new DoWhileNode(condition, body);
        }

        //Conditions
        private ParseNodeCollection ParseIfCondition()
        {
            var nodes = new List<ParseNodeCollection>();

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

        private ParseNodeCollection ParseSwitchCase()
        {
            ForceMatchCurrent(ConditionKeyword.Switch, true);

            var valueToCheck = ParseCondition();
            TryMatchCurrent(Seperator.FlowerOpenBracket, true);

            Token current;
            bool breakOut = false;
            var cases = new List<ParseNode>();

            while (index < sourceFile.TokenCount)
            {
                current = Current;
                if (MatchToken(Seperator.FlowerCloseBracket, current))
                    break;

                switch (current.SyntaxKind)
                {
                    case SyntaxKind.Case:
                        index++;

                        ParseNodeCollection value;
                        current = Current;
                        switch (current.Type)
                        {
                            case TokenType.Identifier:
                                value = ParseEntity(false, SeperatorKind.Colon);

                                if (MatchType(TokenType.Identifier, LookAhead()))
                                {
                                    index++;
                                    if (LookAhead() == Keyword.When)
                                        value = ParseWhenStatement(ValidateType(current, value, true));
                                    else
                                    {
                                        PushInvalidTokenException(current);
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
                                value = new InvalidEntityNode(PushExpectedTokenException("", current), new TokenCollection(current));
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
            return new SwitchNode(valueToCheck, new CompoundStatementNode(cases));

            ParseNode ParseBody()
            {
                if (MatchCurrent(ControlKeyword.Break) || MatchCurrent(ControlKeyword.Return))
                    return new EmptyNode();

                var stmt = ParseStatement(StatementEnum.GeneralStatement | StatementEnum.Scope);

                index++;
                return stmt;
            }

            ControlStatement ParseControlStatement()
            {
                if (MatchCurrent(ControlKeyword.Return))
                    return ParseReturn();
                else if (MatchCurrent(ControlKeyword.Break, true))
                TryMatchCurrent(Seperator.Semicolon);
                else
                    PushExpectedCurrentException(ControlKeyword.Break);

                return new BreakNode();
            }

            ParseNodeCollection ParseWhenStatement(TypeNode typeNode)
            {
                var variable = ParseIdentifier();

                index++;
                ForceMatchCurrent(Keyword.When, true);
                var expression = ParseNonEmptyExpression(false, SeperatorKind.Colon);

                return new WhenNode(new DeclarationNode(new DescriberNode().AddChild(new DescriberKeywordNode(DescriberKeyword.Const)), typeNode, variable), expression);
            }
        }

        //Functions and Properties

        /// <summary>
        /// Parses Property Declaration
        /// <para>• Starts at the opening { </para>
        /// • Ends on same index as closing }
        /// </summary>
        private PropertyNode ParseProperty(DescriberNode describer, TypeNode typeNode)
        {
            TryMatchCurrent(Seperator.FlowerOpenBracket, true);

            DescriberNode accessorDescriber;
            if (MatchCurrent(Seperator.BoxOpenBracket))
                accessorDescriber = ParseDescribers();
            else
                accessorDescriber = new DescriberNode();

            GetNode getNode;
            if (MatchCurrent(Keyword.Get, true))
            {
                getNode = new GetNode(accessorDescriber, ParseStatement(StatementEnum.Scope | StatementEnum.EmptyStatement | StatementEnum.LambdaExpression));
                index++;
            }
            else
                getNode = null;

 
            SetNode setNode;
            if (MatchCurrent(Seperator.BoxOpenBracket))
            {
                accessorDescriber = ParseDescribers();

                TryMatchCurrent(Keyword.Set, true);
                setNode = ParseSetNode(accessorDescriber, typeNode);
            }
            else if (MatchCurrent(Keyword.Set, true))
            {
                accessorDescriber = new DescriberNode();
                setNode = ParseSetNode(accessorDescriber, typeNode);
            }
            else
                setNode = null;

            TryMatchCurrent(Seperator.FlowerCloseBracket);

            if (getNode != null && setNode != null)
                return new PropertyGetSetNode(describer, typeNode, getNode, setNode);
            else if (getNode != null)
                return new PropertyGetNode(describer, typeNode, getNode);
            else
                return new PropertySetNode(describer, typeNode, setNode);

            SetNode ParseSetNode(DescriberNode describer, TypeNode type)
            {
                var set = new SetNode(describer, new DeclarationNode(new DescriberNode().AddChild(new DescriberKeywordNode(DescriberKeyword.Const)), type, IdentifierNode.ValueIdentifier), ParseStatement(StatementEnum.Scope | StatementEnum.EmptyStatement | StatementEnum.LambdaStatement));
                index++;

                return set;
            }
        }

        /// <summary>
        /// Parses Property Declaration
        /// <para>• Starts at the opening ( </para>
        /// • Ends on same index after closing ) or GREATERTHAN in case of generic
        /// </summary>
        private ParseNodeCollection ParseConstructorCall(bool inExpression)
        {
            ForceMatchCurrent(Keyword.Create, true);
            var name = ParseEntity(inExpression, SeperatorKind.OpenBracket);

            var arguments = ParseFunctionArguments();

            GenericCallNode generic = null;
            if (LookAhead() == BinaryOperator.LesserThan)
                generic = ParseGenericCall(!inExpression);

            ExpressionListNode expressions = null;
            if (MatchToken(Seperator.FlowerOpenBracket, LookAhead(), true))
                expressions = ParseExpressionList(SeperatorKind.FlowerCloseBracket);

            if (generic == null)
            {
                if (expressions == null)
                    return new ConstructorCallNode(name, arguments);
                else
                    return new ConstructorCallNode(name, arguments, expressions);
            }
            else if (expressions == null)
                return new ConstructorCallNode(name, arguments, generic);
            else
                return new ConstructorCallNode(name, arguments, generic, expressions);
        }

        /// <summary>
        /// Parses Property Declaration
        /// <para>• Starts at the opening ( </para>
        /// • Ends on same index after closing ) or GREATERTHAN in case of generic
        /// </summary>
        private ParseNodeCollection ParseFunctionCall(bool inExpression, ParseNodeCollection functionName)
        {
            var arguments = ParseFunctionArguments();

            GenericCallNode generic = null;
            if (LookAhead() == BinaryOperator.LesserThan)
                generic = ParseGenericCall(!inExpression);

            if (generic == null)
                return new FunctionCallNode(functionName, arguments);
            else
                return new FunctionCallNode(functionName, arguments, generic);
        }

        private ParseNodeCollection ParsePrintCall()
        {
            ForceMatchCurrent(Keyword.Print);
            var keyword = (Keyword)Current;

            index++;
            return new InputNode(keyword, ParseFunctionArguments(true));
        }

        private ParseNodeCollection ParseInputCall(bool inExpression)
        {
            ForceMatchCurrent(Keyword.Input);
            var keyword = (Keyword)Current;

            index++;
            return new PrintNode(keyword, ParseFunctionArguments(!inExpression));
        }

        private FunctionDeclarationNode ParseFunctionDeclaration(DescriberNode describer, TypeNode returnType)
        {
            IdentifierNode name = ParseIdentifier(true);
            var arguments = ParseFunctionParameters();

            GenericDeclarationNode generic = null;            
            if (MatchCurrent(BinaryOperator.LesserThan))
                generic = ParseGenericDeclaration();

            TypeNode extension = null;
            if (MatchCurrent(Seperator.Colon, true))
                extension = ParseType(SeperatorKind.FlowerOpenBracket | SeperatorKind.Lambda);

            var body = ParseStatement(StatementEnum.Scope | (returnType.Type == TypeNodeEnum.Void ? StatementEnum.LambdaStatement : StatementEnum.LambdaExpression));

            if (generic == null)
            {
                if(extension == null)
                    return new FunctionDeclarationNode(describer, returnType, name, arguments, body);
                else
                    return new ExtensionFunctionDeclarationNode(describer, returnType, name, arguments, body, extension);
            }
            else
            {
                if (extension == null)
                    return new FunctionDeclarationNode(describer, returnType, name, arguments, body, generic);
                else
                    return new ExtensionFunctionDeclarationNode(describer, returnType, name, arguments, body, extension, generic);
            }
        }

        private ConstructorDeclarationNode ParseConstructorDeclaration(DescriberNode describer, TypeNode returnType)
        {
            var arguments = ParseFunctionParameters();

            ExpressionListNode extension = null;
            if(MatchCurrent(Seperator.Colon, true))
            {
                TryMatchCurrent(Keyword.Parent, true);
                TryMatchCurrent(Seperator.OpenBracket, true);

                extension = ParseExpressionList(SeperatorKind.CloseBracket, true);
            }

            var body = ParseStatement(StatementEnum.Scope | StatementEnum.LambdaStatement);

            if (extension == null)
                return new ConstructorDeclarationNode(describer, returnType, arguments, body);
            else
                return new ConstructorDeclarationNode(describer, returnType, arguments, body, extension);
        }

        private ParseNodeCollection ParseOperatorOverload(DescriberNode describer)
        {
            ForceMatchCurrent(FunctionKeyword.Operator, true);
            var returnType = ParseType(SeperatorKind.None);

            index++;
            Operator operatorToOverload = null;
            if (TryMatchCurrentType(TokenType.Operator, false))
            {
                operatorToOverload = Current as Operator;
                index++;
            }

            var arguments = ParseFunctionParameters();

            GenericDeclarationNode generic = null;
            if (MatchToken(BinaryOperator.LesserThan, LookAhead(), true))
                generic = ParseGenericDeclaration();

            var body = ParseStatement(StatementEnum.Scope | StatementEnum.LambdaExpression);
     
            if (generic == null)
                return new OperatorOverloadFunctionDeclarationNode(describer, returnType, arguments, body, operatorToOverload);
            else
                return new OperatorOverloadFunctionDeclarationNode(describer, returnType, arguments, body, operatorToOverload, generic);
        }

        private BaseFunctionDeclarationNode ParseConversionOverload(DescriberNode describer)
        {
            var keyword = Current;
            index++;

            var returnType = ParseType(SeperatorKind.OpenBracket);
            var arguments = ParseFunctionParameters();

            GenericDeclarationNode generic = null;
            if (MatchToken(BinaryOperator.LesserThan, LookAhead(), true))
                generic = ParseGenericDeclaration();

            var body = ParseStatement(StatementEnum.Scope | StatementEnum.LambdaExpression);

            if (keyword == FunctionKeyword.Explicit)
            {
                if (generic == null)
                    return new ExplicitCastDeclarationNode(describer, returnType, arguments, body);
                else
                    return new ExplicitCastDeclarationNode(describer, returnType, arguments, body, generic);
            }
            else
            {
                if (generic == null)
                    return new ImplicitCastDeclarationNode(describer, returnType, arguments, body);
                else
                    return new ImplicitCastDeclarationNode(describer, returnType, arguments, body, generic);
            }
        }

        private ParseNodeCollection ParseIndexerDeclaration(DescriberNode describer)
        {
            ForceMatchCurrent(FunctionKeyword.Indexer, true);
            var returnType = ParseType(SeperatorKind.OpenBracket);

            var arguments = ParseFunctionParameters();

            GenericDeclarationNode generic = null;
            if (MatchToken(BinaryOperator.LesserThan, LookAhead(), true))
                generic = ParseGenericDeclaration();

            var body = ParseProperty(describer, returnType);

            if(generic == null)
                return new IndexerDeclarationNode(describer, returnType, arguments, body);
            else
                return new IndexerDeclarationNode(describer, returnType, arguments, body, generic);
        }

        /// <summary>
        /// Parses Function Arguments (Calling)
        /// <para>• Starts at the opening ( </para>
        /// • Ends one index after closing ) if <paramref name="increment"/> is true
        /// </summary>
        private FunctionArgumentsNode ParseFunctionArguments(bool increment = false)
        {
            TryMatchCurrent(Seperator.OpenBracket, true);

            var arguments = new FunctionArgumentsNode();
            if (MatchCurrent(Seperator.CloseBracket))
                return arguments;

            while (index < sourceFile.TokenCount)
            {
                if (MatchCurrent(Seperator.BoxOpenBracket))
                {
                    DescriberNode describer = ParseDescribers();

                    arguments.AddChild(new FunctionArgumentNode(describer, ParseNonEmptyExpression(false, SeperatorKind.Comma | SeperatorKind.CloseBracket)));
                }
                else
                {
                    arguments.AddChild(new FunctionArgumentNode(new DescriberNode(), ParseNonEmptyExpression(false, SeperatorKind.Comma | SeperatorKind.CloseBracket)));
                }

                if (MatchCurrent(Seperator.CloseBracket))
                {
                    break;
                }

                index++;
            }

            TryMatchCurrent(Seperator.CloseBracket, increment);
            return arguments;
        }

        /// <summary>
        /// Parses Function Paramaters (Declaration)
        /// <para>• Starts at the opening ( </para>
        /// • Ends one index after closing )
        /// </summary>
        private FunctionParamatersNode ParseFunctionParameters()
        {
            TryMatchCurrent(Seperator.OpenBracket, true);

            var arguments = new FunctionParamatersNode();
            if (MatchCurrent(Seperator.CloseBracket, true))
                return arguments;

            while (index < sourceFile.TokenCount)
            {
                DescriberNode describer;
                if (MatchCurrent(Seperator.BoxOpenBracket))
                    describer = ParseDescribers();
                else
                    describer = new DescriberNode();

                var type = ParseType(SeperatorKind.Colon);
                TryMatchCurrent(Seperator.Colon, true);

                var variable = ParseIdentifier();
                if (LookAhead() == AssignmentOperator.Assignment)
                {
                    index += 2;
                    arguments.AddChild(new FunctionParamaterNode(describer, type, variable, ParseNonEmptyExpression(false, SeperatorKind.Comma | SeperatorKind.CloseBracket)));

                    index--;
                }
                else
                    arguments.AddChild(new FunctionParamaterNode(describer, type, variable));

                index++;
  
                if (MatchCurrent(Seperator.Comma, true))
                    continue;
                else if (MatchCurrent(Seperator.CloseBracket))
                    break;
                else
                    index--;
                break;
            }

            TryMatchCurrent(Seperator.CloseBracket, true);
            return arguments;
        }

        //User Defined Types and Namespaces
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

        //Error Handling
        private InvalidTokenException PushInvalidCurrentException() => PushInvalidTokenException(Current);
        private InvalidTokenException PushInvalidTokenException(Token token)
        {
            var exception = new InvalidTokenException(token);
            sourceFile.PushException(exception);

            return exception;
        }

        private InvalidStatementException PushInvalidStatementException()
        {
            var exception = new InvalidStatementException(index);
            sourceFile.PushException(exception);

            return exception;
        }

        private TokenExpectedException PushExpectedCurrentException(TokenType expected) => PushExpectedTokenException(expected, Current);
        private TokenExpectedException PushExpectedTokenException(TokenType expected, Token token) => PushException(new TokenExpectedException(expected, token));

        private TokenExpectedException PushExpectedCurrentException(Token expected) => PushExpectedTokenException(expected, Current);
        private TokenExpectedException PushExpectedTokenException(Token expected, Token token) => PushException(new TokenExpectedException(expected, token));

        private TokenExpectedException PushExpectedCurrentException(string expected) => PushExpectedTokenException(expected, Current);
        private TokenExpectedException PushExpectedTokenException(string expected, Token token) => PushException(new TokenExpectedException(expected, token));

        private Exception PushException<Exception>(Exception exception) where Exception : CompileException
        {
            sourceFile.PushException(exception);

            return exception;
        }

        /// <summary>
        /// Returns a TokenCollection containing all Tokens until
        /// <para>1. Expected <paramref name="breakOutSeperators"/> is found (inclusive)</para>
        /// 2. Default Seperator: ; { } is found (exclusive, overrides <paramref name="breakOutSeperators"/>)
        /// </summary>
        private TokenCollection PanicModeSimple(SeperatorKind breakOutSeperators)
        {
            var invalid = new List<Token>();
            var extra = SeperatorKind.Semicolon | SeperatorKind.FlowerOpenBracket | SeperatorKind.FlowerCloseBracket;
            var match = breakOutSeperators | extra;

            while (index < sourceFile.TokenCount)
            {
                var current = Current;

                if (MatchBreakOutSeperator(current, match))
                {
                    if (MatchBreakOutSeperator(current, extra) && !MatchBreakOutSeperator(current, breakOutSeperators))
                        index--;

                    break;
                }

                invalid.Add(current);
                index++;
            }

            return new TokenCollection(invalid);
        }
    }
}