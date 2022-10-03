using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements
{
    internal abstract class ReturnableCreationStatement<Parent> : CreationStatement<Parent>, IReturnableCreationStatement where Parent : IActionTreeNode
    {
        protected readonly DataType creationType;
        public DataType CreationType { get => creationType; }

        public ReturnableCreationStatement(DataType _creationType, IdentifierNode _creationName, Describer _describer, DescriberEnum _allowed) : base(_creationName,
            _describer,
            _allowed)
        {
            creationType = _creationType;
        }
    }
}
