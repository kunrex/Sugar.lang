using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.DataTypes;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Functions;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Properties;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Structure;

namespace Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes
{
    internal sealed class Interface : TypeWrapper<InterfaceNode>, IGlobalFunctionParent, IPropertyParent
    {
        public override ProjectMemberEnum ProjectMemberType { get => ProjectMemberEnum.Interface; }

        private readonly List<GlobalVoidNode> voids;
        private readonly List<GlobalMethodNode> methods;

        private readonly List<IProperty> properties;

        public Interface(string _name, Describer _describer, InterfaceNode _skeleton, ReferenceCollection _references) : base(_name, _describer, _skeleton, _references)
        {
            voids = new List<GlobalVoidNode>();
            methods = new List<GlobalMethodNode>();

            properties = new List<IProperty>();
        }

        public IPropertyParent AddProperty(IProperty property)
        {
            properties.Add(property);

            return this;
        }

        public IProperty TryFindSetProperty(IdentifierNode identifier)
        {
            var property = FindEntity(identifier.Value, properties);

            switch (property.GlobalMember)
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

        public override string ToString() => $"Interface [Name: {name}]";
    }
}
