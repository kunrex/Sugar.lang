using System;
using System.Collections.Generic;

using Sugar.Language.Tokens.Operators;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.DataTypes;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Variables;
using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions;
using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions.Casting;
using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions.Extensions;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Functions;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Variables;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Properties;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Structure;

namespace Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes
{
    internal sealed class Class : TypeWrapper<ClassNode>, IGlobalVariableParent, IGlobalFunctionParent, IExtensionFunctionParent, IConstructorParent, ICastParent, IOperatorOverloadParent, IIndexerParent
    {
        public override ProjectMemberEnum ProjectMemberType { get => ProjectMemberEnum.Class; }

        private readonly List<ConstructorNode> constructors = new List<ConstructorNode>();

        private readonly List<GlobalVariableNode> variables = new List<GlobalVariableNode>();

        private readonly List<GlobalVoidNode> voids;
        private readonly List<GlobalMethodNode> methods;

        private readonly List<IProperty> properties;

        private readonly List<IIndexer> indexers;
        private readonly List<OperatorOverloadNode> overloads;

        private readonly List<ImplicitCastNode> implicitCasts;
        private readonly List<ExplicitCastNode> explicitCasts;

        public Class(string _name, Describer _describer, ClassNode _skeleton, ReferenceCollection _references) : base(_name, _describer, _skeleton, _references)
        {
            constructors = new List<ConstructorNode>();

            variables = new List<GlobalVariableNode>();

            voids = new List<GlobalVoidNode>();
            methods = new List<GlobalMethodNode>();

            properties = new List<IProperty>();

            indexers = new List<IIndexer>();
            overloads = new List<OperatorOverloadNode>();
            implicitCasts = new List<ImplicitCastNode>();
            explicitCasts = new List<ExplicitCastNode>();
        }


        public IConstructorParent AddConstructor(ConstructorNode constructor)
        {
            constructors.Add(constructor);

            return this;
        }

        public ConstructorNode TryFindGlobalConstructor(FunctionParamatersNode parameters) => FindFunction(name, constructors, parameters);

        public IGlobalVariableParent AddVariable(GlobalVariableNode variable)
        {
            variables.Add(variable);

            return this;
        }

        public GlobalVariableNode TryFindVariable(IdentifierNode identifier) => FindEntity(identifier.Value, variables);

        public IPropertyParent AddProperty(IProperty property)
        {
            properties.Add(property);

            return this;
        }

        public IProperty TryFindSetProperty(IdentifierNode identifier)
        {
            var property = FindEntity(identifier.Value, properties);

            switch(property.GlobalMember)
            {
                case GlobalMemberEnum.PropertySet:
                case GlobalMemberEnum.PropertyGetSet:
                    return property;
                default:
                    return null;
            }
        }

        public IProperty TryFindGetProperty(IdentifierNode identifier)
        {
            var property = FindEntity(identifier.Value, properties);

            switch (property.GlobalMember)
            {
                case GlobalMemberEnum.PropertyGet:
                case GlobalMemberEnum.PropertyGetSet:
                    return property;
                default:
                    return null;
            }
        }

        public IGlobalFunctionParent AddGlobalVoid(GlobalVoidNode globalVoid)
        {
            voids.Add(globalVoid);

            return this;
        }

        public GlobalVoidNode TryFindGlobalVoid(IdentifierNode identifier, FunctionParamatersNode parameters) => FindFunction(identifier.Value, voids, parameters);

        public IGlobalFunctionParent AddGlobalMethod(GlobalMethodNode globalMethod)
        {
            methods.Add(globalMethod);

            return this;
        }

        public GlobalMethodNode TryFindGlobalMethod(IdentifierNode identifier, FunctionParamatersNode parameters) => FindFunction(identifier.Value, methods, parameters);

        public IGlobalFunctionParent AddExtensionVoid(ExtensionVoidNode extensionVoid) => AddGlobalVoid(extensionVoid);
        public IGlobalFunctionParent AddExtensionMethod(ExtensionMethodNode extensionMethod) => AddGlobalMethod(extensionMethod);

        public ExtensionVoidNode TryFindExtensionVoid(IdentifierNode identifier, FunctionParamatersNode parameters) => TryFindGlobalVoid(identifier, parameters) as ExtensionVoidNode;
        public ExtensionMethodNode TryFindExtensionMethod(IdentifierNode identifier, FunctionParamatersNode parameters) => TryFindGlobalMethod(identifier, parameters) as ExtensionMethodNode;

        public IPropertyParent AddIndexer(IIndexer indexer)
        {
            indexers.Add(indexer);

            return this;
        }

        public IIndexer TryFindSetIndexer(IdentifierNode identifier)
        {
            var property = FindEntity(identifier.Value, indexers);

            switch (property.PropertyType)
            {
                case GlobalMemberEnum.PropertySet:
                case GlobalMemberEnum.PropertyGetSet:
                    return property;
                default:
                    return null;
            }
        }

        public IIndexer TryFindGetIndexer(IdentifierNode identifier)
        {
            var property = FindEntity(identifier.Value, indexers);

            switch (property.PropertyType)
            {
                case GlobalMemberEnum.PropertyGet:
                case GlobalMemberEnum.PropertyGetSet:
                    return property;
                default:
                    return null;
            }
        }

        public IConstructorParent AddImplicitCast(ImplicitCastNode implicitCast)
        {
            implicitCasts.Add(implicitCast);

            return this;
        }

        public ImplicitCastNode TryFindImplicitCast(DataType dataType, FunctionParamatersNode parameters) => FindCast(dataType, implicitCasts, parameters);

        public IConstructorParent AddExplicitCast(ExplicitCastNode explicitCast)
        {
            explicitCasts.Add(explicitCast);

            return this;
        }

        public ExplicitCastNode TryFindExplicitCast(DataType dataType, FunctionParamatersNode parameters) => FindCast(dataType, explicitCasts, parameters);

        public IConstructorParent AddOverload(OperatorOverloadNode overload)
        {
            overloads.Add(overload);

            return this;
        }

        public OperatorOverloadNode TryFindOverload(Operator overload, FunctionParamatersNode parameters)
        {
            foreach (var operatorOverload in overloads)
            {
                if (operatorOverload.BaseOperator == overload)
                    return operatorOverload;
            }

            return null;
        }

        public override string ToString() => $"Class [Name: {name}]";
    }
}
