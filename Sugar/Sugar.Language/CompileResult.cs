using System;
using System.Text;
using System.Collections.Generic;

using Sugar.Language.Exceptions;

namespace Sugar.Language
{
    internal abstract class CompileResult
    {
        public abstract IReadOnlyList<CompileException> Exceptions { get; }
    }

    internal class CompileResult<Result> : CompileResult
    {
        protected bool built;
        public bool Built { get => built; }

        public bool Completed
        {
            get
            {
                if (!built)
                    return false;
                else
                    return exceptions.Count != 0;
            }
        }

        protected readonly List<CompileException> exceptions;
        public override IReadOnlyList<CompileException> Exceptions { get => exceptions; }

        public Result Results { get; private set; }

        public CompileResult()
        {
            built = false;
            exceptions = new List<CompileException>();
        }

        public void Add(CompileException exception) => exceptions.Add(exception);

        public CompileResult<Result> Build(Result buildResults)
        {
            Results = buildResults;
            return Build();
        }

        public CompileResult<Result> Build()
        {
            built = true;

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
