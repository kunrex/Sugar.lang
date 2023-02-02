using System;

using Sugar.Language.Parsing.Nodes.Types;

using Sugar.Language.Semantics.Analysis;
using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Namespaces;

using Sugar.Language.Exceptions.Analytics.ClassMemberCreation.SubTypeSearching;

namespace Sugar.Language.Semantics.Services.Implementations.Binding
{
    internal abstract class BinderService<ServiceType> : IValidatableService<ServiceType> where ServiceType : IValidatableService<ServiceType>
    {
        protected abstract string StepName { get; }

        protected readonly DefaultNameSpaceNode defaultNameSpace;
        protected readonly CreatedNameSpaceCollectionNode createdNameSpaces;

        protected readonly SemanticsResult result;

        protected readonly DescriberService describerService;

        public BinderService(DefaultNameSpaceNode _defaultNameSpace, CreatedNameSpaceCollectionNode _createdNameSpaces)
        {
            defaultNameSpace = _defaultNameSpace;
            createdNameSpaces = _createdNameSpaces;

            describerService = new DescriberService();

            result = new SemanticsResult(StepName);
        }

        public abstract SemanticsResult Validate();

        protected DataType FindReferencedType(TypeSearcherService subTypeSearcher, TypeNode type)
        {
            var results = subTypeSearcher.ReferenceDataTypeCollection(type);

            int count = results.Count;
            for (int i = 0; i < count; i++)
            {
                var result = results.Dequeue();

                if (result.Remaining == null && result.ResultEnum != ActionNodeEnum.Namespace)
                    results.Enqueue(result);
            }

            switch (results.Count)
            {
                case 0:
                    result.Add(new NoReferenceToTypeException(type.ToString(), "", 0));
                    return null;
                case 1:
                    return (DataType)results.Dequeue().Result;
                default:
                    result.Add(new AmbigiousReferenceException(type.ToString(), "", 0));
                    return null;
            }
        }
    }
}
