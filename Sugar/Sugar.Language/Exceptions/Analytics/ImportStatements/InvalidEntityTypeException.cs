using System;

using Sugar.Language.Parsing.Nodes.Statements;

using Sugar.Language.Semantics.ActionTrees.Enums;

namespace Sugar.Language.Exceptions.Analytics.ImportStatements
{
    internal sealed class InvalidEntityTypeException : ImportStatementException
    {
        public InvalidEntityTypeException(ImportNode _importNode, DataTypeEnum type, DataTypeEnum expected) : base(_importNode, $"'{ToString(_importNode)} is a {type} and not a(n) {expected}")
        {

        }
    }
}
