using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.UDDataTypes;

using Sugar.Language.Semantics.ActionTrees.Enums;

namespace Sugar.Language.Semantics.ActionTrees.DataTypes
{
    internal sealed class EnumType : DataTypeWrapper<EnumNode>
    {
        public override DataTypeEnum TypeEnum { get => DataTypeEnum.Enum; }

        public EnumType(IdentifierNode _name, List<ImportNode> _imports, EnumNode _skeleton) : base(_name, _imports, _skeleton)
        {

        }

        public override string ToString() => $"Enum Node [{name.Value}]";
    }
}
