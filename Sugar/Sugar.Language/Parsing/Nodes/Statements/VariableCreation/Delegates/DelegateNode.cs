using System;
using System.Collections.Generic;

namespace Sugar.Language.Parsing.Nodes.Statements.VariableCreation
{
    internal abstract class DelegateNode : VariableCreationNode
    {
        public Node Arguments { get => Children[3]; }
        public Node FunctionBody { get => Children[4]; }

        public DelegateNode(Node _describer, Node _type, Node _name, Node _arguments, Node _functionBody) : base(_describer, _type, _name)
        {
            Children = new List<Node>() { _describer, _type, _name, _arguments, _functionBody };
        }
    }
}
