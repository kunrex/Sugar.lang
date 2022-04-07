using System;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Describers;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.Describers
{
    internal interface IDescribable
    {
        public Describer Describer { get; }
        public DescriberEnum Allowed { get; }

        public bool ValidateDescriber();
    }
}
