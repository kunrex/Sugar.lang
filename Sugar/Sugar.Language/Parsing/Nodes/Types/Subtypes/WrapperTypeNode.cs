using System;

namespace Sugar.Language.Parsing.Nodes.Types.Subtypes
{
    internal abstract class WrapperTypeNode : TypeNode
    {
        protected readonly TypeNode type;
        public TypeNode WrappedType { get => type; }

        public WrapperTypeNode(TypeNode _type) : base(_type)
        {
            type = _type;
        }
    }
}
