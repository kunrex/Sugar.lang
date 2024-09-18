using System;
using System.Collections.Generic;

using Sugar.Language.Services;

using Sugar.Language.Exceptions;
using Sugar.Language.Exceptions.Parsing;

using Sugar.Language.Tokens;
using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Parsing.Parser
{
    internal sealed partial class Parser : SingletonService<Parser>
    {
        private int index;

        private SugarFile sourceFile;
        private Token Current
        {
            get
            {
                if (index >= sourceFile.TokenCount)
                    return Final;

                return sourceFile[index];
            }
        }
        private Token Final { get => sourceFile[sourceFile.TokenCount - 1]; }

        /// <summary>
        /// Returns the next Token. Returns `null` if none exists.
        /// </summary>
        private Token LookAhead()
        {
            if (index + 1 >= sourceFile.TokenCount)
                return null;

            return sourceFile[index + 1];
        }


        /// <summary>
        /// Throws an exception when the `current token` is not an expected one.
        /// </summary>
        private void ForceMatchCurrentType(TokenType expected, bool increment = false)
        {
            if (index >= sourceFile.TokenCount)
                throw new TokenExpectedException(expected, Final);

            if (!MatchType(expected, Current, increment))
                throw new TokenExpectedException(expected, Current);
        }

        /// <summary>
        /// Pushes an exception when the `current token` is not an expected one. Returns weather or not the expected token was found.
        /// </summary>
        private bool TryMatchCurrentType(TokenType expected, bool increment = false)
        {
            if (index >= sourceFile.TokenCount)
            {
                sourceFile.PushException(new TokenExpectedException(expected, Final));
                return false;
            }

            if (!MatchCurrentType(expected, increment))
            {
                sourceFile.PushException(new TokenExpectedException(expected, Current));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns weather or not the curent token is an expected token.
        /// </summary>
        private bool MatchCurrentType(TokenType expected, bool increment = false)
        {
            if (index >= sourceFile.TokenCount)
                return false;

            return MatchType(expected, Current, increment);
        }

        /// <summary>
        /// Matches a token to a certain expected type.
        /// </summary>
        private bool MatchType(TokenType expected, Token toMatch, bool increment = false)
        {
            if (toMatch != null && (expected & toMatch.Type) == toMatch.Type)
            {
                if (increment)
                    index++;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Throws an exception if the `current token` does not match a certain token.
        /// </summary>
        private void ForceMatchCurrent(Token matchTo, bool increment = false)
        {
            if (index >= sourceFile.TokenCount)
                throw new TokenExpectedException(matchTo, Final);

            if (!MatchCurrent(matchTo, increment))
                throw new TokenExpectedException(matchTo, Current);
        }

        /// <summary>
        /// Pushes an exception if the `current token` does not match a certain token. Returns weather or not it was a match.
        /// </summary>
        private bool TryMatchCurrent(Token matchTo, bool increment = false)
        {
            if (index >= sourceFile.TokenCount)
            {
                sourceFile.PushException(new TokenExpectedException(matchTo, Final));
                return false;
            }

            if (!MatchCurrent(matchTo, increment))
            {
                sourceFile.PushException(new TokenExpectedException(matchTo, Current));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns weather or not the `current token` is a certain token.
        /// </summary>
        private bool MatchCurrent(Token matchTo, bool increment = false)
        {
            if (index >= sourceFile.TokenCount)
                return false;

            return MatchToken(matchTo, Current, increment);
        }

        /// <summary>
        /// Pushes an exception if a certain token
        /// (<paramref name="matchTo"/>)
        /// does not match a certain base token
        /// (<paramref name="toMatch"/>). Returns weather or not it was a match.
        /// </summary>
        private bool TryMatchToken(Token matchTo, Token toMatch, bool increment = false)
        {
            if (index >= sourceFile.TokenCount)
            {
                sourceFile.PushException(new TokenExpectedException(matchTo, Final));
                return false;
            }

            if (!MatchToken(matchTo, toMatch, increment))
            {
                sourceFile.PushException(new TokenExpectedException(matchTo, toMatch));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns if a certain token
        /// (<paramref name="matchTo"/>)
        /// does match a certain base token
        /// (<paramref name="toMatch"/>).
        /// </summary>
        private bool MatchToken(Token matchTo, Token toMatch, bool increment = false)
        {
            if (toMatch == matchTo)
            {
                if (increment)
                    index++;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Pushes an exception if the `current seperator` matches a certain expected seperator. Returns weather or not the match was found.
        /// </summary>
        private bool TryMatchBreakOutSeperator(SeperatorKind breakOutSeperators)
        {
            if (index >= sourceFile.TokenCount)
            {
                sourceFile.PushException(new TokenExpectedException($"{breakOutSeperators}", Final));
                return false;
            }

            if (!MatchBreakOutSeperator(Current, breakOutSeperators))
            {
                sourceFile.PushException(new TokenExpectedException($"{breakOutSeperators}", Current));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns weather or a certain token matches a certain seperator was found.
        /// </summary>
        private bool MatchBreakOutSeperator(Token seperator, SeperatorKind breakOutSeperators)
        {
            if (!MatchType(TokenType.Seperator, seperator))
                return false;

            var subType = (SeperatorKind)seperator.SyntaxKind;
            return (subType & breakOutSeperators) == subType;
        }
        
        //Error Handling
        private InvalidTokenException PushInvalidCurrentException() => PushInvalidTokenException(Current);
        private InvalidTokenException PushInvalidTokenException(Token token)
        {
            var exception = new InvalidTokenException(token);
            sourceFile.PushException(exception);

            return exception;
        }

        private InvalidStatementException PushInvalidStatementException()
        {
            var exception = new InvalidStatementException(index);
            sourceFile.PushException(exception);

            return exception;
        }

        private TokenExpectedException PushExpectedCurrentException(TokenType expected) => PushExpectedTokenException(expected, Current);
        private TokenExpectedException PushExpectedTokenException(TokenType expected, Token token) => PushException(new TokenExpectedException(expected, token));

        private TokenExpectedException PushExpectedCurrentException(Token expected) => PushExpectedTokenException(expected, Current);
        private TokenExpectedException PushExpectedTokenException(Token expected, Token token) => PushException(new TokenExpectedException(expected, token));

        private TokenExpectedException PushExpectedCurrentException(string expected) => PushExpectedTokenException(expected, Current);
        private TokenExpectedException PushExpectedTokenException(string expected, Token token) => PushException(new TokenExpectedException(expected, token));

        private Exc PushException<Exc>(Exc exception) where Exc : CompileException
        {
            sourceFile.PushException(exception);

            return exception;
        }

        /// <summary>
        /// Returns a TokenCollection containing all Tokens until
        /// <para>1. Expected <paramref name="breakOutSeperators"/> is found (inclusive)</para>
        /// 2. Default Seperator: ; { } is found (exclusive, overrides <paramref name="breakOutSeperators"/>)
        /// </summary>
        private TokenCollection PanicModeSimple(SeperatorKind breakOutSeperators)
        {
            var invalid = new List<Token>();
            var extra = SeperatorKind.Semicolon | SeperatorKind.FlowerOpenBracket | SeperatorKind.FlowerCloseBracket;
            var match = breakOutSeperators | extra;

            while (index < sourceFile.TokenCount)
            {
                var current = Current;

                if (MatchBreakOutSeperator(current, match))
                {
                    if (MatchBreakOutSeperator(current, extra) && !MatchBreakOutSeperator(current, breakOutSeperators))
                        index--;

                    break;
                }

                invalid.Add(current);
                index++;
            }

            return new TokenCollection(invalid);
        }
    }
}