using System;

using Sugar.Language.Semantics.Analysis;

namespace Sugar.Language.Semantics.Services
{
    internal interface ISemanticService<T> where T :ISemanticService<T>
    {
        public SemanticsResult Validate();
    }
}
