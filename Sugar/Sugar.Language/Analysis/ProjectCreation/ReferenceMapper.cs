using System;
using System.Collections.Generic;

using Sugar.Language.Exceptions.Analysis.Import;

using Sugar.Language.Parsing;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Values.Generics;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes;
using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Referencing;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.Namespaces;
using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Generics;
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
                SubReferences(type);

            foreach (var name in projectTree.ProjectNamespace)
                SubReferences(name);
        }

        private void CreateReferences(ReferenceCollection references)
        {
            var current = new Queue<IReferencable>();

            foreach (var import in references.Imports)
            {
                current.Clear();
                var node = import.BaseName;
                
                current.Enqueue(projectTree.ProjectNamespace);

                if (node.NodeType == ParseNodeType.Identifier)
                    EnqeueReferences(((IdentifierNode)node).Value);
                else
                {
                    var longIdentifer = (LongIdentiferNode)node;

                    for (int i = 0; i < longIdentifer.SplitLength; i++)
                    {
                        EnqeueReferences(longIdentifer.NameAt(i));

                        if (current.Count == 0)
                            break;
                    }
                }

                if (current.Count == 0)
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
        }

        private void SubReferences(CreatedNamespaceNode name)
        {
            foreach (var type in name)
                SubReferences(type);

            foreach(var nameSpace in name.Namespaces)
                SubReferences(nameSpace);
        }

        private void SubReferences(DataType type)
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

            GenericDeclarationNode declaration = null;
            switch (type.ProjectMemberType)
            {
                case ProjectMemberEnum.Class:
                    declaration = ((Class)type).ParseSkeleton.Generic;
                    break;
                case ProjectMemberEnum.Struct:
                    declaration = ((Struct)type).ParseSkeleton.Generic;
                    break;
                case ProjectMemberEnum.Interface:
                    declaration = ((Interface)type).ParseSkeleton.Generic;
                    break;
            }

            if(declaration != null)
                foreach(var gen in declaration)
                    type.ReferenceGeneric(new GenericReference(gen.Variable.Value, gen.Enforcement));
        }
    }
}
