using System;
using Sugar.Language.Exceptions.Analytics.ClassMemberCreation;
using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Describers;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Expressions;
using Sugar.Language.Parsing.Nodes.Functions.Declarations;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;
using Sugar.Language.Parsing.Nodes.Functions.Properties;
using Sugar.Language.Parsing.Nodes.Statements.VariableCreation;
using Sugar.Language.Parsing.Nodes.Types.Subtypes;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Collections;
using Sugar.Language.Semantics.ActionTrees.Namespaces;
using Sugar.Language.Semantics.Analysis;
using Sugar.Language.Semantics.Services.Interfaces;

namespace Sugar.Language.Semantics.Services.Implementations
{
    internal sealed class ClassMemberService : IClassMemeberService
    {
        private readonly DefaultNameSpaceNode defaultNameSpace;
        private readonly CreatedNameSpaceCollectionNode createdNameSpaces;

        private readonly DescriberService describerService;

        private readonly SemanticsResult result;

        public ClassMemberService(DefaultNameSpaceNode _defaultNameSpace, CreatedNameSpaceCollectionNode _createdNameSpaces)
        {
            defaultNameSpace = _defaultNameSpace;
            createdNameSpaces = _createdNameSpaces;

            describerService = new DescriberService();

            result = new SemanticsResult("Class Member Creation");
        }

        public SemanticsResult Validate()
        {
            CreateCollectionGlobalMembers(defaultNameSpace);
            CreateCollectionGlobalMembers(createdNameSpaces);

            return result.Build();
        }

        private void CreateCollectionGlobalMembers(IDataTypeCollection collection)
        {
            for (int i = 0; i < collection.DataTypeCount; i++)
                CreateGlobalMembers(collection.GetSubDataType(i));
        }

        private void CreateCollectionGlobalMembers(INameSpaceCollection collection)
        {
            for (int i = 0; i < collection.NameSpaceCount; i++)
                CreateGlobalMembers(collection.GetSubNameSpace(i));
        }

        private void CreateGlobalMembers(CreatedNameSpaceNode nameSpace)
        {
            int i;
            for (i = 0; i < nameSpace.DataTypeCount; i++)
                CreateGlobalMembers(nameSpace.GetSubDataType(i));

            for (i = 0; i < nameSpace.NameSpaceCount; i++)
                CreateGlobalMembers(nameSpace.GetSubNameSpace(i));
        }

        private void CreateGlobalMembers(DataType dataType)
        {
            switch (dataType.TypeEnum)
            {
                case DataTypeEnum.Class:
                    CreateClassMembers((ClassType)dataType);
                    break;
            }
        }

        private void CreateClassMembers(ClassType classType)
        {
            foreach (var node in classType.Skeleton.GetChildren())
            {
                switch (node.NodeType)
                {
                    case NodeType.Initialise:
                    case NodeType.Declaration:
                        var creationNode = (VariableCreationNode)node;
                        var type = (CustomTypeNode)creationNode.Type;

                        var name = (IdentifierNode)creationNode.Name;
                        var describer = describerService.AnalyseDescriber((DescriberNode)creationNode.Describer);

                        if (creationNode.CreationType == NodeType.Variable)
                        {
                            VariableCreationStmt variableCreationNode;
                            if (creationNode.NodeType == NodeType.Declaration)
                                variableCreationNode = new VariableDeclarationStmt(classType.FindReferencedType(type.CustomType, createdNameSpaces), name, describer, false);
                            else
                                variableCreationNode = new VariableInitialisationStmt(classType.FindReferencedType(type.CustomType, createdNameSpaces), name, describer, false, (ExpressionNode)((InitializeNode)node).Value);

                            if (!variableCreationNode.ValidateDescriber())
                            {
                                result.Add(new InvalidDescriberException(((IdentifierNode)creationNode.Name).Value, variableCreationNode.Allowed, variableCreationNode.Describer));
                                break;
                            }

                            classType.AddDeclaration(variableCreationNode);
                            break;
                        }

                        PropertyCreationStmt propertyCreationNode;
                        var propertyType = creationNode.Name.NodeType;

                        switch (propertyType)
                        {
                            case NodeType.PropertyGet:
                                var getNode = (PropertyGetNode)creationNode.Name;
                                propertyCreationNode = CreateStatement(creationNode.NodeType, getNode.Get, null);
                                break;
                            case NodeType.PropertySet:
                                var setNode = (PropertySetNode)creationNode.Name;
                                propertyCreationNode = CreateStatement(creationNode.NodeType, null, setNode.Set);
                                break;
                            default:
                                var getSetNode = (PropertyGetSetNode)creationNode.Name;
                                propertyCreationNode = CreateStatement(creationNode.NodeType, getSetNode.Get, getSetNode.Set);
                                break;
                        }

                        if (!propertyCreationNode.ValidateDescriber())
                        {
                            result.Add(new InvalidDescriberException(((IdentifierNode)creationNode.Name).Value, propertyCreationNode.Allowed, propertyCreationNode.Describer));
                            break;
                        }

                        classType.AddDeclaration(propertyCreationNode);

                        PropertyCreationStmt CreateStatement(NodeType nodeType, Node get, Node set)
                        {
                            if (nodeType == NodeType.Declaration)
                                return new PropertyDeclarationStmt(classType.FindReferencedType(type.CustomType, createdNameSpaces), name, describer, get, set);
                            else
                                return new PropertyInitialisationStmt(classType.FindReferencedType(type.CustomType, createdNameSpaces), name, describer, get, set, (ExpressionNode)((InitializeNode)node).Value);
                        }
                        break;
                    case NodeType.FunctionDeclaration:
                        var functionDeclaration = (FunctionDeclarationNode)node;
                        var returnType = classType.FindReferencedType(functionDeclaration.Type, createdNameSpaces);
                        var arguments = CreateArguments((FunctionDeclarationArgumentsNode)functionDeclaration.Arguments, classType);

                        var method = new MethodDeclarationStmt(returnType, describerService.AnalyseDescriber((DescriberNode)functionDeclaration.Describer), arguments, functionDeclaration);

                        if (!method.ValidateDescriber())
                        {
                            result.Add(new InvalidDescriberException(((IdentifierNode)functionDeclaration.Name).Value, method.Allowed, method.Describer));
                            break;
                        }

                        classType.AddDeclaration(method);
                        break;
                }
            }
        }

        private FunctionArguments CreateArguments(FunctionDeclarationArgumentsNode argumentsNode, DataType type)
        {
            var arguments = new FunctionArguments();

            for (int i = 0; i < argumentsNode.ChildCount; i++)
            {
                var child = (FunctionDeclarationArgumentNode)argumentsNode[i];

                arguments.Add(((IdentifierNode)child.Name).Value, type.FindReferencedType(child.Type, createdNameSpaces));
            }

            return arguments;
        }
    }
}
