using System;
using System.Collections.Generic;
using System.Text;
using Sugar.Language.Exceptions;

namespace Sugar.Language.Semantics.Analysis
{
    internal sealed class SemanticsResult
    {
        private bool built;
        private readonly string stage;

        public bool Completed { get; private set; }

        private readonly List<CompileException> exceptions;
        public IReadOnlyList<CompileException> Exceptions { get => exceptions; }

        public SemanticsResult(string _stage)
        {
            built = false;
            stage = _stage;
            exceptions = new List<CompileException>();
        }

        public void Add(CompileException exception) => exceptions.Add(exception);

        public SemanticsResult Build()
        {
            built = true;

            Completed = exceptions.Count == 0;
            return this;
        }

        public override string ToString()
        {
            if (!built)
                return null;

            var builder = new StringBuilder($"Stage: {stage}, Completed: {Completed}");

            foreach (var exception in exceptions)
                builder.Append($"\nException: {exception}");

            return builder.ToString();
        }
    }
}
