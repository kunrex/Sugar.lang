using System;
using System.Collections.Generic;

using Sugar.Language.Tokens.Enums;

using Sugar.Language.Parsing;
using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.UDDataTypes;
using Sugar.Language.Parsing.Nodes.Expressions.Associative;

using Sugar.Language.Semantics.ActionTrees;
using Sugar.Language.Semantics.ActionTrees.Namespaces;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Collections;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.UDDataTypes.Enums;
using Sugar.Language.Semantics.Analysis.Structure;
using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Parsing.Nodes.Statements.VariableCreation;
using Sugar.Language.Parsing.Nodes.Types.Subtypes;
using Sugar.Language.Parsing.Nodes.Describers;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation;
using Sugar.Language.Parsing.Nodes.Expressions;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation;
using Sugar.Language.Parsing.Nodes.Functions.Properties;
using Sugar.Language.Parsing.Nodes.Functions.Declarations;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global;
using Sugar.Language.Semantics.ActionTrees.CreationStatements;
using Sugar.Language.Semantics.ActionTrees.Interfaces;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;
using Sugar.Language.Semantics.Services.Implementations;

namespace Sugar.Language.Semantics.Analysis
{
    internal sealed class SemanticAnalyser
    {
        private SugarPackage package;
        public SyntaxTreeCollection Collection { get; private set; }

        private DefaultNameSpaceNode defaultNameSpace;
        private CreatedNameSpaceCollectionNode createdNameSpaces;

        public SemanticAnalyser(SyntaxTreeCollection _collection)
        {
            Collection = _collection;
        }

        public SugarPackage Analyse()
        {
            var collections = new NameSpaceStructureService(Collection).Validate();

            defaultNameSpace = (DefaultNameSpaceNode)collections.Results[0];
            createdNameSpaces = (CreatedNameSpaceCollectionNode)collections.Results[1];


            package = new SugarPackage(defaultNameSpace, createdNameSpaces);
            var import = new ImportStatementService(defaultNameSpace, createdNameSpaces).Validate();
            defaultNameSpace.Print("", true);
            createdNameSpaces.Print("", true);
            var classMemberCreation = new ClassMemberService(defaultNameSpace, createdNameSpaces).Validate();



            Console.WriteLine(import);
            Console.WriteLine(classMemberCreation);

            return package;
        }
    }
}
