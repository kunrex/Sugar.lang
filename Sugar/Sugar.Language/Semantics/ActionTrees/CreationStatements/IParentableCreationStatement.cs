using System;

using Sugar.Language.Semantics.ActionTrees.Interfaces;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements
{
    internal interface IParentableCreationStatement<Parent> : ICreationStatement, IParentableActionTreeNode<Parent> where Parent : IActionTreeNode
    {

    }
}
