using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.UDDataTypes;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.CreationStatements;

namespace Sugar.Language.Semantics.ActionTrees.DataTypes
{
    internal abstract class DataTypeWrapper<SkeletonNode> : DataType where SkeletonNode : UDDataTypeNode
    {
        public SkeletonNode Skeleton { get; protected set; }

        public DataTypeWrapper(IdentifierNode _name, List<ImportNode> _imports, GlobalMemberEnum _allowed, SkeletonNode _skeleton) : base(_name, _imports, _allowed)
        {
            Skeleton = _skeleton;
        }

        public virtual bool IsDuplicateGlobalMember(IdentifierNode identifier) => false;

        protected T AddGlobalMember<T>(GlobalMemberEnum memberEnum, ICreationStatement statement) where T : DataTypeWrapper<SkeletonNode>
        {
            globalMemberCollection.Add(memberEnum, statement);

            return (T)this;
        }
    }
}
