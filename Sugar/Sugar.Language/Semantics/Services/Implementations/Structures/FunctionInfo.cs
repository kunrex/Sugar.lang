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
        public bool IsReturnableFunction { get => returnType != null; }

        private readonly Describer describer;
        public Describer Describer { get => describer; }

        private readonly FunctionDeclArgs arguments;
        public FunctionDeclArgs Arguments { get => arguments; }

        public FunctionInfo(Node _body, DataType _returnType, Describer _describer, FunctionDeclArgs _arguments)
        {
            body = _body;
            describer = _describer;

            arguments = _arguments;
            returnType = _returnType;
        }
    }
}
