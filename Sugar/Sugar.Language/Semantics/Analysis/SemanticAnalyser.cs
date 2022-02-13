using System;

using Sugar.Language.Parsing;
using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Interfaces.Creation;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.UDDataTypes;
using Sugar.Language.Parsing.Nodes.Values;

namespace Sugar.Language.Semantics.Analysis
{
    internal sealed class SemanticAnalyser
    {
        public SyntaxTree Base { get; private set; }

        public SemanticAnalyser(SyntaxTree _base)
        {
            Base = _base;
        }

        public bool Analyse()
        {
            var baseNode = Base.BaseNode;

            switch (baseNode.NodeType)
            {
                case NodeType.Group:
                    if (!AnalyseScope(baseNode))
                        return false;

                    break;
            }

            return true;
        }

        private bool AnalyseScope(Node current)
        {
            foreach(var child in current.GetChildren())
            {
                switch(child.NodeType)
                {
                    case NodeType.Assignment:
                        var name = (IdentifierNode)((AssignmentNode)child).Value;

                        var declaration = FindVariableDeclaration(name, child);
                        if (declaration == null)
                            return false;
                        break;
                }
            }

            return true;
        }

        private Node FindVariableDeclaration(Node variable, Node currentLocation)
        {
            switch(variable.NodeType)
            {
                case NodeType.Dot:
                    //find the first identifiers definition
                    //locate the type and find the definition for the type
                    //continue the variable needed has veen found
                    break;
                case NodeType.Variable:
                    return FindVariableDeclaration((IdentifierNode)variable, currentLocation);
            }

            return null;
        }

        private Node FindVariableDeclaration(IdentifierNode variable, Node currentLocation)
        {
            var parent = currentLocation;

            while(parent != null)
            {
                var stopAt = parent;
                parent = parent.Parent;

                if (parent == null)
                    break;

                foreach (var child in parent.GetChildrenBefore(stopAt))
                {
                    switch(child.NodeType)
                    {
                        case NodeType.Initialise:
                        case NodeType.Declaration:
                            var converted = (ICreationNode_Name)child;

                            if (variable.Value == ((IdentifierNode)converted.Name).Value)
                                return child;
                            break;
                    }
                }
            }

            return null;
        }
    }
}
