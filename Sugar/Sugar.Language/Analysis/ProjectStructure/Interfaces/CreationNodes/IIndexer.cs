using System;

using Sugar.Language.Services.Interfaces;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Functions.Structure;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes
{
    internal interface IIndexer : IProperty, ICustomCollection<FunctionParameter>
    {
        public GlobalMemberEnum PropertyType { get; }

        public IIndexer AddArgument(FunctionParameter _parameter);
        public FunctionParameter FindArgument(IdentifierNode identifier);
    }
}
