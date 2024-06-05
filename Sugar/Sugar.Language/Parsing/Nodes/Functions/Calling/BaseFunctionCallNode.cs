using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Values.Generics;

using Sugar.Language.Parsing.Nodes.Interfaces.Generic;

using Sugar.Language.Parsing.Nodes.Functions.Calling.Structure;

namespace Sugar.Language.Parsing.Nodes.Functions.Calling
{
    internal abstract class BaseFunctionCallNode : ParseNodeCollection, IGenericCall
    {
        protected readonly ParseNodeCollection name;
        public ParseNodeCollection Name { get => name; }

        protected readonly FunctionArgumentsNode arguments;
        public FunctionArgumentsNode Arguments { get => arguments; }

        protected readonly GenericCallNode generic;
        public GenericCallNode Generic { get => generic; }

        public BaseFunctionCallNode(ParseNodeCollection _name, FunctionArgumentsNode _arguments) : base(_name, _arguments)
        {
            name = _name;
            arguments = _arguments;
        }

        public BaseFunctionCallNode(ParseNodeCollection _name, FunctionArgumentsNode _arguments, GenericCallNode _generic) : base(_name, _arguments, _generic)
        {
            name = _name;
            arguments = _arguments;

            generic = _generic;
        }

        public override ParseNode AddChild(ParseNode node) { return this; }
    }
}
