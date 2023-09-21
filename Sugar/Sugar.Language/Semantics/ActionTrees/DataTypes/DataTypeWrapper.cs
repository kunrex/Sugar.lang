using System;
using System.Linq;
using System.Collections.Generic;

using Sugar.Language.Tokens.Operators;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.UDDataTypes;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Interfaces;
using Sugar.Language.Semantics.ActionTrees.CreationStatements;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global.Conversion;

namespace Sugar.Language.Semantics.ActionTrees.DataTypes
{
    internal abstract class DataTypeWrapper<SkeletonNode> : DataType where SkeletonNode : UDDataTypeNode
    {
        public SkeletonNode Skeleton { get; protected set; }

        public DataTypeWrapper(IdentifierNode _name, List<ImportNode> _imports, MemberEnum _allowed, SkeletonNode _skeleton) : base(_name, _imports, _allowed)
        {
            Skeleton = _skeleton;
        }

        protected DataTypeSkeleton AddGlobalMember<DataTypeSkeleton, ParentType>(MemberEnum memberEnum, IParentableCreationStatement<ParentType> statement, ParentType parent) where DataTypeSkeleton : DataTypeWrapper<SkeletonNode> where ParentType : IActionTreeNode
        {
            globalMemberCollection.Add(memberEnum, statement);
            statement.SetParent(parent);

            return (DataTypeSkeleton)this;
        }

        protected ReturnType TryFindIdentifiedMember<ReturnType, Parent>(IdentifierNode identifier, MemberEnum memberType) where ReturnType : CreationStatement<Parent> where Parent : IActionTreeNode
        {
            var members = globalMemberCollection[memberType];

            var name = identifier.Value;
            foreach (var member in members)
                if (member.Name == name)
                    return (ReturnType)member;

            return null;
        }

        protected ReturnType TryFindIdentifiedArgumentedMember<ReturnType, Parent>(IdentifierNode identifier, IFunctionArguments arguments, MemberEnum memberType) where ReturnType : CreationStatement<Parent>, IFunction where Parent : IActionTreeNode
        {
            var members = globalMemberCollection[memberType];

            var name = identifier.Value;
            foreach (var function in members)
                if (function.Name == name)
                {
                    var converted = (ReturnType)function;

                    if (CheckArguments(converted, arguments))
                        return converted;
                }

            return null;
        }

        protected IndexerCreationStmt TryFindIndexer(DataType external, IFunctionArguments arguments)
        {
            var indexers = globalMemberCollection[MemberEnum.Indexer];

            foreach (var indexer in indexers)
            {
                var converted = (IndexerCreationStmt)indexer;

                if (!converted.CreationType.StrictCompareTo(external))
                    continue;
                if (CheckArguments(converted, arguments))
                    return converted;
            }

            return null;
        }

        private bool CheckArguments<GlobalMemberType>(GlobalMemberType converted, IFunctionArguments arguments) where GlobalMemberType : IFunction
        {
            var found = true;

            for (int i = 0; i < converted.FunctionArguments.Count; i++)
            {
                if (!arguments[i].CompareTo(converted.FunctionArguments[i]))
                {
                    found = false;
                    break;
                }
            }

            return found;
        }

        protected ReturnType TryFindCast<ReturnType, Parent>(DataType external, MemberEnum memberType) where ReturnType : CastDeclarationStmt<Parent> where Parent : IActionTreeNode
        {
            var conversions = globalMemberCollection[memberType];

            foreach (var conversion in conversions)
            {
                var converted = (ReturnType)conversion;

                if (converted.From == this)
                    if (converted.To == external)
                        return converted;
                    else if (converted.From == external)
                        if (converted.To == this)
                            return converted;
            }

            return null;
        }

        protected OperatorOverloadDeclarationStmt TryFindOperatorOverload(Operator op, DataType operhand1)
        {
            var operators = globalMemberCollection[MemberEnum.OperaterOverload];

            var name = op.OperatorType.ToString();
            foreach (var operatrOverload in operators)
                if (operatrOverload.Name == name)
                {
                    var converted = (OperatorOverloadDeclarationStmt)operatrOverload;

                    if (converted.FunctionArguments[0].StrictCompareTo(operhand1))
                        return converted;
                }

            return null;
        }

        protected OperatorOverloadDeclarationStmt TryFindOperatorOverload(Operator op, DataType operhand1, DataType operhand2)
        {
            var operators = globalMemberCollection[MemberEnum.OperaterOverload];

            var name = op.OperatorType.ToString();
            foreach (var operatrOverload in operators)
                if (operatrOverload.Name == name)
                {
                    var converted = (OperatorOverloadDeclarationStmt)operatrOverload;

                    var arg1 = converted.FunctionArguments[0];
                    var arg2 = converted.FunctionArguments[1];

                    Console.WriteLine(arg1 == null);

                    if (arg1 == this)
                    {
                        if (arg2.StrictCompareTo(operhand1))
                            return converted;
                    }
                    else
                    {
                        if (arg1.StrictCompareTo(operhand2))
                            return converted;
                    }
                }

            return null;
        }
    }
}
