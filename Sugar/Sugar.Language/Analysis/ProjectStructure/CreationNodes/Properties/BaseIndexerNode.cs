using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Sugar.Language.Services;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes;

using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Functions.Structure;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

namespace Sugar.Language.Analysis.ProjectStructure.CreationNodes.Properties
{
    internal abstract class BaseIndexerNode : BasePropertyNode, IIndexer
    {
        protected readonly FunctionParamatersNode parseArguments;
        public FunctionParamatersNode ParseArguments { get => parseArguments; }
        
        public abstract GlobalMemberEnum PropertyType { get; }
        public override GlobalMemberEnum GlobalMember { get => GlobalMemberEnum.Indexer; }

        private readonly Dictionary<string, FunctionParameter> arguments;
        public FunctionParameter this[int index] { get => arguments.ElementAt(index).Value; }

        public int Length { get => arguments.Count; }

        public BaseIndexerNode(Describer _describer, DataType _type, FunctionParamatersNode _arguments) : base(null, _describer, _type)
        {
            parseArguments = _arguments;
            arguments = new Dictionary<string, FunctionParameter>();
        }

        public IIndexer AddArgument(FunctionParameter _parameter)
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
            return new GenericEnumeratorService<BaseIndexerNode, FunctionParameter>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
