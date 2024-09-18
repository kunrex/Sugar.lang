using System;

using Sugar.Language.Parsing.Nodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Nodes;

namespace Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Generics
{
    internal sealed class GenericClass : GenericInstance<Class, ClassNode>
    {
        public GenericClass(Class _class) : base(_class)
        {

        }
    }
}
