using System;
using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Operators.Unary
{
    internal sealed partial class UnaryOperator : Operator
    {
        public override TokenType Type => TokenType.UnaryOperator;

        protected override byte OperatorTypeId { get => 0; }

        private UnaryOperator(string _value, byte _id, OperatorType _operatorType, bool _leftAssociative, int _precedence) : base(_value, _id, _operatorType, _leftAssociative, _precedence)
        {

        }

        private UnaryOperator(UnaryOperator other) : base(other.Value, other.Id, other.OperatorType, other.LeftAssociative, other.Precedence)
        {

        }

        public override Token Clone() => new UnaryOperator(this);
    }
}
