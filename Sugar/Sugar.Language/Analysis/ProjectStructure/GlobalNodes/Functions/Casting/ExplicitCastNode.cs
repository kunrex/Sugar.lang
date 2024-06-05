using System;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Nodes;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

namespace Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions.Casting
{
    internal sealed class ExplicitCastNode : BaseCastNode, IGlobalNode
    {
        public GlobalMemberEnum GlobalMember { get => GlobalMemberEnum.ExplicitCast; }

        public ExplicitCastNode(DataType _returnType, Describer _describer, ParseNodeCollection _body) : base(_returnType, _describer, _body)
        {

        }
    }
}
