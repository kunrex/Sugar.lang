using System;
using System.Collections;
using System.Collections.Generic;

using Sugar.Language.Services;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes;

using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions.Structure;

namespace Sugar.Language.Analysis.ProjectStructure.CreationNodes.Properties
{
    internal abstract class BaseIndexerNode : BasePropertyNode, IIndexer
    {
        public abstract GlobalMemberEnum PropertyType { get; }
        public override GlobalMemberEnum GlobalMember { get => GlobalMemberEnum.Indexer; }

        private readonly List<FunctionArgument> arguments;
        public FunctionArgument this[int index] { get => arguments[index]; }

        public int Length { get => arguments.Count; }

        public BaseIndexerNode(Describer _describer, DataType _type) : base(null, _describer, _type)
        {
            arguments = new List<FunctionArgument>();
        }

        public IIndexer AddArgument(FunctionArgument argument)
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
            return new GenericEnumeratorService<BaseIndexerNode, FunctionArgument>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
