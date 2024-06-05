using System;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes.Functions;

namespace Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions.Extensions
{
    internal sealed class ExtensionVoidNode : GlobalVoidNode, IExtension
    {
        private readonly DataType extensionParent;
        public DataType ExtensionParent { get => extensionParent; }

        public ExtensionVoidNode(string _name, Describer _describer, ParseNodeCollection _body, DataType _parent) : base(_name, _describer, _body)
        {
            extensionParent = _parent;
        }
    }
}
