using System;
using System.Collections.Generic;
using System.Data;
using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Keywords;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Describers;
using Sugar.Language.Parsing.Nodes.Types.Enums;
using Sugar.Language.Parsing.Nodes.Types.Subtypes;

using Sugar.Language.Analysis.ProjectCreation.Utilities;

using Sugar.Language.Analysis.ProjectStructure;

using Sugar.Language.Analysis.ProjectStructure.Enums;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Nodes;
using Sugar.Language.Analysis.ProjectStructure.ProjectNodes;
using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Referencing;
using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Generics;
using Sugar.Language.Parsing.Nodes.Types;
using Sugar.Language.Parsing.Nodes.Values.Generics;

namespace Sugar.Language.Analysis.ProjectCreation
{
    internal abstract class SemanticService 
    {
        protected readonly ProjectTree projectTree;
        private readonly Queue<FindResult> references;
        
        public SemanticService(ProjectTree _projectTree)
        {
            projectTree = _projectTree;
        }

        public abstract void Process();

        protected Describer CreateDescriber(DescriberNode describer)
        {
            DescriberEnum describerEnum = 0;

            foreach(var child in describer)
            {
                switch(child.Keyword.SyntaxKind)
                {
                    case SyntaxKind.Static:
                        describerEnum |= DescriberEnum.Static;
                        break;
                    case SyntaxKind.Public:
                        describerEnum |= DescriberEnum.Public;
                        break;
                    case SyntaxKind.Private:
                        describerEnum |= DescriberEnum.Private;
                        break;
                    case SyntaxKind.Protected:
                        describerEnum |= DescriberEnum.Protected;
                        break;
                    case SyntaxKind.Sealed:
                        describerEnum |= DescriberEnum.Sealed;
                        break;
                    case SyntaxKind.Virtual:
                        describerEnum |= DescriberEnum.Virtual;
                        break;
                    case SyntaxKind.Abstract:
                        describerEnum |= DescriberEnum.Abstract;
                        break;
                    case SyntaxKind.Override:
                        describerEnum |= DescriberEnum.Override;
                        break;
                    case SyntaxKind.Const:
                        describerEnum |= DescriberEnum.Const;
                        break;
                    case SyntaxKind.Readonly:
                        describerEnum |= DescriberEnum.Readonly;
                        break;
                    case SyntaxKind.In:
                        describerEnum |= DescriberEnum.In;
                        break;
                    case SyntaxKind.Out:
                        describerEnum |= DescriberEnum.Out;
                        break;
                    case SyntaxKind.Ref:
                        describerEnum |= DescriberEnum.Ref;
                        break;
                }
            }

            return new Describer(describerEnum);
        }
        protected IReadOnlyCollection<FindResult> FindReferences(DataType dataType, IdentifierNode identifier)
        {
            var references = EnqueueIntialReferences(dataType, identifier.Value);
            var finalReferences = new Queue<FindResult>();

            int length = references.Count;
            for(int i = 0; i < length; i++)
                finalReferences.Enqueue(new FindResult(1, references.Dequeue()));

            return finalReferences;
        }

        protected IReadOnlyCollection<FindResult> FindReferences(DataType dataType, LongIdentiferNode identifier)
        {
            var references = EnqueueIntialReferences(dataType, identifier.NameAt(0));
            var finalReferences = new Queue<FindResult>();
            
            for(int i = 1; i < identifier.SplitLength; i++)
            { 
                int length = references.Count;
 
                for (int j = 0; j < length; j++)
                {
                    var bottom = references.Dequeue();
                    var result = bottom.GetChildReference(identifier.NameAt(i));

                    if (result == null)
                        finalReferences.Enqueue(new FindResult(i + 1, bottom));
                    else
                    {
                        foreach (var reference in result)
                            references.Enqueue(reference);
                    }
                }
            }

            return finalReferences;
        }

        private Queue<IReferencable> EnqueueIntialReferences(DataType dataType, string value)
        {
            var references = new Queue<IReferencable>();

            foreach (var child in projectTree.DefaultNamespace)
                if (child.Name == value)
                    references.Enqueue(child);

            foreach (var reference in dataType.References)
            {
                if (reference.Name == value)
                    references.Enqueue(reference);

                var sub = reference.GetChildReference(value);
                if (sub != null)
                    foreach (var subref in sub)
                        references.Enqueue(subref);
            }

            return references;
        }

        protected DataType FindType(DataType dataType, TypeNode typeNode)
        {
            switch (typeNode.Type)
            {
                case TypeNodeEnum.BuiltIn:
                    return FindType((TypeKeywordNode)typeNode);
                case TypeNodeEnum.Array:
                    return FindType(dataType, (ArrayTypeNode)typeNode);
                case TypeNodeEnum.Created:
                    return FindType(dataType, (CreatedTypeNode)typeNode);
                case TypeNodeEnum.Function:
                    return FindType(dataType, (FunctionTypeNode)typeNode);
                default://function
                    //errror
                    return new InvalidDataType();
            }
        }

