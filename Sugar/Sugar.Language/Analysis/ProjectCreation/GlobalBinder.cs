using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Enum = Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Enum;
using Sugar.Language.Parsing.Nodes.Statements.VariableCreation;

namespace Sugar.Language.Analysis.ProjectCreation
{
    internal sealed class GlobalBinder : SemanticService
    {
        public GlobalBinder(ProjectTree _projectTree) : base(_projectTree)
        {

        }

        public override void Process()
        {
            
        }

        private void BindGlobalMembers(Enum dataType)
        {
            foreach (var node in dataType.ParseSkeleton)
            {
                switch (node.NodeType)
                {
                    case ParseNodeType.Enum:
                    case ParseNodeType.Class:
                    case ParseNodeType.Empty:
                    case ParseNodeType.Struct:
                    case ParseNodeType.Interface:
                        break;
                    case ParseNodeType.Declaration:
                        break;
                    case ParseNodeType.Initialise:
                        break;
                }
            }
        }

        //namespace duplication
        //global binding node filtering
        //global binding
        private void BindGlobalMembers(Struct dataType)
        {
            foreach (var node in dataType.ParseSkeleton)
            {
                switch (node.NodeType)
                {
                    case ParseNodeType.Enum:
                    case ParseNodeType.Class:
                    case ParseNodeType.Empty:
                    case ParseNodeType.Struct:
                    case ParseNodeType.Interface:
                        break;
                    case ParseNodeType.Declaration:
                        break;
                    case ParseNodeType.Initialise:
                        break;
                    case ParseNodeType.PropertyDeclaration:
                        break;
                    case ParseNodeType.PropertyInitialise:
                        break;
                    case ParseNodeType.FunctionDeclaration:
                        break;
                    case ParseNodeType.ConstructorDeclaration:
                        break;
                    case ParseNodeType.Indexer:
                        break;
                    case ParseNodeType.OperatorOverload:
                        break;
                    default:
                        break;
                }
            }
        }

        private void BindGlobalMembers(Class dataType)
        {
            foreach(var node in dataType.ParseSkeleton)
            {
                switch(node.NodeType)
                {
                    case ParseNodeType.Enum:
                    case ParseNodeType.Class:
                    case ParseNodeType.Empty:
                    case ParseNodeType.Struct:
                    case ParseNodeType.Interface:
                        break;
                    case ParseNodeType.Declaration:
                        break;
                    case ParseNodeType.Initialise:
                        break;
                    case ParseNodeType.PropertyDeclaration:
                        break;
                    case ParseNodeType.PropertyInitialise:
                        break;
                    case ParseNodeType.FunctionDeclaration:
                        break;
                    case ParseNodeType.ConstructorDeclaration:
                        break;
                    case ParseNodeType.Indexer:
                        break;
                    case ParseNodeType.ExplicitDeclaration:
                        break;
                    case ParseNodeType.ImplicitDeclaration:
                        break;
                    case ParseNodeType.OperatorOverload:
                        break;
                    default:
                        break;
                }
            }
        }

        private void BindGlobalMembers(Interface dataType)
        {
            foreach (var node in dataType.ParseSkeleton)
            {
                switch (node.NodeType)
                {
                    case ParseNodeType.Enum:
                    case ParseNodeType.Class:
                    case ParseNodeType.Empty:
                    case ParseNodeType.Struct:
                    case ParseNodeType.Interface:
                        break;
                    case ParseNodeType.PropertyDeclaration:
                        break;
                    case ParseNodeType.PropertyInitialise:
                        break;
                    case ParseNodeType.FunctionDeclaration:
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
