using System;

using Sugar.Language.Services;

using Sugar.Language.Parsing;

using Sugar.Language.Analysis.ProjectCreation;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes;

namespace Sugar.Language.Analysis
{
    internal sealed class SemanticAnalyser : SingletonService<SemanticAnalyser>
    {
        private SyntaxTreeCollection wrapper;
        private SyntaxTreeCollection project;

        public SemanticAnalyser()
        {
            
        }

        public SemanticAnalyser WithTrees(SyntaxTreeCollection _wrapperTree, SyntaxTreeCollection _projectTree)
        {
            if (wrapper == null && project == null)
            {
                wrapper = _wrapperTree;
                project = _projectTree;
            }

            return this;
        }

        public ProjectTree Analyse()
        {
            var projectTree = new ProjectTree("test");

            new NamespaceConstructor(wrapper, project, projectTree).Process();
            new ReferenceMapper(wrapper, project, projectTree).Process();

            foreach(var refrence in projectTree.DefaultNamespace[0].References)
            {
                if(refrence != null)
                    Console.WriteLine(refrence.Name);
            }


            return projectTree;
        }
    }
}
