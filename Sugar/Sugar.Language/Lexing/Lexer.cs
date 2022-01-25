using System;
using System.Text;
using System.Collections.Generic;

using Sugar.Language.Tokens;
using Sugar.Language.Exceptions;
using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Keywords;
using Sugar.Language.Tokens.Constants;
using Sugar.Language.Tokens.Seperators;
using Sugar.Language.Tokens.Operators.Unary;
using Sugar.Language.Tokens.Operators.Binary;
using Sugar.Language.Tokens.Constants.Numeric.Real;
using Sugar.Language.Tokens.Constants.Numeric.Integral;

namespace Sugar.Language.Lexing
{
    public sealed class Lexer
    {
        private readonly string source;
        public string Source => source;

        private int index;

        private List<Token> Tokens;

        public Lexer(string _source)
        {
            source = _source;
        }

        private char? LookAhead()
        {
            if (index + 1 >= source.Length)
                return null;

            return source[index + 1];
        }

        private void CloneToken(Token token) => Tokens.Add(token.Clone(index));

        private void AddAssignment(BinaryOperator binaryOperator) => Tokens.Add(binaryOperator.CreateAssignment(index));

        internal List<Token> Lex()
        {
            Tokens = new List<Token>();

            for (index = 0; index < source.Length; index++)
            {
                switch(source[index])
                {
                    case var x when char.IsWhiteSpace(x):
                        break;

                    //Operators
                    case '=':
                        var next = LookAhead();

                        if (next == '=')
                        {
                            CloneToken(BinaryOperator.Equals);
                            index++;
                        }
                        else if (next == '>')
                        {
                            CloneToken(Seperator.Lambda);
                            index++;
                        }
                        else
                            CloneToken(BinaryOperator.Assignment);
                        break;
                    case '!':
                        next = LookAhead();

                        if (next != '=')
                            CloneToken(UnaryOperator.Not);
                        else
                        {
                            CloneToken(BinaryOperator.NotEquals);
                            index++;
                        }
                        break;

                    case '>':
                        next = LookAhead();

                        if (next == '>')
                        {
                            index++;

                            if (LookAhead() == '=')
                            {
                                AddAssignment(BinaryOperator.RightShift);
                                index++;
                            }
                            else
                            {
                                CloneToken(BinaryOperator.GreaterThan);
                                CloneToken(BinaryOperator.GreaterThan);
                            }
                        }
                        else if (next == '=')
                        {
                            CloneToken(BinaryOperator.GreaterThanEquals);
                            index++;
                        }
                        else
                            CloneToken(BinaryOperator.GreaterThan);
                        break;
                    case '<':
                        next = LookAhead();

                        if (next == '<')
                        {
                            index++;

                            if (LookAhead() == '=')
                            {
                                AddAssignment(BinaryOperator.LeftShift);
                                index++;
                            }
                            else
                                CloneToken(BinaryOperator.LeftShift);
                        }
                        else if (next == '=')
                        {
                            CloneToken(BinaryOperator.LesserThanEquals);
                            index++;
                        }
                        else
                            CloneToken(BinaryOperator.LesserThan);
                        break;

                    case '&':
                        next = LookAhead();
                        if (next == '&')
                        {
                            CloneToken(BinaryOperator.And);
                            index++;
                        }
                        else if (next == '=')
                        {
                            AddAssignment(BinaryOperator.BitwiseAnd);
                            index++;
                        }
                        else
                            CloneToken(BinaryOperator.BitwiseAnd);
                        break;
                    case '|':
                        next = LookAhead();

                        if(next == '|')
                        {
                            CloneToken(BinaryOperator.Or);
                            index++;
                        }
                        else if(next == '=')
                        {
                            AddAssignment(BinaryOperator.BitwiseOr);
                            index++; 
                        }
                        else
                            CloneToken(BinaryOperator.BitwiseOr);
                        break;
                    case '^':
                        if (LookAhead() == '=')
                        {
                            AddAssignment(BinaryOperator.BitwiseXor);
                            index++;
                        }
                        else
                            CloneToken(BinaryOperator.BitwiseXor);
                        break;
                    case '~':
                        CloneToken(UnaryOperator.BitwiseNot);
                        break;
                    case '+':
                        next = LookAhead();

                        if (next == '+')
                        {
                            CloneToken(UnaryOperator.Increment);
                            index++;
                        }
                        else if (next == '=')
                        {
                            AddAssignment(BinaryOperator.Addition);
                            index++;
                        }
                        else
                            CloneToken(BinaryOperator.Addition);
                        break;
                    case '-':
                        next = LookAhead();

                        if (next == '-')
                        {
                            CloneToken(UnaryOperator.Decrement);
                            index++;
                        }
                        else if (next == '=')
                        {
                            AddAssignment(BinaryOperator.Subtraction);
                            index++;
                        }
                        else
                        {
                            var prev = Tokens.Count == 0 ? null : Tokens[Tokens.Count - 1];
                            
                            if (next.HasValue && char.IsNumber(next.Value))
                            {
                                if (prev == null)
                                {
                                    Tokens.Add(ReadNumber());
                                    break;
                                }

                                switch (prev.Type)
                                {
                                    case TokenType.Constant:
                                    case TokenType.Identifier:
                                        CloneToken(BinaryOperator.Subtraction);
                                        break;
                                    case TokenType.Seperator:
                                        if (prev == Seperator.CloseBracket || prev == Seperator.BoxCloseBracket)
                                            CloneToken(BinaryOperator.Subtraction);
                                        else
                                            Tokens.Add(ReadNumber());
                                        break;
                                    default:
                                        Tokens.Add(ReadNumber());
                                        break;
                                }
                            }
                            else
                            {
                                if (prev == null)
                                {
                                    CloneToken(UnaryOperator.Minus);
                                    break;
                                }

                                switch (prev.Type)
                                {
                                    case TokenType.Constant:
                                    case TokenType.Identifier:
                                        CloneToken(BinaryOperator.Subtraction);
                                        break;
                                    case TokenType.Seperator:
                                        if (prev == Seperator.CloseBracket || prev == Seperator.BoxCloseBracket)
                                            CloneToken(BinaryOperator.Subtraction);
                                        else
                                            CloneToken(UnaryOperator.Minus);
                                        break;
                                    default:
                                        CloneToken(UnaryOperator.Minus);
                                        break;
                                }
                            }
                        }
                        break;
                    case '*':
                        if (LookAhead() == '=')
                            AddAssignment(BinaryOperator.Multiplication);
                        else
                            CloneToken(BinaryOperator.Multiplication);
                        break;
                        
                    case '/':
                        next = LookAhead();
                        if (next == '=')
                            AddAssignment(BinaryOperator.Division);
                        else if (next == '/')
                            ReadSingleLineComment();
                        else if (next == '*')
                            ReadMultiLineComment();
                        else
                            CloneToken(BinaryOperator.Division);
                        break;
                    case '%':
                        if (LookAhead() == '=')
                            AddAssignment(BinaryOperator.Modulus);
                        else
                            CloneToken(BinaryOperator.Modulus);
                        break;

                    //Seperators
                    case ':':
                        CloneToken(Seperator.Colon);
                        break;
                    case ';':
                        CloneToken(Seperator.Semicolon);
                        break;
                    case '.':
                        CloneToken(Seperator.Dot);
                        break;
                    case ',':
                        CloneToken(Seperator.Comma);
                        break;
                    case '(':
                        CloneToken(Seperator.OpenBracket);
                        break;
                    case ')':
                        CloneToken(Seperator.CloseBracket);
                        break;
                    case '[':
                        CloneToken(Seperator.BoxOpenBracket);
                        break;
                    case ']':
                        CloneToken(Seperator.BoxCloseBracket);
                        break;
                    case '{':
                        CloneToken(Seperator.FlowerOpenBracket);
                        break;
                    case '}':
                        CloneToken(Seperator.FlowerCloseBracket);
                        break;
                    case '?':
                        CloneToken(Seperator.Questionmark);
                        break;

                    //Constants
                    case '"':
                        Tokens.Add(ReadString());
                        break;
                    case '\'':
                        Tokens.Add(ReadCharacter());
                        break;

                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        Tokens.Add(ReadNumber());
                        break;

                    default:
                        Tokens.Add(ReadEntity());
                        break;
                }
            }

            return Tokens;
        }

