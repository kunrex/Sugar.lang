using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sugar.Language.Services;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

using Sugar.Language.Analysis.ProjectStructure.LocalNodes;

using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Functions.Structure;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Functions;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes.Functions;

namespace Sugar.Language.Analysis.ProjectStructure.CreationNodes.Functions
{
    internal abstract class VoidCreationNode<Parent> : CreationNode, IFunction, IBody where Parent : IFunctionParent 
    {
        protected readonly ParseNode body;
        public ParseNode Body { get => body; }

        protected readonly FunctionParamatersNode parseArguments;
        public FunctionParamatersNode ParseArguments { get => parseArguments; }

        private readonly Dictionary<string, FunctionParameter> arguments;
        public FunctionParameter this[int index] { get => arguments.ElementAt(index).Value; }

        public int Length { get => arguments.Count; }

        private readonly Scope scope;
        public Scope Scope { get => scope; }

        public VoidCreationNode(string _name, Describer _describer, ParseNode _body, FunctionParamatersNode _arguments) : base(_name, _describer)
        {
            body = _body;
            parseArguments = _arguments;

            scope = new Scope();
            scope.SetParent(this);

            arguments = new Dictionary<string, FunctionParameter>();
        }

        public IFunction AddArgument(FunctionParameter _parameter)
        {
            if (FindArgument(_parameter.Name) != null)
                arguments.Add(_parameter.Name, _parameter);

            return this;
        }

        public FunctionParameter FindArgument(IdentifierNode identifier) => arguments[identifier.Value];

        private FunctionParameter FindArgument(string identifier)
        {
            foreach (var arg in arguments)
                if (arg.Key == identifier)
                    return arg.Value;

            return null;
        }

        public virtual IEnumerator<FunctionParameter> GetEnumerator()
        {
            return new GenericEnumeratorService<VoidCreationNode<Parent>, FunctionParameter>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
