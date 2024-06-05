using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes
{
    internal abstract class ParseNode
    {
        public ParseNode Parent { get; protected set; }
        public abstract ParseNodeType NodeType { get; }

        public abstract void SetParent();

        public void SetParent(ParseNode _parent)
        {
            Parent = _parent;

            SetParent();
        }

        public void Print(string indent, bool last)
        {
            Console.Write(indent);
            if (last)
            {
                Console.Write("\\-");
                indent += "  ";
            }
            else
            {
                Console.Write("|-");
                indent += "| ";
            }

            Console.WriteLine(ToString());
            PrintChildren(indent);
        }

        protected abstract void PrintChildren(string indent);
    }
}
