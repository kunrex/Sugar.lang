using System;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Operators.Unary;

namespace Sugar.Language.Tokens.Operators
{
    internal abstract partial class Operator : Token
    {
        public int Precedence { get; private set; }//lower the value, greater the precedance
        public bool LeftAssociative { get; protected set; }
        public OperatorType OperatorType { get; protected set; }

        protected override byte TypeID { get => 4; }
        protected abstract byte OperatorTypeId { get; }

        public override int SubType => (int)OperatorType;

        protected Operator(string _value, byte _id, OperatorType _operatorType, bool _leftAssociative, int _precedence) : base(_value, _id)
        {
            OperatorType = _operatorType;

            LeftAssociative = _leftAssociative;
            Precedence = _precedence;

            Id = (ushort)(TypeID * 1000 + OperatorTypeId * 100 + _id);
        }

        protected Operator(string _value, ushort _id, OperatorType _operatorType, bool _leftAssociative, int _precedence) : base()
        {
            Value = _value;
            Id = _id;

            OperatorType = _operatorType;
            LeftAssociative = _leftAssociative;
            Precedence = _precedence;
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
