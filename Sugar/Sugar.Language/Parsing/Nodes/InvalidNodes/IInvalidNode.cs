using System;

using Sugar.Language.Exceptions;
using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes
{
    internal interface IInvalidNode 
    {
        public CompileException Exception { get; }
    }
}
