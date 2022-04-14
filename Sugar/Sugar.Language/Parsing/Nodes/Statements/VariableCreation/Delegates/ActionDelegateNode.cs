using System;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Types.Subtypes;

namespace Sugar.Language.Parsing.Nodes.Statements.VariableCreation.Delegates
{
    internal sealed class ActionDelegateNode : DelegateNode
    {
        public override NodeType NodeType { get => NodeType.Action; }

        public ActionDelegateNode(Node _describer, Node _name, Node _arguments, Node _functionBody) : base(_describer, new ActionTypeNode(), _name, _arguments, _functionBody)
        {

        }

        public override string ToString() => $"Action Decleration Node";
    }
}
