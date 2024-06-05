using System;

using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions.Casting;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Functions
{
    internal interface ICastParent : IFunctionParent
    {
        public IConstructorParent AddImplicitCast(ImplicitCastNode implicitCast);
        public ImplicitCastNode TryFindImplicitCast(DataType type, FunctionParamatersNode parameters);

        public IConstructorParent AddExplicitCast(ExplicitCastNode explicitCast);
        public ExplicitCastNode TryFindExplicitCast(DataType type, FunctionParamatersNode parameters);
    }
}
