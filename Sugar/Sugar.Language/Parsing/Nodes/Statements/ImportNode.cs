using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.DataTypes.Enums;

namespace Sugar.Language.Parsing.Nodes.Statements
{
    internal sealed class ImportNode : StatementNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Import; }

        private readonly CreationType creationType;
        public CreationType CreationType { get => creationType; }

        private readonly ParseNodeCollection baseName;
        public ParseNodeCollection BaseName { get => baseName; }

        public ImportNode(CreationType _creationType, ParseNodeCollection _name) : base(_name)
        {
            creationType = _creationType;
            baseName = _name;
        }

        public override string ToString() => $"Import Statement Node [Type: {creationType}]";
    }
}
