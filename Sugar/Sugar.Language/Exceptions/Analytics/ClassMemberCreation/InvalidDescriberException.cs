using System;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Enums;

namespace Sugar.Language.Exceptions.Analytics.ClassMemberCreation
{
    internal sealed class InvalidDescriberException : CompileException
    {
        public InvalidDescriberException(string variableName, DescriberEnum allowed, Describer error) : base($"The describer is not valid on this item. Item: {variableName}, Allowed: {allowed}, Current: {error}", 0)
        {

        }
    }
}
