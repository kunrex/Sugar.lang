using System;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Nodes;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

namespace Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions.Casting
{
    internal sealed class ImplicitCastNode : BaseCastNode, IGlobalNode
    {
        public GlobalMemberEnum GlobalMember { get => GlobalMemberEnum.ImplicitCast; }

        public ImplicitCastNode(DataType _returnType, Describer _describer, ParseNode _body) : base(_returnType, _describer, _body)
        {

        }        
    }
}
