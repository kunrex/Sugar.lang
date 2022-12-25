using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Semantics.Services.Structure;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Collections;

namespace Sugar.Language.Semantics.Services.Interfaces
{
    internal interface ITypeSearcherService : ISemanticService
    {
        public Queue<SearchResult<IDataTypeCollection>> ReferenceDataTypeCollection(Node type);
    }
}
