using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Analysis.ProjectStructure.LocalNodes.Variables;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Variables
{
    internal interface ILocalVariableParent : IVariableParent<ILocalVariableParent, LocalVariableNode>
    {
        public ILocalVariableParent AddVariable(LocalVariableNode variable);
        public LocalVariableNode TryFindVariable(IdentifierNode identifier);
    }
}
