using System;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Functions
{
    internal interface IGlobalFunctionParent : IFunctionParent<IGlobalFunctionParent, GlobalVoidNode, GlobalMethodNode>
    {
        public IGlobalFunctionParent AddGlobalVoid(GlobalVoidNode globalVoid);
        public GlobalVoidNode TryFindGlobalVoid(IdentifierNode identifier, FunctionParamatersNode parameters);

        public IGlobalFunctionParent AddGlobalMethod(GlobalMethodNode globalMethod);
        public GlobalMethodNode TryFindGlobalMethod(IdentifierNode identifier, FunctionParamatersNode parameters);
    }
}
