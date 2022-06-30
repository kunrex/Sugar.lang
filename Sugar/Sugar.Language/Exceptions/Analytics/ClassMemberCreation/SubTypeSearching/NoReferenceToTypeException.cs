using System;

namespace Sugar.Language.Exceptions.Analytics.ClassMemberCreation.SubTypeSearching
{
    internal sealed class NoReferenceToTypeException : CompileException
    {
        public NoReferenceToTypeException(string toSearch, string type, int index) : base($"No reference to the data type '{toSearch}' exists in '{type}'. You might be missing an import statement", index) 
        {

        }
    }
}
