using System;

using Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums;

namespace Sugar.Language.Semantics.BuiltInTypes.Numeric
{
    internal abstract class NumericType : SugarType
    {
        public override bool ReferenceType { get => false; }

        protected readonly TypeEnum returnType, subTypes;

        public NumericType(TypeEnum _return, TypeEnum _implicit, TypeEnum _explicit, TypeEnum _subType) : base(_implicit | _return, _explicit)
        {
            returnType = _return;
            subTypes = _subType;
        }
    }
}
