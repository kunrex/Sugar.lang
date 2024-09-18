using System;
using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions;
using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Types.Enums;
using Sugar.Language.Parsing.Nodes.Types.Subtypes;
using Sugar.Language.Parsing.Nodes.Statements.VariableCreation;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes;

using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Variables;
using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;
using Sugar.Language.Parsing.Nodes.Functions.Declarations;
using Sugar.Language.Parsing.Nodes.Types;
using Enum = Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Enum;

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
                        var declarationNode = (DeclarationNode)node;
                        var globalVariable = new GlobalVariableNode(declarationNode.Name.Value, CreateDescriber(declarationNode.Describer), FindType(dataType, declarationNode.Type));
                        break;
                    case ParseNodeType.Initialise:
                        break;
                    case ParseNodeType.PropertyDeclaration:
                        break;
                    case ParseNodeType.PropertyInitialise:
                        break;
                    case ParseNodeType.FunctionDeclaration:
                        var functionDeclaration = (FunctionDeclarationNode)node;

                        if (functionDeclaration.Type.Type == TypeNodeEnum.Void)
                        {
                            //globalvoidnode.body  should be a parsenode cause it can be empty 
                            var globalFunction = new GlobalVoidNode(functionDeclaration.Name.Value,
                                CreateDescriber(functionDeclaration.Describer), functionDeclaration.Body);
                        }
                        else
                        {
                            var globalFunction = new GlobalMethodNode(functionDeclaration.Name.Value,
                                CreateDescriber(functionDeclaration.Describer),
                                FindType(dataType, functionDeclaration.Type), functionDeclaration.Body);
                        }

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
