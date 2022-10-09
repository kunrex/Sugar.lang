using System;

using Sugar.Language.Semantics.ActionTrees.DataTypes;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global.Extensions
{
    internal interface IExtension
    {
        public DataType ParentType { get; }
    }
}
