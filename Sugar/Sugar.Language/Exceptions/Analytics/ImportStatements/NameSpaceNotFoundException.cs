using System;

using Sugar.Language.Parsing.Nodes.Statements;

namespace Sugar.Language.Exceptions.Analytics.ImportStatements
{
    internal sealed class NameSpaceNotFoundException : ImportStatementException
    {
        public NameSpaceNotFoundException(ImportNode _importNode) : base(_importNode, $"Namespace: '{ToString(_importNode)} not found'")
        {

        }
    }
}
