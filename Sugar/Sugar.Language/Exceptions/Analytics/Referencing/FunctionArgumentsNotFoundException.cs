using System;

namespace Sugar.Language.Exceptions.Analytics.Referencing
{
    internal sealed class FunctionArgumentsNotFoundException : CompileException
    {
        public FunctionArgumentsNotFoundException(string name) : base($"Invalid FUnction Arguments for function: {name}", 0)
        {

        }
    }
}
