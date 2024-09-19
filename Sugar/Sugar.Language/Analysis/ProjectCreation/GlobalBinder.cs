using System;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Types.Enums;
using Sugar.Language.Parsing.Nodes.Functions.Properties;
using Sugar.Language.Parsing.Nodes.Functions.Declarations;
using Sugar.Language.Parsing.Nodes.Statements.VariableCreation;
using Sugar.Language.Parsing.Nodes.Statements.VariableCreation.Properties;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.OperatorOverloading;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes;
using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Variables;
using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions;
using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Properties;
using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions.Casting;

using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Properties.Structure;

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
                        var declarationNode = (DeclarationNode)node;
                        dataType.AddVariable(new GlobalVariableNode(declarationNode.Name.Value, CreateDescriber(declarationNode.Describer), FindType(dataType, declarationNode.Type)));
                        break;
                    case ParseNodeType.Initialise:
                        var initialiseNode = (InitializeNode)node;
                        dataType.AddVariable(new GlobalInitialiseNode(initialiseNode.Name.Value, CreateDescriber(initialiseNode.Describer), FindType(dataType, initialiseNode.Type), initialiseNode.Value));
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
                        var declarationNode = (DeclarationNode)node;
                        dataType.AddVariable(new GlobalVariableNode(declarationNode.Name.Value, CreateDescriber(declarationNode.Describer), FindType(dataType, declarationNode.Type)));
                        break;
                    case ParseNodeType.Initialise:
                        var initialiseNode = (InitializeNode)node;
                        dataType.AddVariable(new GlobalInitialiseNode(initialiseNode.Name.Value, CreateDescriber(initialiseNode.Describer), FindType(dataType, initialiseNode.Type), initialiseNode.Value));
                        break;
                    case ParseNodeType.PropertyDeclaration:
                        var propertyDecl = (PropertyDeclarationNode)node;
                        switch (propertyDecl.Property.NodeType)
                        {
                            case ParseNodeType.PropertyGet:
                                var getNode = (PropertyGetNode)propertyDecl.Property;
                                dataType.AddProperty(new PropertyGet(propertyDecl.Name.Value, CreateDescriber(propertyDecl.Describer), FindType(dataType, propertyDecl.Type), new Get(CreateDescriber(getNode.Get.Describer), getNode.Get.Body)));
                                break;
                            case ParseNodeType.PropertySet:
                                var setNode = (PropertySetNode)propertyDecl.Property;
                                dataType.AddProperty(new PropertySet(propertyDecl.Name.Value, CreateDescriber(propertyDecl.Describer), FindType(dataType, propertyDecl.Type), new Set(CreateDescriber(setNode.Set.Describer), setNode.Set.Body, FindType(dataType, setNode.Type))));
                                break;
                            case ParseNodeType.PropertyGetSet:
                                var getSetNode = (PropertyGetSetNode)propertyDecl.Property;
                                dataType.AddProperty(new PropertyGetSet(propertyDecl.Name.Value, CreateDescriber(propertyDecl.Describer), FindType(dataType, propertyDecl.Type), new Get(CreateDescriber(getSetNode.Get.Describer), getSetNode.Get.Body), new Set(CreateDescriber(getSetNode.Set.Describer), getSetNode.Set.Body, FindType(dataType, getSetNode.Type))));
                                break;
                        }
                        break;
                    case ParseNodeType.PropertyInitialise:
                        var propertyInit = (PropertyInitialisationNode)node;
                        /*switch (propertyInit.Property.NodeType)
                        {
                            case ParseNodeType.PropertyGet:
                                var getNode = (PropertyGetNode)propertyInit.Property;
                                dataType.AddProperty(new PropertyGet(propertyInit.Name.Value, CreateDescriber(propertyInit.Describer), FindType(dataType, propertyInit.Type), new Get(CreateDescriber(getNode.Get.Describer), getNode.Get.Body)));
                                break;
                            case ParseNodeType.PropertySet:
                                var setNode = (PropertySetNode)propertyInit.Property;
                                dataType.AddProperty(new PropertySet(propertyInit.Name.Value, CreateDescriber(propertyInit.Describer), FindType(dataType, propertyInit.Type), new Set(CreateDescriber(setNode.Set.Describer), setNode.Set.Body, FindType(dataType, setNode.Type))));
                                break;
                            case ParseNodeType.PropertyGetSet:
                                var getSetNode = (PropertyGetSetNode)propertyInit.Property;
                                dataType.AddProperty(new PropertyGetSet(propertyInit.Name.Value, CreateDescriber(propertyInit.Describer), FindType(dataType, propertyInit.Type), new Get(CreateDescriber(getSetNode.Get.Describer), getSetNode.Get.Body), new Set(CreateDescriber(getSetNode.Set.Describer), getSetNode.Set.Body, FindType(dataType, getSetNode.Type))));
                                break;
                        }*/
                        break;
                     case ParseNodeType.FunctionDeclaration:
                        var functionDeclaration = (FunctionDeclarationNode)node;

                        if (functionDeclaration.Type.Type == TypeNodeEnum.Void)
                            dataType.AddGlobalVoid(new GlobalVoidNode(functionDeclaration.Name.Value, CreateDescriber(functionDeclaration.Describer), functionDeclaration.Body));
                        else
                            dataType.AddGlobalMethod(new GlobalMethodNode(functionDeclaration.Name.Value, CreateDescriber(functionDeclaration.Describer), FindType(dataType, functionDeclaration.Type), functionDeclaration.Body));
                        break;
                    case ParseNodeType.ConstructorDeclaration:
                        var constructor = (ConstructorDeclarationNode)node;
                        dataType.AddConstructor(new ConstructorNode(dataType, CreateDescriber(constructor.Describer), constructor.Body));
                        break;
                    case ParseNodeType.Indexer:
                        var indexer = (IndexerDeclarationNode)node;
                        switch (indexer.Property.NodeType)
                        {
                            case ParseNodeType.PropertyGet:
                                var getNode = (PropertyGetNode)indexer.Property;
                                dataType.AddProperty(new IndexerGet(CreateDescriber(indexer.Describer), FindType(dataType, indexer.Type), new Get(CreateDescriber(getNode.Get.Describer), getNode.Get.Body)));
                                break;
                            case ParseNodeType.PropertySet:
                                var setNode = (PropertySetNode)indexer.Property;
                                dataType.AddProperty(new IndexerSet(CreateDescriber(indexer.Describer), FindType(dataType, indexer.Type), new Set(CreateDescriber(setNode.Set.Describer), setNode.Set.Body, FindType(dataType, setNode.Type))));
                                break;
                            case ParseNodeType.PropertyGetSet:
                                var getSetNode = (PropertyGetSetNode)indexer.Property;
                                dataType.AddProperty(new IndexerGetSet(CreateDescriber(indexer.Describer), FindType(dataType, indexer.Type), new Get(CreateDescriber(getSetNode.Get.Describer), getSetNode.Get.Body), new Set(CreateDescriber(getSetNode.Set.Describer), getSetNode.Set.Body, FindType(dataType, getSetNode.Type))));
                                break;
                        }
                        break;
                    case ParseNodeType.OperatorOverload:
                        var overload = (OperatorOverloadFunctionDeclarationNode)node;
                        dataType.AddOverload(new OperatorOverloadNode(overload.Operator, CreateDescriber(overload.Describer), overload.Body, FindType(dataType, overload.Type)));
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
                        dataType.AddVariable(new GlobalVariableNode(declarationNode.Name.Value, CreateDescriber(declarationNode.Describer), FindType(dataType, declarationNode.Type)));
                        break;
                    case ParseNodeType.Initialise:
                        var initialiseNode = (InitializeNode)node;
                        dataType.AddVariable(new GlobalInitialiseNode(initialiseNode.Name.Value, CreateDescriber(initialiseNode.Describer), FindType(dataType, initialiseNode.Type), initialiseNode.Value));
                        break;
                    case ParseNodeType.PropertyDeclaration:
                        var propertyDecl = (PropertyDeclarationNode)node;
                        switch (propertyDecl.Property.NodeType)
                        {
                            case ParseNodeType.PropertyGet:
                                var getNode = (PropertyGetNode)propertyDecl.Property;
                                dataType.AddProperty(new PropertyGet(propertyDecl.Name.Value, CreateDescriber(propertyDecl.Describer), FindType(dataType, propertyDecl.Type), new Get(CreateDescriber(getNode.Get.Describer), getNode.Get.Body)));
                                break;
                            case ParseNodeType.PropertySet:
                                var setNode = (PropertySetNode)propertyDecl.Property;
                                dataType.AddProperty(new PropertySet(propertyDecl.Name.Value, CreateDescriber(propertyDecl.Describer), FindType(dataType, propertyDecl.Type), new Set(CreateDescriber(setNode.Set.Describer), setNode.Set.Body, FindType(dataType, setNode.Type))));
                                break;
                            case ParseNodeType.PropertyGetSet:
                                var getSetNode = (PropertyGetSetNode)propertyDecl.Property;
                                dataType.AddProperty(new PropertyGetSet(propertyDecl.Name.Value, CreateDescriber(propertyDecl.Describer), FindType(dataType, propertyDecl.Type), new Get(CreateDescriber(getSetNode.Get.Describer), getSetNode.Get.Body), new Set(CreateDescriber(getSetNode.Set.Describer), getSetNode.Set.Body, FindType(dataType, getSetNode.Type))));
                                break;
                        }
                        break;
                    case ParseNodeType.PropertyInitialise:
                        var propertyInit = (PropertyInitialisationNode)node;
                        /*switch (propertyInit.Property.NodeType)
                        {
                            case ParseNodeType.PropertyGet:
                                var getNode = (PropertyGetNode)propertyInit.Property;
                                dataType.AddProperty(new PropertyGet(propertyInit.Name.Value, CreateDescriber(propertyInit.Describer), FindType(dataType, propertyInit.Type), new Get(CreateDescriber(getNode.Get.Describer), getNode.Get.Body)));
                                break;
                            case ParseNodeType.PropertySet:
                                var setNode = (PropertySetNode)propertyInit.Property;
                                dataType.AddProperty(new PropertySet(propertyInit.Name.Value, CreateDescriber(propertyInit.Describer), FindType(dataType, propertyInit.Type), new Set(CreateDescriber(setNode.Set.Describer), setNode.Set.Body, FindType(dataType, setNode.Type))));
                                break;
                            case ParseNodeType.PropertyGetSet:
                                var getSetNode = (PropertyGetSetNode)propertyInit.Property;
                                dataType.AddProperty(new PropertyGetSet(propertyInit.Name.Value, CreateDescriber(propertyInit.Describer), FindType(dataType, propertyInit.Type), new Get(CreateDescriber(getSetNode.Get.Describer), getSetNode.Get.Body), new Set(CreateDescriber(getSetNode.Set.Describer), getSetNode.Set.Body, FindType(dataType, getSetNode.Type))));
                                break;
                        }*/
                        break;
                    case ParseNodeType.FunctionDeclaration:
                        var functionDeclaration = (FunctionDeclarationNode)node;

                        if (functionDeclaration.Type.Type == TypeNodeEnum.Void)
                            dataType.AddGlobalVoid(new GlobalVoidNode(functionDeclaration.Name.Value, CreateDescriber(functionDeclaration.Describer), functionDeclaration.Body));
                        else
                            dataType.AddGlobalMethod(new GlobalMethodNode(functionDeclaration.Name.Value, CreateDescriber(functionDeclaration.Describer), FindType(dataType, functionDeclaration.Type), functionDeclaration.Body));
                        break;
                    case ParseNodeType.ConstructorDeclaration:
                        var constructor = (ConstructorDeclarationNode)node;
                        dataType.AddConstructor(new ConstructorNode(dataType, CreateDescriber(constructor.Describer), constructor.Body));
                        break;
                    case ParseNodeType.Indexer:
                        var indexer = (IndexerDeclarationNode)node;
                        switch (indexer.Property.NodeType)
                        {
                            case ParseNodeType.PropertyGet:
                                var getNode = (PropertyGetNode)indexer.Property;
                                dataType.AddProperty(new IndexerGet(CreateDescriber(indexer.Describer), FindType(dataType, indexer.Type), new Get(CreateDescriber(getNode.Get.Describer), getNode.Get.Body)));
                                break;
                            case ParseNodeType.PropertySet:
                                var setNode = (PropertySetNode)indexer.Property;
                                dataType.AddProperty(new IndexerSet(CreateDescriber(indexer.Describer), FindType(dataType, indexer.Type), new Set(CreateDescriber(setNode.Set.Describer), setNode.Set.Body, FindType(dataType, setNode.Type))));
                                break;
                            case ParseNodeType.PropertyGetSet:
                                var getSetNode = (PropertyGetSetNode)indexer.Property;
                                dataType.AddProperty(new IndexerGetSet(CreateDescriber(indexer.Describer), FindType(dataType, indexer.Type), new Get(CreateDescriber(getSetNode.Get.Describer), getSetNode.Get.Body), new Set(CreateDescriber(getSetNode.Set.Describer), getSetNode.Set.Body, FindType(dataType, getSetNode.Type))));
                                break;
                        }
                        break;
                    case ParseNodeType.ExplicitDeclaration:
                        var explicitCast = (ExplicitCastDeclarationNode)node;
                        dataType.AddExplicitCast(new ExplicitCastNode(FindType(dataType, explicitCast.Type), CreateDescriber(explicitCast.Describer), explicitCast.Body));
                        break;
                    case ParseNodeType.ImplicitDeclaration:
                        var implicitCast = (ImplicitCastDeclarationNode)node;
                        dataType.AddImplicitCast(new ImplicitCastNode(FindType(dataType, implicitCast.Type), CreateDescriber(implicitCast.Describer), implicitCast.Body));
                        break;
                    case ParseNodeType.OperatorOverload:
                        var overload = (OperatorOverloadFunctionDeclarationNode)node;
                        dataType.AddOverload(new OperatorOverloadNode(overload.Operator, CreateDescriber(overload.Describer), overload.Body, FindType(dataType, overload.Type)));
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
                        var propertyDecl = (PropertyDeclarationNode)node;
                        switch (propertyDecl.Property.NodeType)
                        {
                            case ParseNodeType.PropertyGet:
                                var getNode = (PropertyGetNode)propertyDecl.Property;
                                dataType.AddProperty(new PropertyGet(propertyDecl.Name.Value, CreateDescriber(propertyDecl.Describer), FindType(dataType, propertyDecl.Type), new Get(CreateDescriber(getNode.Get.Describer), getNode.Get.Body)));
                                break;
                            case ParseNodeType.PropertySet:
                                var setNode = (PropertySetNode)propertyDecl.Property;
                                dataType.AddProperty(new PropertySet(propertyDecl.Name.Value, CreateDescriber(propertyDecl.Describer), FindType(dataType, propertyDecl.Type), new Set(CreateDescriber(setNode.Set.Describer), setNode.Set.Body, FindType(dataType, setNode.Type))));
                                break;
                            case ParseNodeType.PropertyGetSet:
                                var getSetNode = (PropertyGetSetNode)propertyDecl.Property;
                                dataType.AddProperty(new PropertyGetSet(propertyDecl.Name.Value, CreateDescriber(propertyDecl.Describer), FindType(dataType, propertyDecl.Type), new Get(CreateDescriber(getSetNode.Get.Describer), getSetNode.Get.Body), new Set(CreateDescriber(getSetNode.Set.Describer), getSetNode.Set.Body, FindType(dataType, getSetNode.Type))));
                                break;
                        }
                        break;
                    case ParseNodeType.PropertyInitialise:
                        var propertyInit = (PropertyInitialisationNode)node;
                        /*switch (propertyInit.Property.NodeType)
                        {
                            case ParseNodeType.PropertyGet:
                                var getNode = (PropertyGetNode)propertyInit.Property;
                                dataType.AddProperty(new PropertyGet(propertyInit.Name.Value, CreateDescriber(propertyInit.Describer), FindType(dataType, propertyInit.Type), new Get(CreateDescriber(getNode.Get.Describer), getNode.Get.Body)));
                                break;
                            case ParseNodeType.PropertySet:
                                var setNode = (PropertySetNode)propertyInit.Property;
                                dataType.AddProperty(new PropertySet(propertyInit.Name.Value, CreateDescriber(propertyInit.Describer), FindType(dataType, propertyInit.Type), new Set(CreateDescriber(setNode.Set.Describer), setNode.Set.Body, FindType(dataType, setNode.Type))));
                                break;
                            case ParseNodeType.PropertyGetSet:
                                var getSetNode = (PropertyGetSetNode)propertyInit.Property;
                                dataType.AddProperty(new PropertyGetSet(propertyInit.Name.Value, CreateDescriber(propertyInit.Describer), FindType(dataType, propertyInit.Type), new Get(CreateDescriber(getSetNode.Get.Describer), getSetNode.Get.Body), new Set(CreateDescriber(getSetNode.Set.Describer), getSetNode.Set.Body, FindType(dataType, getSetNode.Type))));
                                break;
                        }*/
                        break;
                    case ParseNodeType.FunctionDeclaration:
                        var functionDeclaration = (FunctionDeclarationNode)node;

                        if (functionDeclaration.Type.Type == TypeNodeEnum.Void)
                            dataType.AddGlobalVoid(new GlobalVoidNode(functionDeclaration.Name.Value, CreateDescriber(functionDeclaration.Describer), functionDeclaration.Body));
                        else
                            dataType.AddGlobalMethod(new GlobalMethodNode(functionDeclaration.Name.Value, CreateDescriber(functionDeclaration.Describer), FindType(dataType, functionDeclaration.Type), functionDeclaration.Body));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
