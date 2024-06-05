using System;
using System.Collections.Generic;

using Sugar.Language.Tokens.Enums;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Describers;
using Sugar.Language.Parsing.Nodes.Expressions.Associative;

using Sugar.Language.Analysis.ProjectStructure;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes;
using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Referencing;

namespace Sugar.Language.Analysis.ProjectCreation
{
    internal abstract class SemanticService 
    {
        protected readonly ProjectTree projectTree;

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

        protected IReadOnlyCollection<Tuple<IReferencable, ParseNode>>FindReferences(DataType dataType, ParseNode node)
        {
            var references = new Queue<IReferencable>();
            var finalRefences = new Queue<Tuple<IReferencable, ParseNode>>();

            string value;
            ParseNode current;
            if(node.NodeType == ParseNodeType.Variable)
            {
                value = ((IdentifierNode)node).Value;
                current = null;
            }
            else
            {
                var dot = (DotExpression)node;

                value = ((IdentifierNode)dot.LHS).Value;
                current = dot.RHS;
            }

            
            foreach (var child in projectTree.DefaultNamespace)
                if (child.Name == value)
                    references.Enqueue(child);

            foreach (var reference in dataType.References)
            {
                if(reference.Name == value)
                    references.Enqueue(reference);

                var sub = reference.GetChildReference(value);
                if (sub != null)
                    foreach (var subref in sub)
                        references.Enqueue(subref);
            }

            while(current != null)
            {
                switch(current.NodeType)
                {
                    case ParseNodeType.Dot:
                        var dot = (DotExpression)current;
                        EnqeueReferences(((IdentifierNode)dot.LHS).Value);

                        current = dot.RHS;
                        break;
                    case ParseNodeType.Variable:
                        EnqeueReferences(((IdentifierNode)current).Value);

                        current = null;
                        break;
                }

                if (references.Count == 0)
                    break;
        
                void EnqeueReferences(string value)
                {
                    int length = references.Count;
                    for (int i = 0; i < length; i++)
                    {
                        var bottom = references.Dequeue();
                        var result = bottom.GetChildReference(value);
                        if (result == null)
                            finalRefences.Enqueue(new Tuple<IReferencable, ParseNode>(bottom, current));
                        else
                        {
                            foreach (var reference in result)
                                references.Enqueue(reference);
                        }
                    }
                }
            }

            return finalRefences;
        }
    }
}
