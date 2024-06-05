using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Interfaces.Creation;

namespace Sugar.Language.Parsing.Nodes.DataTypes
{
    internal sealed class NamespaceNode : ParseNodeCollection, ICreationNode_Body
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Namespace; } 

        private readonly DescriberNode describer;
        public DescriberNode Describer { get => describer; }

        private readonly ParseNodeCollection name;
        public ParseNodeCollection Name { get => name; }

        private readonly ParseNode body;
        public ParseNode Body { get => body; }

        public NamespaceNode(DescriberNode _describer, ParseNodeCollection _name, ParseNode _body) : base(_describer, _name, _body)
        {
            describer = _describer;

            name = _name;
            body = _body;
        }

        public override string ToString() => $"Name Space Node";
    }
}
