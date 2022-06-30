using System;
using Sugar.Language.Semantics.Analysis;

namespace Sugar.Language.Semantics.Services
{
    internal interface IValidatableService<T> : ISemanticService where T : IValidatableService<T>
    {
        public SemanticsResult Validate();
    }
}
