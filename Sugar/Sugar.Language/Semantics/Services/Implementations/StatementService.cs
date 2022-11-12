using System;
using System.Collections.Generic;

using Sugar.Language.Semantics.Analysis;
using Sugar.Language.Semantics.Services.Interfaces;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Namespaces;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation;
using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Functions.Declarations;
using Sugar.Language.Parsing.Nodes.Types;
using Sugar.Language.Parsing.Nodes.Types.Subtypes;
using Sugar.Language.Parsing.Nodes.Types.Enums;
using Sugar.Language.Semantics.Services.Implementations.Structures;
using Sugar.Language.Parsing.Nodes.Describers;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Local.FunctionArguments;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Local;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation;
using Sugar.Language.Parsing.Nodes.Expressions;
using Sugar.Language.Semantics.ActionTrees;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation.PropertyIdentifiers;
using Sugar.Language.Parsing.Nodes.Statements.VariableCreation;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Local;
using Sugar.Language.Parsing.Nodes.Expressions.Associative;
using Sugar.Language.Semantics.ActionTrees.CreationStatements;
using Sugar.Language.Semantics.ActionTrees.Interfaces;

namespace Sugar.Language.Semantics.Services.Implementations
{
    internal sealed class StatementService : IStatementService
    {
        private readonly DefaultNameSpaceNode defaultNameSpace;
        private readonly CreatedNameSpaceCollectionNode createdNameSpaces;

        private readonly SemanticsResult result;

        private readonly DescriberService describerService;

        public StatementService(DefaultNameSpaceNode _default, CreatedNameSpaceCollectionNode _collection)
        {
            defaultNameSpace = _default;
            createdNameSpaces = _collection;

            describerService = new DescriberService();

            result = new SemanticsResult("Statement Validation");
        }

        public SemanticsResult Validate()
        {
            var integer = defaultNameSpace.GetInternalDataType(Analysis.BuiltInTypes.Enums.TypeEnum.Integer);
            Validate((StructType)integer);

            return result.Build();
        }

        private void Validate(StructType dataType)
        {
            var subTypeSearcher = new SubTypeSearcherService(dataType, defaultNameSpace, createdNameSpaces);
            var functions = dataType.GetAllMembers(MemberEnum.Functions);

            foreach (var function in functions)
            {
                var converted = (IFunction)function;

                var body = converted.Scope.Body;
                if (body.NodeType == NodeType.Scope)
                    CreateLocalMembers(converted.Scope, subTypeSearcher);
                else if(body.NodeType == NodeType.FunctionDeclaration)
                {
                    CreateLocalFunction((FunctionDeclarationNode)body, converted, subTypeSearcher);
                    //probably throw a warning
                }
            }

            var properties = dataType.GetAllMembers(MemberEnum.Properties);

            foreach(var property in properties)
            {
                var declaration = (IProperty)property;

                switch (declaration.PropertyType)
                {
                    case PropertyTypeEnum.Get:
                        CreateLocalMembers(declaration.GetExpression.Scope, subTypeSearcher);
                        break;
                    case PropertyTypeEnum.Set:
                        CreateLocalMembers(declaration.SetExpression.Scope, subTypeSearcher);
                        break;
                    case PropertyTypeEnum.GetSet:
                        CreateLocalMembers(declaration.GetExpression.Scope, subTypeSearcher);
                        CreateLocalMembers(declaration.SetExpression.Scope, subTypeSearcher);
                        break;
                }
            }

            foreach(var function in functions)
            {
                var converted = (IFunction)function;

                var scope = converted.Scope;
                var body = scope.Body;

                if(body.NodeType == NodeType.Scope)
                    foreach(var child in scope.Body.GetChildren())
                    {
                        switch(child.NodeType)
                        {
                            case NodeType.Scope:
                            case NodeType.FunctionDeclaration:
                                break;
                            case NodeType.Declaration:
                            case NodeType.Initialise:
                                CreatLocalVariable((DeclarationNode)child, scope, subTypeSearcher);
                                break;
                            case NodeType.Assignment:
                                var assignment = (AssignmentNode)child;

                                var value = assignment.Value;

                                break;
                        }
                    }
            }
        }

