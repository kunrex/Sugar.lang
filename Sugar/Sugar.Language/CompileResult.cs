using System;
using System.Collections.Generic;

using Sugar.Language.Exceptions;

namespace Sugar.Language
{
    internal abstract class CompileResult
    {
        public abstract IReadOnlyList<CompileException> Exceptions { get; }
    }

    internal sealed class CompileResult<Result> : CompileResult
    {
        private bool built;
        public bool Built { get => built; }

        public bool Completed
        {
            get
            {
                if (!built)
                    return false;
                else
                    return Exceptions.Count == 0;
            }
        }

        private readonly List<CompileException> exceptions;
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
    }
}
