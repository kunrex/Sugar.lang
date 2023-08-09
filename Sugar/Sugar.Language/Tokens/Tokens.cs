using System;
using System.Collections;
using System.Collections.Generic;

using Sugar.Language.Services;
using Sugar.Language.Services.Interfaces;

namespace Sugar.Language.Tokens
{
    internal sealed class TokenCollection : ICustomCollection<Token>
    {
        private readonly List<Token> tokens;

        public int Length { get => tokens.Count;  }

        public Token this[int index] { get => tokens[index]; }

        public TokenCollection(List<Token> _tokens)
        {
            tokens = _tokens;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Token> GetEnumerator()
        {
            return new GenericEnumeratorService<TokenCollection, Token>(this);
        }
    }
}
