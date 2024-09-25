using System;
using System.Collections;
using System.Collections.Generic;

using Sugar.Language.Exceptions.Analysis.Processing;

using Sugar.Language.Services.Interfaces;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

using Sugar.Language.Analysis.ProjectStructure.LocalNodes.Functions;
using Sugar.Language.Analysis.ProjectStructure.LocalNodes.Variables;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Nodes;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Scope;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Functions;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Variables;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes.Functions;

namespace Sugar.Language.Analysis.ProjectStructure.LocalNodes
{
    internal class Scope : ICustomCollection<ILocalNode>, IParentableNode<IScopeParent>, ILocalNode, IScopeParent, ILocalVariableParent, ILocalFunctionParent
    {
        public MemberTypeEnum MemberType { get => MemberTypeEnum.Local; }
        public LocalMemberEnum LocalMember { get => LocalMemberEnum.Scope; }
  
        public int Length { get => variables.Count + voids.Count + methods.Count + statements.Count; }
        public ILocalNode this[int index] { get => throw new NotImplementedException(); }

        
        private readonly Dictionary<string, LocalVariableNode> variables;

        private readonly Dictionary<string, List<LocalVoidNode>> voids;
        private readonly Dictionary<string, List<LocalMethodNode>> methods;

        private readonly List<ILocalNode> statements;

        private IScopeParent parent;
        public IScopeParent Parent { get => parent; }

        public Scope()
        {
            variables = new Dictionary<string, LocalVariableNode>();

            voids = new Dictionary<string, List<LocalVoidNode>>();
            methods = new Dictionary<string, List<LocalMethodNode>>();

            statements = new List<ILocalNode>();
        }

        public void SetParent(IScopeParent scopeParent)
        {
            if (parent != null)
                throw new DoubleParentAssignementException();

            parent = scopeParent;
        }

        public ILocalFunctionParent AddLocalVoid(LocalVoidNode localVoid)
        {
            voids.TryGetValue(localVoid.Name, out var value);
            if(value != null)
                value.Add(localVoid);
            else
                voids.Add(localVoid.Name, new List<LocalVoidNode>() { localVoid });

            return this;
        }

        public LocalVoidNode TryFindLocalVoid(IdentifierNode identifier, FunctionParamatersNode arguments) => FindFunction(identifier.Value, voids, arguments);

        public ILocalFunctionParent AddLocalMethod(LocalMethodNode localMethod)
        {
            methods.TryGetValue(localMethod.Name, out var value);
            if(value != null)
                value.Add(localMethod);
            else
                methods.Add(localMethod.Name, new List<LocalMethodNode>() { localMethod });

            return this;
        }

        public LocalMethodNode TryFindLocalMethod(IdentifierNode identifier, FunctionParamatersNode arguments) => FindFunction(identifier.Value, methods, arguments);

        public ILocalVariableParent AddVariable(LocalVariableNode localVariable)
        {
            variables.Add(localVariable.Name, localVariable);

            return this;
        }

        public LocalVariableNode TryFindVariable(IdentifierNode identifier)
        {
            variables.TryGetValue(identifier.Value, out LocalVariableNode value);
            return value;
        }

        public IEnumerator<ILocalNode> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        protected Function FindFunction<Function>(string name, Dictionary<string, List<Function>> collection, FunctionParamatersNode parameters) where Function : IFunction
        {
            collection.TryGetValue(name, out var value);
            if (value != null)
            {
                foreach (var definition in value)
                {
                    bool exit = true;
                    foreach (var arg in parameters)
                    {
                        if (definition.FindArgument(arg.Name) == null)
                        {
                            exit = false;
                            break;
                        }
                    }

                    if (exit)
                        return definition;
                }
            }

            return default(Function);
        }
    }
}
