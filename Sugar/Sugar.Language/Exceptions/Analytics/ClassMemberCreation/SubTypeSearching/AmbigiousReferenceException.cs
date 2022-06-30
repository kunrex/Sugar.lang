using System;

namespace Sugar.Language.Exceptions.Analytics.ClassMemberCreation.SubTypeSearching
{
    internal sealed class AmbigiousReferenceException : CompileException
    {
        public AmbigiousReferenceException(string toSearch, string type, int index) : base($"An ambigious reference exists in {type} for the type {toSearch}. Try being more specific", index)
        {

        }
    }
}
