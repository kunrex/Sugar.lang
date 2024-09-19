using System;
using System.Collections.Generic;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Seperators;
using Sugar.Language.Tokens.Keywords.Subtypes.Loops;
using Sugar.Language.Tokens.Keywords.Subtypes.Describers;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Loops;

using Sugar.Language.Parsing.Parser.Enums;

using Sugar.Language.Parsing.Nodes.NodeGroups;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Statements.VariableCreation;

namespace Sugar.Language.Parsing.Parser
{
    internal sealed partial class Parser
    {
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
                            if(subStatements[subStatements.Count - 1].NodeType == ParseNodeType.Invalid)
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
    }
}