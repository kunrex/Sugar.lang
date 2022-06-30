using System;
using System.Text;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.Expressions.Associative;

namespace Sugar.Language.Exceptions.Analytics.ImportStatements
{
    internal abstract class ImportStatementException : ImportStatementCompileException
    {
        public ImportStatementException(ImportNode _importNode, string exception) : base(exception, GetIndex(_importNode))
        {
           
        }

        protected static string ToString(ImportNode importNode)
        {
            var builder = new StringBuilder();

            var current = importNode.Name;

            while(current != null)
            {
                switch(current.NodeType)
                {
                    case NodeType.Dot:
                        var dotExpression = (DotExpression)current;

                        builder.Append(((IdentifierNode)dotExpression.LHS).Value);
                        current = dotExpression.RHS;
                        break;
                    case NodeType.Variable:
                        builder.Append(((IdentifierNode)current).Value);
                        current = null;
                        break;
                }

                builder.Append(".");
            }

            return builder.ToString();
        }
    }
}
