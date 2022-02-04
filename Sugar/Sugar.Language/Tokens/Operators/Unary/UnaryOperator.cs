using System;
using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Operators.Unary
{
    internal sealed partial class UnaryOperator : Operator
    {
        public override TokenType Type { get => TokenType.UnaryOperator; }

        private UnaryOperator(string _value, OperatorKind _operatorType, bool _leftAssociative, int _precedence) : base(_value, _operatorType, _leftAssociative, _precedence)
        {

        }

        private UnaryOperator(string _value, SyntaxKind _syntaxKind, bool _leftAssociative, int _precedence) : base(_value, _syntaxKind, _leftAssociative, _precedence)
        {

        }

        public override Token Clone() => new UnaryOperator(Value, SyntaxKind, LeftAssociative, Precedence);
    }
}
