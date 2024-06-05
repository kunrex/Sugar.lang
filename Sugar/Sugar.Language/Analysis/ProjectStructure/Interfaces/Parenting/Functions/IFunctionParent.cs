using System;

using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Functions;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes.Functions;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Functions
{
    internal interface IFunctionParent : IParent
    {

    }

    internal interface IFunctionParent<This, Void, Method> : IFunctionParent where This : IFunctionParent<This, Void, Method> where Void : VoidCreationNode<This>, IFunction where Method : MethodCreationNode<This>, IMethod
    {

    }
}
