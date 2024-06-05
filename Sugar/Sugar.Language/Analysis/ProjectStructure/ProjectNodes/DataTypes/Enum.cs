using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.DataTypes;
using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Variables;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Variables;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Structure;

namespace Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes
{
    internal sealed class Enum : TypeWrapper<EnumNode>, IGlobalVariableParent
    {
        public override ProjectMemberEnum ProjectMemberType { get => ProjectMemberEnum.Enum; }

        private readonly List<GlobalVariableNode> variables = new List<GlobalVariableNode>();

        public Enum(string _name, Describer _describer, EnumNode _skeleton, ReferenceCollection _references) : base(_name, _describer, _skeleton, _references)
        {
            variables = new List<GlobalVariableNode>();
        }

        public IGlobalVariableParent AddVariable(GlobalVariableNode variable)
        {
            variables.Add(variable);

            return this;
        }

        public GlobalVariableNode TryFindVariable(IdentifierNode identifier) => FindEntity(identifier.Value, variables);

        public override string ToString() => $"Enum [Name: {name}]";
    }
}
