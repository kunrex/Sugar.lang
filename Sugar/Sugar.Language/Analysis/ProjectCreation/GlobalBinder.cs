using System;
using Sugar.Language.Analysis.ProjectStructure;
using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Types.Enums;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes;
using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Parsing.Nodes.Statements.VariableCreation;
using Sugar.Language.Parsing.Nodes.Statements.VariableCreation.Properties;

using Sugar.Language.Parsing.Nodes.Functions.Properties;
using Sugar.Language.Parsing.Nodes.Functions.Declarations;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.OperatorOverloading;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Functions;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Variables;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Properties;

using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Variables;
using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions;
using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Properties;
using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions.Casting;
using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Properties.Initialise;
using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Properties.Structure;
using Sugar.Language.Analysis.ProjectStructure.Enums;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Tokens;
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
            var integer = projectTree.DefaultNamespace.GetInternalDataType(WrapperTypeEnum.Integer);
            var describer = new Describer(DescriberEnum.EnumModifiers);
            foreach (var node in dataType.ParseSkeleton)
            {
                switch (node.NodeType)
                {
                    case ParseNodeType.Identifier:
                        var identifier = (IdentifierNode)node;
                        dataType.AddVariable(new GlobalVariableNode(identifier.Value, describer, integer));
                        break;
                    case ParseNodeType.Assignment:
                        var assignment = (AssignmentNode)node;
                        dataType.AddVariable(new GlobalInitialiseNode(((IdentifierNode)assignment.Value).Value, describer, integer, assignment.To));
                        break;
                    default:
                        //error
                        break;
                }
            }
        }

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
                        PushVariableDeclaration(dataType, (DeclarationNode)node); 
                        break;
                    case ParseNodeType.Initialise:
                        PushVariableInitialisation(dataType, (InitializeNode)node);
                        break;
                    case ParseNodeType.PropertyDeclaration:
                        PushPropertyDeclarationNode(dataType, (PropertyDeclarationNode)node);
                        break;
                    case ParseNodeType.PropertyInitialise:
                        PushPropertyInitialisationNode(dataType, (PropertyInitialisationNode)node);
                        break;
                     case ParseNodeType.FunctionDeclaration: 
                         PushFunctionDeclarationNode(dataType, (FunctionDeclarationNode)node);
                         break;
                    case ParseNodeType.ConstructorDeclaration:
                        PushConstructorDeclarationNode(dataType, (ConstructorDeclarationNode)node);
                        break;
                    case ParseNodeType.Indexer:
                        PushIndexerNode(dataType, (IndexerDeclarationNode)node);
                        break;
                    case ParseNodeType.OperatorOverload:
                        PushOperatorOverloadNode(dataType, (OperatorOverloadFunctionDeclarationNode)node);
                        break;
                    default:
                        //error
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
                        PushVariableDeclaration(dataType, (DeclarationNode)node); 
                        break;
                    case ParseNodeType.Initialise:
                        PushVariableInitialisation(dataType, (InitializeNode)node);
                        break;
                    case ParseNodeType.PropertyDeclaration:
                        PushPropertyDeclarationNode(dataType, (PropertyDeclarationNode)node);
                        break;
                    case ParseNodeType.PropertyInitialise:
                        PushPropertyInitialisationNode(dataType, (PropertyInitialisationNode)node);
                        break;
                    case ParseNodeType.FunctionDeclaration: 
                        PushFunctionDeclarationNode(dataType, (FunctionDeclarationNode)node);
                        break;
                    case ParseNodeType.ConstructorDeclaration:
                        PushConstructorDeclarationNode(dataType, (ConstructorDeclarationNode)node);
                        break;
                    case ParseNodeType.Indexer:
                        PushIndexerNode(dataType, (IndexerDeclarationNode)node);
                        break;
                    case ParseNodeType.ExplicitDeclaration:
                        PushExplicitCastNode(dataType, (ExplicitCastDeclarationNode)node);
                        break;
                    case ParseNodeType.ImplicitDeclaration:
                        PushImplicitCastNode(dataType, (ImplicitCastDeclarationNode)node);
                        break;
                    case ParseNodeType.OperatorOverload:
                        PushOperatorOverloadNode(dataType, (OperatorOverloadFunctionDeclarationNode)node);
                        break;
                    default:
                        //error
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
                        PushPropertyDeclarationNode(dataType, (PropertyDeclarationNode)node);
                        break;
                    case ParseNodeType.PropertyInitialise:
                        PushPropertyInitialisationNode(dataType, (PropertyInitialisationNode)node);
                        break;
                    case ParseNodeType.FunctionDeclaration: 
                        PushFunctionDeclarationNode(dataType, (FunctionDeclarationNode)node);
                        break;
                    default:
                        //error
                        break;
                }
            }
        }

        private void PushVariableDeclaration<T>(T dataType, DeclarationNode declaration) where T : DataType, IGlobalVariableParent
        {
            dataType.AddVariable(new GlobalVariableNode(declaration.Name.Value, CreateDescriber(declaration.Describer), FindType(dataType, declaration.Type)));
        }
        
        private void PushVariableInitialisation<T>(T dataType, InitializeNode initialise) where T : DataType, IGlobalVariableParent
        {
            dataType.AddVariable(new GlobalInitialiseNode(initialise.Name.Value, CreateDescriber(initialise.Describer), FindType(dataType, initialise.Type), initialise.Value));
        }
        
        private void PushPropertyDeclarationNode<T>(T dataType, PropertyDeclarationNode propertyDecl) where T : DataType, IPropertyParent
        {
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
        }
        
        private void PushPropertyInitialisationNode<T>(T dataType, PropertyInitialisationNode propertyInit) where T : DataType, IPropertyParent
        {
            switch (propertyInit.Property.NodeType)
            {
                case ParseNodeType.PropertyGet:
                    var getNode = (PropertyGetNode)propertyInit.Property;
                    dataType.AddProperty(new GetInitialise(propertyInit.Name.Value, CreateDescriber(propertyInit.Describer), FindType(dataType, propertyInit.Type), new Get(CreateDescriber(getNode.Get.Describer), getNode.Get.Body), propertyInit.Value));
                    break;
                case ParseNodeType.PropertySet:
                    var setNode = (PropertySetNode)propertyInit.Property;
                    dataType.AddProperty(new SetInitialise(propertyInit.Name.Value, CreateDescriber(propertyInit.Describer), FindType(dataType, propertyInit.Type), new Set(CreateDescriber(setNode.Set.Describer), setNode.Set.Body, FindType(dataType, setNode.Type)), propertyInit.Value));
                    break;
                case ParseNodeType.PropertyGetSet:
                    var getSetNode = (PropertyGetSetNode)propertyInit.Property;
                    dataType.AddProperty(new GetSetInitialise(propertyInit.Name.Value, CreateDescriber(propertyInit.Describer), FindType(dataType, propertyInit.Type), new Get(CreateDescriber(getSetNode.Get.Describer), getSetNode.Get.Body), new Set(CreateDescriber(getSetNode.Set.Describer), getSetNode.Set.Body, FindType(dataType, getSetNode.Type)), propertyInit.Value));
                    break;
            }
        }

        private void PushFunctionDeclarationNode<T>(T dataType, FunctionDeclarationNode functionDeclaration) where T : DataType, IGlobalFunctionParent
        {
            if (functionDeclaration.Type.Type == TypeNodeEnum.Void)
                dataType.AddGlobalVoid(new GlobalVoidNode(functionDeclaration.Name.Value, CreateDescriber(functionDeclaration.Describer), functionDeclaration.Body, functionDeclaration.Arguments));
            else
                dataType.AddGlobalMethod(new GlobalMethodNode(functionDeclaration.Name.Value, CreateDescriber(functionDeclaration.Describer), FindType(dataType, functionDeclaration.Type), functionDeclaration.Body, functionDeclaration.Arguments));
        }
        
        private void PushConstructorDeclarationNode<T>(T dataType, ConstructorDeclarationNode constructor) where T : DataType, IConstructorParent
        {
            dataType.AddConstructor(new ConstructorNode(dataType, CreateDescriber(constructor.Describer), constructor.Body, constructor.Arguments, constructor.ParentOverrideNode));
        }

        private void PushIndexerNode<T>(T dataType, IndexerDeclarationNode indexer) where T : DataType, IIndexerParent
        {
            switch (indexer.Property.NodeType)
            {
                case ParseNodeType.PropertyGet:
                    var getNode = (PropertyGetNode)indexer.Property;
                    dataType.AddProperty(new IndexerGet(CreateDescriber(indexer.Describer), FindType(dataType, indexer.Type), new Get(CreateDescriber(getNode.Get.Describer), getNode.Get.Body), indexer.Arguments));
                    break;
                case ParseNodeType.PropertySet:
                    var setNode = (PropertySetNode)indexer.Property;
                    dataType.AddProperty(new IndexerSet(CreateDescriber(indexer.Describer), FindType(dataType, indexer.Type), new Set(CreateDescriber(setNode.Set.Describer), setNode.Set.Body, FindType(dataType, setNode.Type)), indexer.Arguments));
                    break;
                case ParseNodeType.PropertyGetSet:
                    var getSetNode = (PropertyGetSetNode)indexer.Property;
                    dataType.AddProperty(new IndexerGetSet(CreateDescriber(indexer.Describer), FindType(dataType, indexer.Type), new Get(CreateDescriber(getSetNode.Get.Describer), getSetNode.Get.Body), new Set(CreateDescriber(getSetNode.Set.Describer), getSetNode.Set.Body, FindType(dataType, getSetNode.Type)), indexer.Arguments));
                    break;
            }
        }

        private void PushExplicitCastNode<T>(T dataType, ExplicitCastDeclarationNode explicitCast) where T : DataType, ICastParent
        {
            dataType.AddExplicitCast(new ExplicitCastNode(FindType(dataType, explicitCast.Type), CreateDescriber(explicitCast.Describer), explicitCast.Body, explicitCast.Arguments));
        }

        private void PushImplicitCastNode<T>(T dataType, ImplicitCastDeclarationNode implicitCast) where T : DataType, ICastParent
        {
            dataType.AddImplicitCast(new ImplicitCastNode(FindType(dataType, implicitCast.Type), CreateDescriber(implicitCast.Describer), implicitCast.Body, implicitCast.Arguments));
        }

        private void PushOperatorOverloadNode<T>(T dataType, OperatorOverloadFunctionDeclarationNode overload) where T : DataType, IOperatorOverloadParent
        {
            dataType.AddOverload(new OperatorOverloadNode(overload.Operator, CreateDescriber(overload.Describer), overload.Body, FindType(dataType, overload.Type), overload.Arguments));
        }
    }
}
