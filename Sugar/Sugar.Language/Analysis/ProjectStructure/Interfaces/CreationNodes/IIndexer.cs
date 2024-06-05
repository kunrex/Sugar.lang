using System;

using Sugar.Language.Services.Interfaces;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions.Structure;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes
{
    internal interface IIndexer : IProperty, ICustomCollection<FunctionArgument>
    {
        public GlobalMemberEnum PropertyType { get; }

        public IIndexer AddArgument(FunctionArgument argument);
        public FunctionArgument FindArgument(IdentifierNode identifier);
    }
}
