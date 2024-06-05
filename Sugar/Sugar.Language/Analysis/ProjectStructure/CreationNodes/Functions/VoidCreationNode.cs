using System;
using System.Collections;
using System.Collections.Generic;

using Sugar.Language.Services;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Analysis.ProjectStructure.LocalNodes;

using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions.Structure;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Functions;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes.Functions;

namespace Sugar.Language.Analysis.ProjectStructure.CreationNodes.Functions
{
    internal abstract class VoidCreationNode<Parent> : CreationNode, IFunction, IBody where Parent : IFunctionParent 
    {
        protected readonly ParseNodeCollection body;
        public ParseNodeCollection Body { get => body; }

        private readonly List<FunctionArgument> arguments;
        public FunctionArgument this[int index] { get => arguments[index]; }

        public int Length { get => arguments.Count; }

        private readonly Scope scope;
        public Scope Scope { get => scope; }

        public VoidCreationNode(string _name, Describer _describer, ParseNodeCollection _body) : base(_name, _describer)
        {
            body = _body;

            scope = new Scope();
            scope.SetParent(this);

            arguments = new List<FunctionArgument>();
        }

        public IFunction AddArgument(FunctionArgument argument)
        {
            if (FindArgument(argument.Name) != null)
                arguments.Add(argument);

            return this;
        }

        public FunctionArgument FindArgument(IdentifierNode identifier) => FindArgument(identifier.Value);

        private FunctionArgument FindArgument(string identifier)
        {
            foreach (var arg in arguments)
                if (arg.Name == identifier)
                    return arg;

            return null;
        }

        public virtual IEnumerator<FunctionArgument> GetEnumerator()
        {
            return new GenericEnumeratorService<VoidCreationNode<Parent>, FunctionArgument>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
