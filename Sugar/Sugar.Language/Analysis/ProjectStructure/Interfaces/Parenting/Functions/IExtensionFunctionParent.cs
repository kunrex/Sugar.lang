using System;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions.Extensions;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Functions
{
    internal interface IExtensionFunctionParent : IGlobalFunctionParent
    {
        public IGlobalFunctionParent AddExtensionVoid(ExtensionVoidNode extensionVoid);
        public IGlobalFunctionParent AddExtensionMethod(ExtensionMethodNode extensionMethod);

        public ExtensionVoidNode TryFindExtensionVoid(IdentifierNode identifier, FunctionParamatersNode parameters);
        public ExtensionMethodNode TryFindExtensionMethod(IdentifierNode identifier, FunctionParamatersNode parameters);
    }
}
