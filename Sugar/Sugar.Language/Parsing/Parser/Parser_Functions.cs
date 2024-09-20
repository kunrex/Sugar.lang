using System;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Keywords;
using Sugar.Language.Tokens.Operators;
using Sugar.Language.Tokens.Seperators;
using Sugar.Language.Tokens.Operators.Binary;
using Sugar.Language.Tokens.Operators.Assignment;
using Sugar.Language.Tokens.Keywords.Subtypes.Functions;
using Sugar.Language.Tokens.Keywords.Subtypes.Describers;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Parsing.Parser.Enums;

using Sugar.Language.Parsing.Nodes.Describers;
    
using Sugar.Language.Parsing.Nodes.NodeGroups;

using Sugar.Language.Parsing.Nodes.Types;
using Sugar.Language.Parsing.Nodes.Types.Enums;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Values.Generics;

using Sugar.Language.Parsing.Nodes.Statements.VariableCreation;

using Sugar.Language.Parsing.Nodes.Functions.Calling;
using Sugar.Language.Parsing.Nodes.Functions.Properties;
using Sugar.Language.Parsing.Nodes.Functions.Declarations;
using Sugar.Language.Parsing.Nodes.Functions.Calling.Structure;
using Sugar.Language.Parsing.Nodes.Functions.Properties.Accessors;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.OperatorOverloading;

namespace Sugar.Language.Parsing.Parser
{
    internal sealed partial class Parser
    {
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
    }
}