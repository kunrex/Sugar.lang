using System;
using System.Collections.Generic;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Keywords;
using Sugar.Language.Tokens.Operators;
using Sugar.Language.Tokens.Seperators;
using Sugar.Language.Tokens.Operators.Unary;
using Sugar.Language.Tokens.Operators.Binary;
using Sugar.Language.Tokens.Operators.Assignment;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Parsing.Parser.Enums;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.NodeGroups;

using Sugar.Language.Parsing.Nodes.CtrlStatements;

using Sugar.Language.Parsing.Nodes.DataTypes.Enums;

using Sugar.Language.Parsing.Nodes.Types;
using Sugar.Language.Parsing.Nodes.Types.Subtypes;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Values.Generics;
using Sugar.Language.Parsing.Nodes.Values.Invalid;

using Sugar.Language.Parsing.Nodes.Declarations.Delegates;

using Sugar.Language.Parsing.Nodes.TryCatchFinally;
using Sugar.Language.Parsing.Nodes.TryCatchFinally.Blocks;

using Sugar.Language.Parsing.Nodes.Expressions;
using Sugar.Language.Parsing.Nodes.Expressions.Associative;
using Sugar.Language.Parsing.Nodes.Expressions.Operative;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.Statements.VariableCreation;
using Sugar.Language.Parsing.Nodes.Statements.VariableCreation.Properties;

namespace Sugar.Language.Parsing.Parser
{
    internal sealed partial class Parser
    {
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

            switch (current.SyntaxKind)
            {
                case SyntaxKind.FlowerOpenBracket:
                    if (!Match(StatementEnum.Scope))
                        return new InvalidEntityNode(PushInvalidCurrentException(),PanicModeSimple(SeperatorKind.None));

                    return ParseScope();
                case SyntaxKind.Lambda:
                    index++;
                    if (Match(StatementEnum.LambdaStatement))
                        return ParseStatement(false, SeperatorKind.Semicolon);
                    if (Match(StatementEnum.LambdaExpression))
                        return new LambdaNode(ParseNonEmptyExpression(SeperatorKind.Semicolon));
                    
                    return new InvalidStatementNode(PushInvalidStatementException(), PanicModeSimple(SeperatorKind.None));
                default:
                    if (!Match(StatementEnum.StandaloneStatement))
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
                    if (TryMatchCurrent(Seperator.BoxOpenBracket))
                        return ParseDescriberStatement(breakOutSeperators);
                    
                    return new InvalidStatementNode(PushInvalidStatementException(), PanicModeSimple(breakOutSeperators));
                default:
                    return new InvalidStatementNode(PushInvalidStatementException(), PanicModeSimple(breakOutSeperators));
            }
        }

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

                if (MatchType(TokenType.Keyword, current))
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
                    toReturn = new ThrowExceptionNode(ParseNonEmptyExpression(SeperatorKind.Semicolon));
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
                    toReturn = ParseConstructorDeclaration(new DescriberNode(),
                        new ConstructorTypeNode(ParseType(SeperatorKind.OpenBracket)));
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
                    return ParseConstructorDeclaration(describer,
                        new ConstructorTypeNode(ParseType(SeperatorKind.OpenBracket)));
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
                case ParseNodeType.Identifier:
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
                            if (next.SyntaxKind == SyntaxKind.Lambda)
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
            switch (node.NodeType)
            {
                case ParseNodeType.PropertyInitialise:
                case ParseNodeType.PropertyDeclaration:
                case ParseNodeType.FunctionDeclaration:
                    return true;
                default:
                    return false;
            }
        }

        private ParseNode ParseDeclaration(DescriberNode describer) =>
            ParseDeclaration(describer, ParseType(SeperatorKind.None));

        private ParseNode ParseDeclaration(DescriberNode describer, TypeNode typeNode)
        {
            index++;

            if (MatchCurrent(Seperator.Colon, true))
                return ParseVariableDeclaration(describer, typeNode);
            if (MatchCurrentType(TokenType.Identifier))
                return ParseFunctionDeclaration(describer, typeNode);
            
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
                    return new PropertyInitialisationNode(describer, type, identifier, property, ParseNonEmptyExpression(SeperatorKind.Semicolon));

                }
                
                return new PropertyDeclarationNode(describer, type, identifier, property);
            }
            
            List<DeclarationNode> declarations = new List<DeclarationNode>();

            while (index < sourceFile.TokenCount)
            {
                IdentifierNode identifier = ParseIdentifier(true);

                if (MatchCurrent(AssignmentOperator.Assignment))
                {
                    index++;
                    declarations.Add(new InitializeNode(describer, type, identifier, ParseNonEmptyExpression(SeperatorKind.Semicolon | SeperatorKind.Comma)));
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

        private ParseNodeCollection ParseVariableAssignment(ParseNodeCollection variable, AssignmentOperator assignmentOperator, SeperatorKind breakOutSeperators)
        {
            index++;
            var expression = ParseNonEmptyExpression(breakOutSeperators);
            int x = 10;
            int y = 20;
            int z = 10;
            x += y = z;
            if (assignmentOperator.BaseOperator == null)
                return new AssignmentNode(variable, expression);
            else
                return new AssignmentNode(variable, new BinaryExpression(assignmentOperator.BaseOperator, variable, expression));
        }

        private ImportNode ParseImportStatement()
        {
            index++;
            CreationType entityType;

            var current = Current;
            switch (current.SyntaxKind)
            {
                case SyntaxKind.Enum:
                    index++;
                    entityType = CreationType.Enum;
                    break;
                case SyntaxKind.Class:
                    index++;
                    entityType = CreationType.Class;
                    break;
                case SyntaxKind.String:
                    index++;
                    entityType = CreationType.Struct;
                    break;
                case SyntaxKind.Interface:
                    index++;
                    entityType = CreationType.Interface;
                    break;
                default:
                    entityType = CreationType.Namespace;
                    break;
            }

            return new ImportNode(entityType, ParseVariable(SeperatorKind.Semicolon));
        }

        private TryCatchFinallyBlockNode ParseTryCatchBlock()
        {
            ForceMatchCurrent(Keyword.Try, true);
            TryBlockNode tryBlock =
                new TryBlockNode(ParseStatement(StatementEnum.GeneralStatement | StatementEnum.Scope));
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
                finallyBlock =
                    new FinallyBlockNode(ParseStatement(StatementEnum.GeneralStatement | StatementEnum.Scope));

            if (catchBlock == null)
            {
                if (finallyBlock == null)
                    return new InvalidTryCatchFinallyNode(PushExpectedCurrentException("catch of finally"), tryBlock);
                
                return new TryCatchFinallyBlockNode(tryBlock, finallyBlock);
            }
            if (finallyBlock == null)
                return new TryCatchFinallyBlockNode(tryBlock, catchBlock);
            
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
                toReturn = new DelegateNode(describer, paramaters, ParseStatement(StatementEnum.Scope));
            else
                toReturn = new DelegateNode(describer, paramaters, ParseStatement(StatementEnum.Scope), generic);

            MatchCurrent(Seperator.FlowerCloseBracket, true);
            return toReturn;
        }
    }
}