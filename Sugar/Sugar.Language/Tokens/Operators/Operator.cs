using System;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Operators.Unary;

namespace Sugar.Language.Tokens.Operators
{
    internal abstract partial class Operator : Token
    {
        public int Precedence { get; private set; }//lower the value, greater the precedance
        public bool LeftAssociative { get; protected set; }
        public OperatorKind OperatorType { get; protected set; }

        protected Operator(string _value, OperatorKind _operatorType, bool _leftAssociative, int _precedence) : base(_value, (SyntaxKind)_operatorType)
        {
            OperatorType = _operatorType;

            Precedence = _precedence;
            LeftAssociative = _leftAssociative;
        }

        protected Operator(string _value, SyntaxKind _syntaxKind, bool _leftAssociative, int _precedence) : base(_value, _syntaxKind)
        {
            Value = _value;
            SyntaxKind = _syntaxKind;

            Precedence = _precedence;
            LeftAssociative = _leftAssociative;
        }

        public static Operator ConvertUnaryToPrefix(Operator _operator)
        {
            if (_operator == UnaryOperator.Increment)
                return UnaryOperator.IncrementPrefix;
            else if (_operator == UnaryOperator.Decrement)
                return UnaryOperator.DecrementPrefix;
            return _operator;
        }
    }
}