        //hi future me, yeah im kinda happy rn :'D
        //okie anyway, i implemented the new parenting thing, creation type enum and all of that
        //so kindly move onto checking if a variable currently exists or not :)
        private void CreateLocalMembers(Scope parent, SubTypeSearcherService subTypeSearcher)
        {
            var body = parent.Body;

            if (body.NodeType == NodeType.Scope)
                foreach (var child in body.GetChildren())
                    switch (child.NodeType)
                    {
                        case NodeType.Scope:
                            var scope = new Scope(child, parent);
                            CreateLocalMembers(scope, subTypeSearcher);

                            parent.AddScope(scope);
                            break;
                        case NodeType.FunctionDeclaration:
                            CreateLocalFunction((FunctionDeclarationNode)child, parent, subTypeSearcher);
                            break;
                    }
        }

        private void CreatLocalVariable(DeclarationNode declaration, Scope scope, SubTypeSearcherService subTypeSearcher)
        {
            var name = (IdentifierNode)declaration.Name;
            var type = FindReferencedType(subTypeSearcher, (TypeNode)declaration.Type);
            var describer = describerService.AnalyseDescriber((DescriberNode)declaration.Describer);

            switch(declaration.NodeType)
            {
                case NodeType.Declaration:
                    scope.AddDeclaration(new LocalVariableDeclarationStmt(type, name, describer));
                    break;
                case NodeType.Initialise:
                    scope.AddDeclaration(new LocalVariableInitialisationStmt(type, name, describer, (ExpressionNode)((InitializeNode)declaration).Value));
                    break;
            }
        }

        private IParentableCreationStatement ReferenceVariable(Node name, IScopeParent parent, SubTypeSearcherService subTypeSearcher)
        {
            IdentifierNode immedeate;
            if (name.NodeType == NodeType.Dot)
                immedeate = (IdentifierNode)((DotExpression)name).LHS;
            else
                immedeate = (IdentifierNode)name;

            LocalVariableDeclarationStmt declaration = null;
            while (true)
            {
                var result = parent.TryFindVariableCreation(immedeate);

                if (result == null)
                {
                    parent = parent.Parent;

                    if (parent == null)
                        break;
                }
                else
                {
                    declaration = result;
                    break;
                }
            }

            if (declaration == null)
                return null;
            else if (name.NodeType == NodeType.Variable)
                return declaration;
            else
                return ReferenceVariable(name, declaration.CreationType);
        }

        //basically, referencing variables recursivley should work now, do some clean up here cause its pretty ugly
        //theres gonna be a bunch of errors when you compile telling you to implement CreationType in all CreationStatements so umm yeah fix that lmao
        //yk the current implementation is pretty shit, you should defintly sit down for some time and think this through, i mean the idea is solid just the way its implement isn't
        private GlobalVariableDeclarationStmt ReferenceVariable(Node name, DataType dataType)
        {
            GlobalVariableDeclarationStmt declaration = null;
            name = ((DotExpression)name).RHS;

            switch (name.NodeType)
            {
                case NodeType.Variable:
                    return declaration;
                default:
                    //x.y.z
                    var current = ((DotExpression)name).RHS;

                    while (true)
                    {
                        switch (current.NodeType)
                        {
                            case NodeType.Dot:
                                var dot = (DotExpression)current;
                                var lhs = (IdentifierNode)dot.LHS;

                                switch (dataType.TypeEnum)
                                {
                                    case DataTypeEnum.Class:
                                    case DataTypeEnum.Struct:
                                        IGeneralContainer generalContainer = (IGeneralContainer)dataType;

                                        var variable = generalContainer.TryFindVariableCreation(lhs);
                                        if(variable == null)
                                        {
                                            //check property

                                            if(variable == null)//if its still null
                                                return null;
                                        }

                                        declaration = variable;
                                        current = dot.RHS;
                                        break;
                                    case DataTypeEnum.Interface:
                                        //check property

                                        break;
                                }
                                break;
                            default:
                                var identifier = (IdentifierNode)current;

                                switch (dataType.TypeEnum)
                                {
                                    case DataTypeEnum.Class:
                                    case DataTypeEnum.Struct:
                                        IGeneralContainer generalContainer = (IGeneralContainer)dataType;

                                        var variable = generalContainer.TryFindVariableCreation(identifier);
                                        if (variable == null)
                                        {
                                            //check property

                                           
                                        }

                                        if (variable == null)//if its still null
                                            return null;
                                        break;
                                    case DataTypeEnum.Interface:
                                        //check property

                                        break;
                                }
                                return declaration;
                        }
                    }
            }
        }

