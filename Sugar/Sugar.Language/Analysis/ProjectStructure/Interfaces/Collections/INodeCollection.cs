using System;
namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.Collections
{
    internal interface INodeCollection<Node, Collection> : INode where Collection : INodeCollection<Node, Collection>
    {
        public Collection AddEntity(Node node);
    }
}
