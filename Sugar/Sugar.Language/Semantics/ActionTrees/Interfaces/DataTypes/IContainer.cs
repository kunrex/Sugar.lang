using System;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes
{
    internal interface IContainer<Element, IContainerType> : IActionTreeNode where Element : IActionTreeNode where IContainerType : IContainer<Element, IContainerType>
    {
        public IContainerType AddDeclaration(Element declaration);
    }
}
