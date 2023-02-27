using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.InvalidNodes;

namespace Sugar.Language.Parsing.Parser.Structure
{
    internal struct ParseInvalidResult
    {
        private readonly InvalidTokenCollectionNode tokenCollection;
        public InvalidTokenCollectionNode TokenCollection { get => tokenCollection; }

        private readonly Node valid;
        public Node Valid { get => valid; }
        public bool IsValid { get => valid != null; }

        public ParseInvalidResult(InvalidTokenCollectionNode _collection)
        {
            tokenCollection = _collection;
            valid = null;
        }

        public ParseInvalidResult(InvalidTokenCollectionNode _collection, Node _valid)
        {
            tokenCollection = _collection;
            valid = _valid;
        }
    }
}
