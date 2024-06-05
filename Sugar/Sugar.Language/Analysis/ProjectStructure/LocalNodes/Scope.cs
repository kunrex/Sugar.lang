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

        private readonly List<LocalVariableNode> variables;

        private readonly List<LocalVoidNode> voids;
        private readonly List<LocalMethodNode> methods;

        private readonly List<ILocalNode> statements;

        private IScopeParent parent;
        public IScopeParent Parent { get => parent; }

        public Scope()
        {
            variables = new List<LocalVariableNode>();

            voids = new List<LocalVoidNode>();
            methods = new List<LocalMethodNode>();

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
            voids.Add(localVoid);

            return this;
        }

        public LocalVoidNode TryFindLocalVoid(IdentifierNode identifier, FunctionParamatersNode arguments) => FindFunction(identifier.Value, voids, arguments);

        public ILocalFunctionParent AddLocalMethod(LocalMethodNode localMethod)
        {
            methods.Add(localMethod);

            return this;
        }

        public LocalMethodNode TryFindLocalMethod(IdentifierNode identifier, FunctionParamatersNode arguments) => FindFunction(identifier.Value, methods, arguments);

        public ILocalVariableParent AddVariable(LocalVariableNode localVariable)
        {
            variables.Add(localVariable);

            return this;
        }

        public LocalVariableNode TryFindVariable(IdentifierNode identifier)
        {
            var value = identifier.Value;
            foreach(var variable in variables)
                if (variable.Name == value)
                    return variable;

            return null;
        }

        public IEnumerator<ILocalNode> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        protected Function FindFunction<Function>(string name, List<Function> collection, FunctionParamatersNode parameters) where Function : IFunction
        {
            foreach (var function in collection)
                if (function.Name == name)
                {
                    bool exit = true;
                    foreach (var arg in parameters)
                    {
                        if (function.FindArgument(arg.Name) == null)
                        {
                            exit = false;
                            break;
                        }
                    }

                    if (exit)
                        return function;
                }

            return default(Function);
        }
    }
}
