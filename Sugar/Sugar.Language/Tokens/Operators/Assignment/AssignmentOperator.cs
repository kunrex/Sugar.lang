using System;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Operators.Binary;

namespace Sugar.Language.Tokens.Operators.Assignment
{
    internal sealed partial class AssignmentOperator : Operator
    {
        public override TokenType Type { get => TokenType.AssignmentOperator; }

        public BinaryOperator BaseOperator { get; private set; }

        private AssignmentOperator(BinaryOperator baseOperator) : base($"{baseOperator.Value}=", (OperatorKind)(baseOperator.SyntaxKind + 26), false, 13)
        {
            BaseOperator = baseOperator;
        }

        private AssignmentOperator(string _value, SyntaxKind _syntaxKind) : base(_value, _syntaxKind, false, 13)
        {

        }

        private AssignmentOperator(string _value, OperatorKind _operatorKind) : base(_value, _operatorKind, false, 13)
        {

        }

        public override Token Clone() => new AssignmentOperator(Value, SyntaxKind);
    }
}
