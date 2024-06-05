using System;

using Sugar.Language.Tokens.Operators;

using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Functions
{
    internal interface IOperatorOverloadParent : IFunctionParent
    {
        public IConstructorParent AddOverload(OperatorOverloadNode overload);
        public OperatorOverloadNode TryFindOverload(Operator overload, FunctionParamatersNode parameters);
    }
}
