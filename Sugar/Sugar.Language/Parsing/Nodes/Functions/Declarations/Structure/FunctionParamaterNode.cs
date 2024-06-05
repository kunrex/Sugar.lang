using System;

using Sugar.Language.Parsing.Nodes.Types;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Statements.VariableCreation;

namespace Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure
{
    internal sealed class FunctionParamaterNode : InitializeNode
    {
        public override ParseNodeType NodeType => ParseNodeType.ArgumentDeclaration;

        public FunctionParamaterNode(DescriberNode _describer, TypeNode _type, IdentifierNode _name) : base(_describer, _type, _name, new DefaultValueNode())
        {
            
        }

        public FunctionParamaterNode(DescriberNode _describer, TypeNode _type, IdentifierNode _name, ParseNodeCollection _defaultValue) : base(_describer, _type, _name, _defaultValue)
        {
           
        }

        public override string ToString() => $"Function Paramater Node";
    }
}
