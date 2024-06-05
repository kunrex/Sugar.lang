using System;

using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Functions;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes.Functions;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Functions
{
    internal interface IFunctionParentable<Parent, Void, Method> : IParentableNode<Parent> where Parent : IFunctionParent<Parent, Void, Method> where Void : VoidCreationNode<Parent>, IFunction where Method : MethodCreationNode<Parent>, IMethod
    {

    }
}
