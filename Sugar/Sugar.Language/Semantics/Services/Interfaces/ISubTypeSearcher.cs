using System;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Namespaces;

namespace Sugar.Language.Semantics.Services.Interfaces
{
    internal interface ISubTypeSearcher : ISemanticService
    {
        public DataType FindReferencedType(Node type, CreatedNameSpaceCollectionNode collectionNode);
    }
}
