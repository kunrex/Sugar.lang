using System;

using Sugar.Language.Parsing.Nodes.DataTypes;

namespace Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Generics
{
    internal sealed class GenericInterface : GenericInstance<Interface, InterfaceNode>
    {
        public GenericInterface(Interface _class) : base(_class)
        {

        }
    }
}
