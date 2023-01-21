using System;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Statements.VariableCreation;

namespace Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure
{
    internal sealed class FunctionDeclarationArgumentNode : InitializeNode
    {
        public override NodeType NodeType => NodeType.ArgumentDeclaration;

        public Node DefaultValue { get => ChildCount == 3 ? null : Children[3]; }

        public FunctionDeclarationArgumentNode(Node _describer, Node _type, Node _name) : base (_describer, _type, _name, null)
        {
            
        }

        public FunctionDeclarationArgumentNode(Node _describer, Node _type, Node _name, Node _defaultValue) : base(_describer, _type, _name, _defaultValue)
        {
           
        }

        public override string ToString() => $"Function Declaration Argument Node";
    }
}
