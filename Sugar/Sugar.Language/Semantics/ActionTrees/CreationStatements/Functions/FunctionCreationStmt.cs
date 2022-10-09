using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions
{
    internal abstract class FunctionCreationStmt<Parent> : ReturnableCreationStatement<Parent> where Parent : IActionTreeNode
    {
        protected readonly Node nodeBody;
        public Node NodeBody { get => nodeBody; }

        protected readonly FunctionArguments arguments;
        public FunctionArguments FunctionArguments { get => arguments; }

        public FunctionCreationStmt(DataType _creationType, string _name, Describer _describer, DescriberEnum _allowed, FunctionArguments _arguments, Node _nodeBody) : base(
            _creationType,
            _name,
            _describer,
            _allowed)
        {
            nodeBody = _nodeBody;
            arguments = _arguments;
        }
    }
}
