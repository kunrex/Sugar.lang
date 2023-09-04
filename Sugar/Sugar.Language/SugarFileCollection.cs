using System;
using System.Collections;
using System.Collections.Generic;

using Sugar.Language.Parsing;

using Sugar.Language.Services;
using Sugar.Language.Services.Interfaces;

namespace Sugar.Language
{
    internal sealed class SugarFileCollection : ICustomCollection<SugarFile>
    {
        private readonly List<SugarFile> files;
        public int Length { get => files.Count; }

        private SyntaxTreeCollection syntaxTree = null;
        public SyntaxTreeCollection SyntaxTree
        {
            get
            {
                if(syntaxTree == null)
                {
                    syntaxTree = new SyntaxTreeCollection();
                    foreach (var file in files)
                        syntaxTree.Add(file.SyntaxTree);
                }

                return syntaxTree;
            }
        }

        public bool Valid
        {
            get
            {
                foreach (var file in files)
                    if (file.ExceptionCount > 0)
                        return false;

                return true;
            }
        }

        public SugarFile this[int index] { get => throw new NotImplementedException(); }

        public SugarFileCollection()
        {
            files = new List<SugarFile>();
        }

        public SugarFileCollection Add(SugarFile file)
        {
            files.Add(file);

            return this;
        }

        public void PrintExceptions()
        {
            foreach (var file in files)
                file.PrintExceptions();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<SugarFile> GetEnumerator()
        {
            return new GenericEnumeratorService<SugarFileCollection, SugarFile>(this);
        }
    }
}
