using System;

using Sugar.Language.Exceptions.Analysis.Processing;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Scope;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Properties;

namespace Sugar.Language.Analysis.ProjectStructure.CreationNodes.Properties
{
    internal abstract class BasePropertyNode : CreationNode, IProperty, IParentableNode<IPropertyParent>, IScopeParent
    {
        public override MemberTypeEnum MemberType { get => MemberTypeEnum.Global; }
        protected override DescriberEnum BaseDescribers { get => DescriberEnum.AccessModifiers; }

        private readonly DataType creationType;
        public DataType CreationType { get => creationType; }

        private IPropertyParent parent;
        public IPropertyParent Parent { get => parent; }

        public abstract GlobalMemberEnum GlobalMember { get; }

        public BasePropertyNode(string _name, Describer _describer, DataType _type) : base(_name, _describer)
        {
            creationType = _type;
        }

        public void SetParent(IPropertyParent propertyParent)
        {
            if (parent != null)
                throw new DoubleParentAssignementException();

            parent = propertyParent;
        }
    }
}
