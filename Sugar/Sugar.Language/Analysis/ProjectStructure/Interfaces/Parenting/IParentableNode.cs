using System;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting
{
    internal interface IParentableNode<ParentType> : INode where ParentType : INode
    {
        public ParentType Parent { get; }

        public void SetParent(ParentType parent);
    }
}
