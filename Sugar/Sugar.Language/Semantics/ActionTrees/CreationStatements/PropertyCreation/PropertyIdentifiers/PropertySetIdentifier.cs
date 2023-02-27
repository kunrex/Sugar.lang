using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Local;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation.PropertyIdentifiers
{
    internal sealed class PropertySetIdentifier : PropertyIdentifier
    {
        private readonly LocalVariableDeclarationStmt value;
        public LocalVariableDeclarationStmt Value { get => value; }

        public override ActionNodeEnum ActionNodeType { get => ActionNodeEnum.PropertySet; }

        public PropertySetIdentifier(Describer _describer, Node _body, DataType _creationType) : base(_describer, _body)
        {
            value = new LocalVariableDeclarationStmt(_creationType, IdentifierNode.ValueIdentifier, new Describer(DescriberEnum.Const));

            scope.AddDeclaration(value);
        }

        public override void Print(string indent, bool last) => Console.WriteLine(indent + " Set Property Identifier");
    }
}
