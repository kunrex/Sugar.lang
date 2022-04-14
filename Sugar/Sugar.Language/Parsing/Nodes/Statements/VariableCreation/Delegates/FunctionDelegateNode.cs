using System;

using Sugar.Language.Parsing.Nodes.Enums;


namespace Sugar.Language.Parsing.Nodes.Statements.VariableCreation.Delegates
{
    internal sealed class FunctionDelegateNode : DelegateNode
    {
        public override NodeType NodeType { get => NodeType.Function; }

        public FunctionDelegateNode(Node _describer, Node _type, Node _name, Node _arguments, Node _functionBody) : base(_describer, _type, _name, _arguments, _functionBody)
        {

        }

        public override string ToString() => $"Action Decleration Node";
    }
}
