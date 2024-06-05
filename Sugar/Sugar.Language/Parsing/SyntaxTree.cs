using System;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Structure;

namespace Sugar.Language.Parsing
{
    internal sealed class SyntaxTree
    {
        private readonly ParseNode baseNode;
        public ParseNode BaseNode { get => baseNode; }

        private ReferenceCollection references;
        public ReferenceCollection References { get => references; }

        public SyntaxTree(ParseNode _baseNode)
        {
            baseNode = _baseNode;
        }

        public SyntaxTree WithReferences(ReferenceCollection refer)
        {
            if(references != null)
                references = refer;

            return this;
        }

        public void Print() => baseNode.Print("", true);

        public void ParentNodes() => baseNode.SetParent();
    }
}
