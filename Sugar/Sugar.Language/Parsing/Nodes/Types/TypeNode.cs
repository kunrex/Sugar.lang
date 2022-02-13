using System;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Types.Enums;

using Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums;

namespace Sugar.Language.Parsing.Nodes.Types
{
    internal abstract class TypeNode : Node
    {
        public abstract TypeNodeEnum Type { get; }
        public override NodeType NodeType => NodeType.Type;

        public virtual TypeEnum ReturnType() => throw new InvalidOperationException();
    }
}
