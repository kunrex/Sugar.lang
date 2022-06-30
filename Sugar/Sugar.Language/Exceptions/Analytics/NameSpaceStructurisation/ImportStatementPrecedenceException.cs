using System;

using Sugar.Language.Parsing.Nodes.Statements;

namespace Sugar.Language.Exceptions.Analytics.NameSpaceStructurisation
{
    internal class ImportStatementPrecedenceException : ImportStatementCompileException
    {
        public ImportStatementPrecedenceException(ImportNode importNode) : base("Import statements must preceed all definitions", GetIndex(importNode))
        {

        }
    }
}
