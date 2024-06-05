using System;

namespace Sugar.Language.Exceptions.Analysis.Import
{
    internal sealed class AmbigiousReferenceException : CompileException
    {
        public AmbigiousReferenceException() : base("Ambigious reference exception, try to be more specific.", 0)
        {

        }
    }
}
