using System;
using System.Collections.Generic;

namespace Sugar.Language.Parsing.Nodes
{
    internal abstract class ParseNodeCollection : NodeCollection<ParseNode>
    {
        public ParseNodeCollection() : base()
        {
           
        }

        public ParseNodeCollection(List<ParseNode> _children) : base(_children)
        {
            
        }

        public ParseNodeCollection(params ParseNode[] _children) : base(_children)
        {
            
        }

        public virtual ParseNode AddChild(ParseNode node)
        {
            Add(node);

            return this;
        }
    }
}