        //then you can move onto orderely creation of variables and storing which variables have currently been created
        //after that expressions can be evaluated, allowed operators, type mismatches and all that good stuff can be checked since everything is accessible
        private void CreateLocalFunction(FunctionDeclarationNode function, IFunctionContainer<LocalMethodCreationStmt, LocalVoidDeclarationStmt> parent, SubTypeSearcherService subTypeSearcher)
        {
            var name = (IdentifierNode)function.Name;
            var functionInfo = GatherArguments(function, subTypeSearcher, (TypeNode)function.Type);

            IFunction localFunction;
            if (functionInfo.ReturnType == null)
            {
                var localVoid = new LocalVoidDeclarationStmt(name, functionInfo.Describer, functionInfo.Arguments, functionInfo.Body);
                parent.AddDeclaration(localVoid);

                localFunction = localVoid;
            }
            else
            {
                var localMethod = new LocalMethodCreationStmt(functionInfo.ReturnType, name, functionInfo.Describer, functionInfo.Arguments, functionInfo.Body);
                parent.AddDeclaration(localMethod);

                localFunction = localMethod;
            }

            CreateLocalMembers(localFunction.Scope, subTypeSearcher);
        }

        private FunctionInfo GatherArguments(BaseFunctionDeclarationNode baseFunction, SubTypeSearcherService subTypeSearcher, TypeNode type) => new FunctionInfo(baseFunction.Body,
                                    FindReferencedType(subTypeSearcher, type),
                                    new Describer(0),
                                    CreateArguments(subTypeSearcher, (FunctionDeclarationArgumentsNode)baseFunction.Arguments));

        private DataType FindReferencedType(SubTypeSearcherService subTypeSearcher, TypeNode type)
        {
            switch (type.Type)
            {
                case TypeNodeEnum.BuiltIn:
                    return defaultNameSpace.GetInternalDataType(type.ReturnType());
                case TypeNodeEnum.Constructor:
                    return subTypeSearcher.TryFindReferencedType((CustomTypeNode)((ConstructorTypeNode)type).ConstructorReturnType);
                case TypeNodeEnum.Void:
                    return null;
                default:
                    return subTypeSearcher.TryFindReferencedType(((CustomTypeNode)type).CustomType);
            }
        }

        private FunctionArguments CreateArguments(SubTypeSearcherService subTypeSearcher, FunctionDeclarationArgumentsNode argumentsNode)
        {
            var arguments = new FunctionArguments();

            for (int i = 0; i < argumentsNode.ChildCount; i++)
            {
                var child = (FunctionDeclarationArgumentNode)argumentsNode[i];

                IFunctionArgument argument;
                var name = (IdentifierNode)child.Name;
                var describer = describerService.AnalyseDescriber((DescriberNode)child.Describer);

                if (!describer.ValidateAccessor(DescriberEnum.ReferenceModifiers))
                {
                    Console.WriteLine("Invalid Describer for argument");
                    continue;
                }

                if (child.DefaultValue == null)
                    argument = new FunctionArgumentDeclarationStmt(FindReferencedType(subTypeSearcher, (TypeNode)child.Type), name, describer);
                else
                    argument = new FunctionArgumentInitialisationStmt(FindReferencedType(subTypeSearcher, (TypeNode)child.Type), name, describer, (ExpressionNode)child.DefaultValue);

                arguments.Add(name.Value, argument);
            }

            return arguments;
        }

        /*private VariableCreationStmt ReferenceFinalVariable(Node node, IVariableContainer start)
        {
            IVariableContainer current = start;

            while(true)
            {
                switch(node.NodeType)
                {
                    case Parsing.Nodes.Enums.NodeType.Dot:
                        break;
                    case Parsing.Nodes.Enums.NodeType.Variable:
                        var identifer = (IdentifierNode)node;
                        Console.WriteLine(identifer.Value);

                        var variable = current.TryFindVariableCreation(identifer);
                        if (variable == null)
                        {
                            Console.WriteLine("null");
                            return null;
                        }

                        return variable;
                }
            }
        }*/
    }
}
