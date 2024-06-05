using System;

using Sugar.Language.Parsing.Nodes.Types;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Declarations.Delegates;

namespace Sugar.Language.Parsing.Nodes.Statements.VariableCreation
{
    internal class DelegateDeclarationNode : VariableCreationNode
    {
        private readonly DelegateNode delegateNode;
        public DelegateNode Delegate { get => delegateNode; }

        public DelegateDeclarationNode(DescriberNode _describer, TypeNode _type, IdentifierNode _name, DelegateNode _delegate) : base(_describer, _type, _name)
        {
            delegateNode = _delegate;
            Add(delegateNode);
        }
    }
}
