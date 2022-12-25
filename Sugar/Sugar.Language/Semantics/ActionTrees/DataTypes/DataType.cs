using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Statements;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Namespaces;
using Sugar.Language.Semantics.ActionTrees.DataTypes.Structure;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Collections;

namespace Sugar.Language.Semantics.ActionTrees.DataTypes
{
    internal abstract class DataType : ActionTreeNode<IDataTypeCollection>, IDataTypeCollection, INameable
    {
        public override ActionNodeEnum ActionNodeType { get; }

        protected readonly IdentifierNode name;
        public string Name { get => name.Value; }
        public IdentifierNode Identifier { get => name; }

        protected readonly List<DataType> subTypes;
        public int DataTypeCount { get => subTypes.Count; }

        protected readonly List<ImportNode> referencedImports;
        public IEnumerable<ImportNode> ReferencedImports
        {
            get
            {
                foreach (var nameSpace in referencedImports)
                    yield return nameSpace;
            }
        }

        protected readonly List<DataType> referencedTypes;
        public IEnumerable<DataType> ReferencedTypes
        {
            get
            {
                foreach (var type in referencedTypes)
                    yield return type;
            }
        }

        protected readonly List<CreatedNameSpaceNode> referencedNameSpaces;
        public IEnumerable<CreatedNameSpaceNode> ReferncedNameSpaces
        {
            get
            {
                foreach (var nameSpace in referencedNameSpaces)
                    yield return nameSpace;
            }
        }

        protected readonly MemberEnum allowedGlobalMemberDeclarations;
        public MemberEnum AllowedGlobalMemberDeclarations { get => allowedGlobalMemberDeclarations; }

        protected readonly MemberCollection globalMemberCollection;

        public DataType(IdentifierNode _name, List<ImportNode> _imports, MemberEnum _allowed)
        {
            name = _name;
            allowedGlobalMemberDeclarations = _allowed;

            referencedImports = _imports;
            subTypes = new List<DataType>();
            referencedTypes = new List<DataType>();
            referencedNameSpaces = new List<CreatedNameSpaceNode>();

            globalMemberCollection = new MemberCollection(allowedGlobalMemberDeclarations);
        }

        public DataType GetSubDataType(int index) => subTypes[index];

        public DataType TryFindDataType(IdentifierNode identifier)
        {
            var value = identifier.Value;
            foreach (var type in subTypes)
                if (type.Name == value)
                    return type;

            return null;
        }

        public IDataTypeCollection AddEntity(DataType datatypeToAdd)
        {
            subTypes.Add(datatypeToAdd);
            referencedTypes.Add(datatypeToAdd);

            datatypeToAdd.SetParent(this);

            return this;
        }

        public void ReferenceDataType(DataType dataTypes)
        {
            foreach (var type in referencedTypes)
                if (type == dataTypes)
                    return;

            referencedTypes.Add(dataTypes);
        }

        public void ReferenceNameSpace(CreatedNameSpaceNode createdNameSpaces)
        {
            foreach (var nameSpace in referencedNameSpaces)
                if (nameSpace == createdNameSpaces)
                    return;

            referencedNameSpaces.Add(createdNameSpaces);
        }

        protected override void PrintChildren(string indent)
        {
            for (int i = 0; i < subTypes.Count; i++)
                subTypes[i].Print(indent, i == subTypes.Count - 1);

            if (referencedImports.Count > 0)
            {
                Console.WriteLine("Referenced Data Types");
                for (int i = 0; i < referencedTypes.Count; i++)
                    referencedTypes[i].Print(indent, i == referencedImports.Count - 1);

                Console.WriteLine("Referenced Name Spaces");
                for (int i = 0; i < referencedNameSpaces.Count; i++)
                    referencedNameSpaces[i].Print(indent, i == referencedImports.Count - 1);
            }
            else
                Console.WriteLine(indent + "Imports: None");
        }
    }
}
