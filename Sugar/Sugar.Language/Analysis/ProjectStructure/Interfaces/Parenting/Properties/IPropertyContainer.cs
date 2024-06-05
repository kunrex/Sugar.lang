using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Properties
{
    internal interface IPropertyParent : IParent
    {
        public IPropertyParent AddProperty(IProperty property);

        public IProperty TryFindSetProperty(IdentifierNode identifier);
        public IProperty TryFindGetProperty(IdentifierNode identifier);
    }
}
