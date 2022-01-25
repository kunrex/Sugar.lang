using System;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Operators.Assignment;

namespace Sugar.Language.Tokens.Operators.Binary
{
    internal sealed partial class BinaryOperator : Operator
    {
        public override TokenType Type => TokenType.BinaryOperator;
        protected override byte OperatorTypeId { get => 1; }

        private BinaryOperator(string _value, byte _id, OperatorType _operatorType, bool _leftAssociative, int _precedence) : base(_value, _id, _operatorType, _leftAssociative, _precedence)
        {

        }

        private BinaryOperator(BinaryOperator other) : base(other.Value, other.Id, other.OperatorType, other.LeftAssociative, other.Precedence)
        {

        }

        public override Token Clone() => new BinaryOperator(this);

        public AssignmentOperator CreateAssignment(int _index) => new AssignmentOperator(this, _index);
    }
}
