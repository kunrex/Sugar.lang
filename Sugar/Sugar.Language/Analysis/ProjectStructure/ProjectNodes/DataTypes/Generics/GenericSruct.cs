using System;

using Sugar.Language.Parsing.Nodes.DataTypes;

namespace Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Generics
{
    internal sealed class GenericStruct : GenericInstance<Struct, StructNode>
    {
        public GenericStruct(Struct _class) : base(_class)
        {

        }
    }
}
