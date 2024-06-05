using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Interfaces.Creation;

namespace Sugar.Language.Parsing.Nodes.Functions.Calling.Structure
{
    internal sealed class FunctionArgumentNode : ParseNodeCollection, ICreationNode_Value
    {
        public override ParseNodeType NodeType { get => ParseNodeType.ArgumentCall; }

        private readonly DescriberNode describer;
        public DescriberNode Describer { get => describer; }

        private readonly ParseNodeCollection value;
        public ParseNodeCollection Value { get => value; }
        
        public FunctionArgumentNode(ParseNodeCollection _value) : base(_value)
        {
            describer = null;
            value = _value;
        }

        public FunctionArgumentNode(DescriberNode _describer, ParseNodeCollection _value) : base(_describer, _value)
        {
            describer = _describer;
            value = _value;
        }

        public override string ToString() => $"Function Argument Node";
    }
}
