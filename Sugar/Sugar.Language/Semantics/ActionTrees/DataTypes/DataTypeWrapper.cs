using System;
using System.Linq;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.UDDataTypes;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Interfaces;
using Sugar.Language.Semantics.ActionTrees.CreationStatements;

namespace Sugar.Language.Semantics.ActionTrees.DataTypes
{
    internal abstract class DataTypeWrapper<SkeletonNode> : DataType where SkeletonNode : UDDataTypeNode
    {
        public SkeletonNode Skeleton { get; protected set; }

        public DataTypeWrapper(IdentifierNode _name, List<ImportNode> _imports, MemberEnum _allowed, SkeletonNode _skeleton) : base(_name, _imports, _allowed)
        {
            Skeleton = _skeleton;
        }

        public abstract bool IsDuplicate(IdentifierNode identifier);

        protected DataTypeSkeleton AddGlobalMember<DataTypeSkeleton, ParentType>(MemberEnum memberEnum, IParentableCreationStatement<ParentType> statement, ParentType parent) where DataTypeSkeleton : DataTypeWrapper<SkeletonNode> where ParentType : IActionTreeNode
        {
            globalMemberCollection.Add(memberEnum, statement);
            statement.SetParent(parent);

            return (DataTypeSkeleton)this;
        }

        public IEnumerable<ICreationStatement> GetAllMembers(MemberEnum memberEnum) => globalMemberCollection[memberEnum];
    }
}
