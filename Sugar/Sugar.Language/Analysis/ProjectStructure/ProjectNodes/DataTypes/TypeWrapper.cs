using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.DataTypes;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

using Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions.Casting;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Structure;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes.Functions;

namespace Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes
{
    internal abstract class TypeWrapper<Skeleton> : DataType where Skeleton : DataTypeNode
    {
        protected readonly Skeleton skeleton;
        public Skeleton ParseSkeleton { get => skeleton; }
        
        public TypeWrapper(string _name, Describer _describer, Skeleton _skeleton, ReferenceCollection _references) : base(_name, _describer, _references)
        {
            skeleton = _skeleton;
        }

        protected Entity FindEntity<Entity>(string name, List<Entity> collection) where Entity : ICreationNode
        {
            foreach (var entity in collection)
                if (entity.Name == name)
                    return entity;

            return default(Entity);
        }

        protected Function FindFunction<Function>(string name, List<Function> collection, FunctionParamatersNode parameters) where Function : IFunction
        {
            foreach(var function in collection)
                if(function.Name == name)
                {
                    bool exit = true;
                    foreach(var arg in parameters)
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

        protected Cast FindCast<Cast>(DataType type, List<Cast> casts, FunctionParamatersNode parameters) where Cast : BaseCastNode
        {
            foreach (var cast in casts)
            {
                if (cast.CreationType == type)
                {
                    bool exit = true;
                    foreach (var arg in parameters)
                    {
                        if (cast.FindArgument(arg.Name) == null)
                        {
                            exit = false;
                            break;
                        }
                    }

                    if (exit)
                        return cast;
                }
            }

            return null;
        }
    }
}
