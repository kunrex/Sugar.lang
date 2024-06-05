using System;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Variables
{
    internal interface IVariableParent<This, Variable> : IParent where This : IVariableParent<This, Variable> where Variable : IParentableNode<This>
    {
        
    }
}
