using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Services.Interfaces;

using Sugar.Language.Analysis.ProjectStructure.LocalNodes;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Scope;

using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions.Structure;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes.Functions
{
    internal interface IFunction : ICreationNode, ICustomCollection<FunctionArgument>, IScopeParent, IBody
    {
        public Scope Scope { get; }

        public IFunction AddArgument(FunctionArgument argument);
        public FunctionArgument FindArgument(IdentifierNode identifier);
    }
}
