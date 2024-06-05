using System;
using System.Collections.Generic;

using Sugar.Language.Exceptions.Analysis.Import;

using Sugar.Language.Parsing;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Expressions.Associative;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes;
using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Referencing;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.Namespaces;
using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Structure;

namespace Sugar.Language.Analysis.ProjectCreation
{
    internal sealed class ReferenceMapper : SemanticService
    {
        private readonly SyntaxTreeCollection wrapper;
        private readonly SyntaxTreeCollection project;

        public ReferenceMapper(SyntaxTreeCollection _wrapper, SyntaxTreeCollection _project, ProjectTree _projectTree) : base(_projectTree)
        {
            wrapper = _wrapper;
            project = _project;
        }

        public override void Process()
        {
            foreach (var tree in project)
                CreateReferences(tree.References);

            foreach (var type in projectTree.DefaultNamespace)
                ReferenceParents(type);

            foreach (var name in projectTree.ProjectNamespace)
                ReferenceParents(name);
        }

        private void CreateReferences(ReferenceCollection references)
        {
            var current = new Queue<IReferencable>();

            //filter imports for repeats
            foreach (var import in references.Imports)
            {
                current.Clear();
                var node = import.BaseName;
                
                current.Enqueue(projectTree.ProjectNamespace);

                while(node != null)
                {
                    switch (node.NodeType)
                    {
                        case ParseNodeType.Dot:
                            var dot = (DotExpression)node;
                            EnqeueReferences(((IdentifierNode)dot.LHS).Value);

                            node = ((DotExpression)node).RHS;
                            break;
                        case ParseNodeType.Variable:
                            EnqeueReferences(((IdentifierNode)node).Value);

                            node = null;
                            break;
                    }

                    if (current.Count == 0)
                        break;

                    void EnqeueReferences(string value)
                    {
                        int length = current.Count;
                        for (int i = 0; i < length; i++)
                        {
                            var result = current.Dequeue().GetChildReference(value);
                            if (result == null)
                                continue;

                            foreach (var reference in result)
                                current.Enqueue(reference);
                        }
                    }
                }

                if(current.Count == 0)
                {
                    projectTree.WithException(new NoReferenceFoundException());
                }
                else
                {
                    int final = current.Count;
                    var creationType = (ProjectMemberEnum)import.CreationType;
                    for (int i = 0; i < final; i++)
                    {
                        var referencable = current.Dequeue();

                        if (referencable.ProjectMemberType == creationType)
                            current.Enqueue(referencable);
                    }

                    if (current.Count == 1)
                    {
                        var top = current.Dequeue();

                        if(references.HasNotReference(top))
                            references.WithReference(current.Dequeue());
                    }
                    else
                        projectTree.WithException(new AmbigiousReferenceException());
                }
            }
        }

        private void ReferenceParents(CreatedNamespaceNode name)
        {
            foreach (var type in name)
                ReferenceParents(type);

            foreach(var nameSpace in name.Namespaces)
                ReferenceParents(nameSpace);
        }

        private void ReferenceParents(DataType type)
        {
            IReferencable parent = type.GetParent();
            while (parent != null)
            {
                type.ReferenceParent(parent);

                switch (parent.ProjectMemberType)
                {
                    case ProjectMemberEnum.DataTypes:
                    case ProjectMemberEnum.CreatedNameSpace:
                        parent = parent.GetParent();
                        break;
                    default:
                        parent = null;
                        break;
                }
            }

            type.ReferenceParent();
        }
    }
}
