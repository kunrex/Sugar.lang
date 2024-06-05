using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Types;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Interfaces.Creation;

namespace Sugar.Language.Parsing.Nodes.Statements.VariableCreation
{
    internal abstract class VariableCreationNode : StatementNode, ICreationNode_Type, ICreationNode_Name
    {
        protected readonly DescriberNode describer;
        public DescriberNode Describer { get => describer; }

        protected readonly TypeNode type;
        public TypeNode Type { get => type; }

        protected readonly IdentifierNode name;
        public virtual IdentifierNode Name { get => name; }

        public VariableCreationNode(DescriberNode _describer, TypeNode _type, IdentifierNode _name) : base(_describer, _type, _name)
        {
            describer = _describer;

            type = _type;
            name = _name;
        }
    }
}
