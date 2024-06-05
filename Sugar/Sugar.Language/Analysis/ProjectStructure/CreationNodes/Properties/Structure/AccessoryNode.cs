using System;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.LocalNodes;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Describers;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes;

namespace Sugar.Language.Analysis.ProjectStructure.CreationNodes.Properties.Structure
{
    internal abstract class AccessoryNode : Scope, IDescribable, IBody
    {
        protected readonly Describer describer;
        public Describer Describers { get => describer; }

        protected readonly ParseNodeCollection body;
        public ParseNodeCollection Body { get => body; }

        public AccessoryNode(Describer _describer, ParseNodeCollection _body) : base()
        {
            body = _body;
            describer = _describer;
        }

        public bool ValidateDescribers() => describer.ValidateDescription(DescriberEnum.AccessModifiers);
        public bool MatchDescriber(Describer tomatch) => tomatch.ValidateDescriber(describer);
    }
}
