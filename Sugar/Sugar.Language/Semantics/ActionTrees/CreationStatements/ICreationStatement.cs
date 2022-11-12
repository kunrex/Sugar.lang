using System;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Interfaces;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Describers;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements
{
    internal interface ICreationStatement : IActionTreeNode, IDescribable
    {
        public string Name { get; }

        public CreationTypeEnum CreationEnumType { get; }
    }
}
