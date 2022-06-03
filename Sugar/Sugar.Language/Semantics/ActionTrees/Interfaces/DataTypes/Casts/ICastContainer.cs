using System;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Functions.Declarations;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global.Conversion;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes.Casts
{
    internal interface ICastContainer<T, SubType, Parent> : IContainer<T, ICastContainer<T, SubType, Parent>> where T : CastDeclarationStmt<SubType, Parent> where SubType : UnnamedFunctionDeclarationNode where Parent : IActionTreeNode
    {
        
    }
}
