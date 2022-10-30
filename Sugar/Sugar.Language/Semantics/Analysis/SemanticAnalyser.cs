using System;

using Sugar.Language.Parsing;

using Sugar.Language.Semantics.ActionTrees;
using Sugar.Language.Semantics.ActionTrees.Namespaces;
using Sugar.Language.Semantics.Services.Implementations;

namespace Sugar.Language.Semantics.Analysis
{
    internal sealed class SemanticAnalyser
    {
        private SugarPackage package;

        public SyntaxTreeCollection Collection { get; private set; }
        public SyntaxTreeCollection InternalDataTypes { get; private set; }

        private DefaultNameSpaceNode defaultNameSpace;
        private CreatedNameSpaceCollectionNode createdNameSpaces;

        public SemanticAnalyser(SyntaxTreeCollection _internalDataTypes, SyntaxTreeCollection _collection)
        {
            Collection = _collection;
            InternalDataTypes = _internalDataTypes;
        }

        public SugarPackage Analyse()
        {
            var collections = new NameSpaceStructureService(InternalDataTypes, Collection).Validate();

            defaultNameSpace = (DefaultNameSpaceNode)collections.Results[0];
            createdNameSpaces = (CreatedNameSpaceCollectionNode)collections.Results[1];

            package = new SugarPackage(defaultNameSpace, createdNameSpaces);
            var import = new ImportStatementService(defaultNameSpace, createdNameSpaces).Validate();
            defaultNameSpace.Print("", true);
            createdNameSpaces.Print("", true);
            var classMemberCreation = new ClassMemberService(defaultNameSpace, createdNameSpaces).Validate();

            var statementValidation = new StatementService(defaultNameSpace, createdNameSpaces).Validate();

            Console.WriteLine(import);
            Console.WriteLine(classMemberCreation);
            Console.WriteLine(statementValidation);

            return package;
        }
    }
}
