using System;

using Sugar.Language.Parsing.Nodes.Describers;

namespace Sugar.Language.Parsing.Nodes.Interfaces.Creation
{
    internal interface ICreationNode
    {
        public DescriberNode Describer { get; }
    }
}