        protected DataType FindType(TypeKeywordNode type)
        {
            return projectTree.DefaultNamespace.GetInternalDataType(FindInternalType(type.Keyword));
        }

        protected DataType FindType(DataType dataType, CreatedTypeNode type)
        {
            Queue<DataType> references = new Queue<DataType>();
            if (type.Identifier.NodeType == ParseNodeType.Identifier)
            {
                foreach (var reference in FindReferences(dataType, (IdentifierNode)type.Identifier))
                {
                    if ((reference.Referencable.ProjectMemberType & ProjectMemberEnum.DataTypes) != reference.Referencable.ProjectMemberType)
                        continue;

                    references.Enqueue(dataType);
                }
            }
            else
            {
                var identifier = (LongIdentiferNode)type.Identifier;

                foreach (var reference in FindReferences(dataType, identifier))
                {
                    if (reference.Index != identifier.SplitLength - 1)
                        continue;
                    if ((reference.Referencable.ProjectMemberType & ProjectMemberEnum.DataTypes) != reference.Referencable.ProjectMemberType)
                        continue;
                    
                    references.Enqueue(dataType);
                }
            }

            if (type.Generic != null)
                CheckGeneric(dataType, type.Generic, references);
            
            return references.Dequeue();
        }

        protected void CheckGeneric(DataType dataType, GenericCallNode genericCallNode, Queue<DataType> references)
        {
            int length = references.Count;
            for (int i = 0; i < length; i++)
            {
                var current = references.Dequeue();
                if(current.GenericCount != genericCallNode.Length)
                    continue;

                DataType genericReference;
                switch (current.ProjectMemberType)
                {
                    case ProjectMemberEnum.Class:
                        genericReference = new GenericClass((Class)current);
                        break;
                    case ProjectMemberEnum.Struct:
                        genericReference = new GenericStruct((Struct)current);
                        break;
                    default://interface
                        genericReference = new GenericInterface((Interface)current);
                        break;
                }
                
                //check inheritance
                    
                foreach (TypeNode typeNode in genericCallNode)
                    genericReference.AddEntity(FindType(dataType, typeNode));
                    
                references.Enqueue(genericReference);
            }
        }
        
        protected DataType FindType(DataType dataType, FunctionTypeNode type)
        {
            if (type.Generic.Length != 1)
            {
                //error
                return new InvalidDataType();
            }

            if(type.Generic[0].Type == TypeNodeEnum.BuiltIn)
                return FindType((TypeKeywordNode)type.Generic[0]);
            else
                return FindType(dataType, (CreatedTypeNode)type.Generic[0]);
        }
        
        protected DataType FindType(DataType dataType, ArrayTypeNode type)
        {
            if (type.Generic.Length != 1)
            {
                //error
                return new InvalidDataType();
            }

            DataType arrayType = new GenericClass((Class)projectTree.DefaultNamespace.GetInternalDataType(FindInternalType(null)));
            
            if (type.Generic[0].Type == TypeNodeEnum.BuiltIn)
                arrayType.AddEntity(FindType((TypeKeywordNode)type.Generic[0]));
            else
            {
                var returnType = FindType(dataType, (CreatedTypeNode)type.Generic[0]);
                if (returnType == null)
                {
                    //error
                    arrayType.AddEntity(new InvalidDataType());
                }
                else
                    arrayType.AddEntity(returnType);
            }
            
            return arrayType;
        }

        protected InternalTypeEnum FindInternalType(Keyword keyword)
        {
            switch(keyword.SyntaxKind)
            {
                case SyntaxKind.Byte:
                    return InternalTypeEnum.Byte;
                case SyntaxKind.SByte:
                    return InternalTypeEnum.SByte;
                case SyntaxKind.Short:
                    return InternalTypeEnum.Short;
                case SyntaxKind.UShort:
                    return InternalTypeEnum.UShort;
                case SyntaxKind.Int:
                    return InternalTypeEnum.Integer;
                case SyntaxKind.UInt:
                    return InternalTypeEnum.UInteger;
                case SyntaxKind.Long:
                    return InternalTypeEnum.Long;
                case SyntaxKind.Ulong:
                    return InternalTypeEnum.ULong;
                case SyntaxKind.Float:
                    return InternalTypeEnum.Float;
                case SyntaxKind.Double:
                    return InternalTypeEnum.Double;
                case SyntaxKind.Decimal:
                    return InternalTypeEnum.Decimal;
                case SyntaxKind.Char:
                    return InternalTypeEnum.Character;
                case SyntaxKind.String:
                    return InternalTypeEnum.String;
                case SyntaxKind.Bool:
                    return InternalTypeEnum.Boolean;
                default:
                    return 0;
            }
        }
    }
}
