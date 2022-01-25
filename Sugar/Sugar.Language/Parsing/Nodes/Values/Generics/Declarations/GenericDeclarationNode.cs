using System;
using System.Collections.Generic;

namespace Sugar.Language.Parsing.Nodes.Values.Generics.Declarations
{
    internal sealed class GenericDeclarationNode : BaseGenericNode
    {
        public GenericDeclarationNode(List<Node> _declarations)
        {
            Children = _declarations;
        }

        public override string ToString() => $"Generic Declaration Node";
    }
}
