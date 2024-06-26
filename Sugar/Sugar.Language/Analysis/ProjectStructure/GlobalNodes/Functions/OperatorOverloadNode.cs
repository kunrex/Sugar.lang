﻿using System;

using Sugar.Language.Tokens.Operators;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Functions;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Nodes;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Functions;

namespace Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions
{
    internal sealed class OperatorOverloadNode : MethodCreationNode<IOperatorOverloadParent>, IGlobalNode
    {
        public override MemberTypeEnum MemberType { get => MemberTypeEnum.Global; }
        public GlobalMemberEnum GlobalMember { get => GlobalMemberEnum.OperaterOverload; }

        private readonly Operator baseOperator;
        public Operator BaseOperator { get => baseOperator; }

        protected override DescriberEnum BaseDescribers { get => DescriberEnum.ConstructorBaseDescriber; }

        public OperatorOverloadNode(Operator _operator, Describer _describer, ParseNodeCollection _body, DataType _type) : base(_operator.Value, _describer, _body, _type)
        {
            baseOperator = _operator;
        }
    }
}
