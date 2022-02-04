using System;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Operators.Assignment;

namespace Sugar.Language.Tokens.Operators.Binary
{
    internal sealed partial class BinaryOperator : Operator
    {
        public override TokenType Type { get => TokenType.BinaryOperator; }

        private BinaryOperator(string _value, OperatorKind _operatorType, bool _leftAssociative, int _precedence) : base(_value, _operatorType, _leftAssociative, _precedence)
        {

        }

        private BinaryOperator(string _value, SyntaxKind _syntaxKind, bool _leftAssociative, int _precedence) : base(_value, _syntaxKind, _leftAssociative, _precedence)
        {

        }

        public override Token Clone() => new BinaryOperator(Value, SyntaxKind, LeftAssociative, Precedence);
    }
}
