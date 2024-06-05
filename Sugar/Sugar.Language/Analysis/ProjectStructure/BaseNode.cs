using System;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.Interfaces;

namespace Sugar.Language.Analysis.ProjectStructure
{
    internal abstract class BaseNode : INode
    {
        public abstract MemberTypeEnum MemberType { get; }

        public BaseNode()
        {

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

        protected virtual void PrintChildren(string indent) { }
    }
}
