using System;

using Sugar.Language.Parsing.Nodes.Values.Generics;

namespace Sugar.Language.Parsing.Nodes.Interfaces.Generic
{
    internal interface IGenericCall
    {
        public GenericCallNode Generic { get; }
    }
}
