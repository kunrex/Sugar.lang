using System;
using System.Collections.Generic;

using Sugar.Language.Tokens;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Keywords;
using Sugar.Language.Tokens.Constants;
using Sugar.Language.Tokens.Seperators;
using Sugar.Language.Tokens.Keywords.Subtypes.Describers;
using Sugar.Language.Tokens.Keywords.Subtypes.Conditions;
using Sugar.Language.Tokens.Keywords.Subtypes.ControlStatements;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Parsing.Nodes.Types;

using Sugar.Language.Parsing.Parser.Enums;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.NodeGroups;

using Sugar.Language.Parsing.Nodes.CtrlStatements;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Values.Invalid;

using Sugar.Language.Parsing.Nodes.Statements.VariableCreation;

using Sugar.Language.Parsing.Nodes.Conditions.IfConditions;
using Sugar.Language.Parsing.Nodes.Conditions.SwitchConditions;

namespace Sugar.Language.Parsing.Parser
{
    internal sealed partial class Parser
    {
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
                if (MatchCurrent(ControlKeyword.Break, true))
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
                var expression = ParseNonEmptyExpression(SeperatorKind.Colon);

                return new WhenNode(new DeclarationNode(new DescriberNode().AddChild(new DescriberKeywordNode(DescriberKeyword.Const)), typeNode, variable), expression);
            }
        }
    }
}