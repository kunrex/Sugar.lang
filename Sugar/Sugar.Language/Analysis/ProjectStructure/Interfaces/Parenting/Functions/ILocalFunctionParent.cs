using System;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

using Sugar.Language.Analysis.ProjectStructure.LocalNodes.Functions;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Functions
{
    internal interface ILocalFunctionParent : IFunctionParent<ILocalFunctionParent, LocalVoidNode, LocalMethodNode>
    {
        public ILocalFunctionParent AddLocalVoid(LocalVoidNode localVoid);
        public LocalVoidNode TryFindLocalVoid(IdentifierNode identifier, FunctionParamatersNode arguments);

        public ILocalFunctionParent AddLocalMethod(LocalMethodNode localMethod);
        public LocalMethodNode TryFindLocalMethod(IdentifierNode identifier, FunctionParamatersNode arguments);
    }
}