        private void ReadSingleLineComment()
        {
            while(source[index] != '\n')
                index++;
        }

        private void ReadMultiLineComment()
        {
            index += 2;
            int till = source.Length - 1;

            while (index < till)
            {
                if (source[index] == '*')
                    if (LookAhead() == '/')
                    {
                        index++;
                        break;
                    }

                index++;
            }
        }

        private Token ReadString()
        {
            index++;
            StringBuilder value = new StringBuilder();

            while(index < source.Length)
            {
                var current = source[index];

                switch(current)
                {
                    case '\\':
                        value.Append('\\');

                        var next = LookAhead();
                        value.Append(next == null ? string.Empty : next.ToString());
                        index += 2;
                        continue;
                    case '"':
                        return new StringConstant(value.ToString(), index);
                    default:
                        value.Append(current);
                        break;
                }

                index++;
            }

            throw new InvalidCharacterException(source[source.Length - 1], '"', index);
        }

        private Token ReadCharacter()
        {
            index++;
            if (source[index] == '\\')
                index++;

            var token = new CharacterConstant(source[index].ToString(), index);

            index++;
            if (source[index] != '\'')
                throw new InvalidCharacterException(source[index], '\'', index);

            return token;
        }

        private Token ReadNumber()
        {
            StringBuilder value = new StringBuilder();

            bool isReal = false;

            while (index < source.Length)
            {
                var current = source[index];

                switch (current)
                {
                    case '-':
                        if (value.Length > 0)
                        {
                            index--;
                            break;
                        }
                        else
                            value.Append('-');
                        break;
                    case 'l':
                        value.Append('l');
                        return Extract();
                    case 'u':
                        value.Append('u');
                        if (LookAhead() != 'l')
                            return Extract();
                        break;
                    case 'f':
                        value.Append('f');
                        return Extract();
                    case 'd':
                        value.Append('d');
                        return Extract();
                    case 'm':
                        value.Append('m');
                        return Extract();
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        value.Append(current);
                        break;
                    case '.':
                        var next = LookAhead();
                        if (next == null)
                        {
                            index--;
                            return Extract();
                        }
                        else if (char.IsNumber(next.Value))
                        {
                            if (isReal)
                            {
                                index--;
                                return Extract();
                            }

                            value.Append(current);
                            isReal = true;
                            break;
                        }

                        index--;
                        return Extract();
                    default:
                        index--;
                        return Extract();
                }

                index++;
            }

            return Extract();
            Token Extract()
            {
                var constant = value.ToString();
                if (isReal)
                    switch (constant[constant.Length - 1])
                    {
                        case 'd':
                            return new DoubleConstant(constant, index);
                        case 'm':
                            return new DecimalConstant(constant, index);
                        default:
                            return new FloatConstant(constant, index);
                    }
                else
                    switch (constant[constant.Length - 1])
                    {
                        case 'u':
                            return new UIntegerConstant(constant, index);
                        case 'l':
                            if (constant[constant.Length - 2] == 'u')
                                return new ULongConstant(constant, index);
                            else
                                return new LongConstant(constant, index);
                        default:
                            return new IntegerConstant(constant, index);
                    }
            }
        }

