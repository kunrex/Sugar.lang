using System;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Operators.Binary;

namespace Sugar.Language.Tokens.Operators.Assignment
{
    internal sealed partial class AssignmentOperator : Operator
    {
        public override TokenType Type => TokenType.AssignmentOperator;
        protected override byte OperatorTypeId { get => 2; }

        public BinaryOperator BaseOperator { get; private set; }

        public AssignmentOperator(byte _id) : base("=", _id, OperatorType.Assignment, false, 13)
        {

        }

        public AssignmentOperator(BinaryOperator baseOperator, int _index) : base($"{baseOperator.Value}=", (byte)(baseOperator.UniqueID % 100), OperatorType.Assignment, false, 13)
        {
            BaseOperator = baseOperator;
            Index = _index;
        }

        private AssignmentOperator(AssignmentOperator other) : base(other.Value, other.UniqueID, OperatorType.Assignment, false, 13)
        {

        }

        public override Token Clone() => new AssignmentOperator(this);
    }
}
