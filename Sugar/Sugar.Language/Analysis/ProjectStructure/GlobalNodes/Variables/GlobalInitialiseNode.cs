using System;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

namespace Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Variables;

internal sealed class GlobalInitialiseNode : GlobalVariableNode
{
    private readonly ParseNode value;
    public ParseNode Value { get => value; }
    
    public GlobalInitialiseNode(string _name, Describer _describer, DataType _type, ParseNode _value) : base(_name, _describer, _type)
    {
        value = _value;
    }
}