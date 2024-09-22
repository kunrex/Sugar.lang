using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;
using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes.Functions;

namespace Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions.Extensions
{
    internal sealed class ExtensionMethodNode : GlobalMethodNode, IExtension
    {
        private readonly DataType extensionParent;
        public DataType ExtensionParent { get => extensionParent; }

        public ExtensionMethodNode(string _name, Describer _describer, DataType _type, ParseNodeCollection _body, FunctionParamatersNode _arguments, DataType _parent) : base(_name, _describer, _type, _body, _arguments)
        {
            extensionParent = _parent;
        }
    }
}
