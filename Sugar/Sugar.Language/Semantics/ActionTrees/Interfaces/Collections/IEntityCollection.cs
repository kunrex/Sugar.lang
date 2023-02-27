using System;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.Collections
{
    internal interface IEntityCollection<DataType, Collection> : IActionTreeNode where Collection : IEntityCollection<DataType, Collection> 
    {
        public Collection AddEntity(DataType dataType);
    }
}
