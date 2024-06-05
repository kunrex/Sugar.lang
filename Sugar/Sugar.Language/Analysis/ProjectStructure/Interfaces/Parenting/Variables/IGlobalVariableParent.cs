using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Variables;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Variables
{
    internal interface IGlobalVariableParent : IVariableParent<IGlobalVariableParent, GlobalVariableNode>
    {
        public IGlobalVariableParent AddVariable(GlobalVariableNode variable);
        public GlobalVariableNode TryFindVariable(IdentifierNode identifier);
    }
}
