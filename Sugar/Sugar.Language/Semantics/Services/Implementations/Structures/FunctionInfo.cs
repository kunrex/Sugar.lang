using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.Services.Implementations.Structures
{
    internal struct FunctionInfo 
    {
        private readonly Node body;
        public Node Body { get => body; }

        private readonly DataType returnType;
        public DataType ReturnType { get => returnType; }

        private readonly Describer describer;
        public Describer Describer { get => describer; }

        private readonly FunctionArguments arguments;
        public FunctionArguments Arguments { get => arguments; }

        public FunctionInfo(Node _body, DataType _returnType, Describer _describer, FunctionArguments _arguments)
        {
            body = _body;
            describer = _describer;

            arguments = _arguments;
            returnType = _returnType;
        }
    }
}
