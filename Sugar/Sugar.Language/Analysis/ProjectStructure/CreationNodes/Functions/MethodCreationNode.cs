using System;
using System.Collections;
using System.Collections.Generic;

using Sugar.Language.Services;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions.Structure;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Functions;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes.Functions;

namespace Sugar.Language.Analysis.ProjectStructure.CreationNodes.Functions
{
    internal abstract class MethodCreationNode<Parent> : VoidCreationNode<Parent>, IMethod where Parent : IFunctionParent
    {
        protected readonly DataType creationType; 
        public DataType CreationType { get => creationType; }

        public MethodCreationNode(string _name, Describer _describer, ParseNodeCollection _body, DataType _type) : base(_name, _describer, _body)
        {
            creationType = _type;
        }

        public override IEnumerator<FunctionArgument> GetEnumerator()
        {
            return new GenericEnumeratorService<MethodCreationNode<Parent>, FunctionArgument>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
