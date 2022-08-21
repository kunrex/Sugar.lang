using System;

using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global.Conversion;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes.Casts
{
    internal interface ICastContainer<T, Parent> : IContainer<T, ICastContainer<T, Parent>> where T : CastDeclarationStmt<Parent> where Parent : IActionTreeNode
    {
        
    }
}
