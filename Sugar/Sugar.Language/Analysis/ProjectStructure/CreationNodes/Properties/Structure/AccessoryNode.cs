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
        private readonly Describer describer;
        public Describer Describers { get => describer; }

        private readonly ParseNode body;
        public ParseNode Body { get => body; }

        protected AccessoryNode(Describer _describer, ParseNode _body) : base()
        {
            body = _body;
            describer = _describer;
        }

        public bool MatchDescriber(Describer toMatch) => Describer.ValidateDescriber(toMatch, describer);
        public bool ValidateDescribers() => Describer.ValidateDescription(describer, DescriberEnum.AccessModifiers);
    }
}
