using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Sugar.Language.Exceptions
{
    internal sealed class CompoundCompileException : CompileException, IEnumerable<CompileException>
    {
        private readonly CompileException[] exceptions;

        public bool IsReadOnly { get => true; }
        public int Count { get => exceptions.Length; }

        public CompoundCompileException(params CompileException[] _exceptions) : base($"{_exceptions.Length} exceptiones detected. At index: {_exceptions[0].Index0}", _exceptions[0].Index0)
        {
            exceptions = _exceptions;
        }

        public override string ToString()
        {
            var builder = new StringBuilder(expcetionString + '\n');

            foreach (var exception in exceptions)
                builder.Append(exception);

            return builder.ToString();
        }

        public IEnumerator<CompileException> GetEnumerator() => throw new NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    }
}
