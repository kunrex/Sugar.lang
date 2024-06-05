using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Types;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Parsing.Nodes.Describers;

namespace Sugar.Language.Parsing.Nodes.Statements.VariableCreation
{
    internal class DeclarationNode : VariableCreationNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Declaration; }

        public DeclarationNode(DescriberNode _describer, TypeNode _type, IdentifierNode _name) : base(_describer, _type, _name)
        {
 
        }

        public override string ToString() => $"Declaration Node";
    }
}