        private Token ReadEntity()
        {
            StringBuilder value = new StringBuilder();

            while (index < source.Length)
            {
                var current = source[index];

                if (char.IsWhiteSpace(current))
                    return Extract();

                switch(current)
                {
                    case '=':
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                    case ';':
                    case ':':
                    case '.':
                    case ',':
                    case '(':
                    case ')':
                    case '[':
                    case ']':
                    case '{':
                    case '}':
                    case '<':
                    case '>':
                    case '&':
                    case '|':
                    case '!':
                    case '^':
                    case '~':
                    case '?':
                    case '%':
                        index--;
                        return Extract();

                    case '"':
                    case '\'':
                        throw new InvalidCharacterException(current, index);

                    default:
                        value.Append(current);
                        break;
                }

                index++;
            }

            return Extract();
            Token Extract()
            {
                var valueToString = value.ToString();

                var token = FindKeyWord(valueToString);
                return token == null ? new Identifier(valueToString, index) : token;
            }
        }

        private Token FindKeyWord(string _value)
        {
            foreach(var keyword in Keyword.Keywords)
                if(keyword.Value == _value)
                    return keyword.Clone(index);

            if (BinaryOperator.AsCastOperator.Value == _value)
                return BinaryOperator.AsCastOperator.Clone(index);
            else if (BoolConstant.True.Value == _value)
                return BoolConstant.True.Clone(index);
            else if (BoolConstant.False.Value == _value)
                return BoolConstant.False.Clone(index);
            else if (NullConstant.Null.Value == _value)
                return NullConstant.Null.Clone(index);

            return null;
        }
    }
}
