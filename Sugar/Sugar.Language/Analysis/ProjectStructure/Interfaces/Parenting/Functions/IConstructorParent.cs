using System;

using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Functions
{
    internal interface IConstructorParent : IFunctionParent
    {
        public IConstructorParent AddConstructor(ConstructorNode constructor);
        public ConstructorNode TryFindGlobalConstructor(FunctionParamatersNode parameters);
    }
}
