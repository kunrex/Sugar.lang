using System;

using Sugar.Language.Tokens.Enums;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.Services.Interfaces;
using Sugar.Language.Semantics.ActionTrees.Describers;

namespace Sugar.Language.Semantics.Services.Implementations
{
    internal class DescriberService : IDescriberService
    {
        public Describer AnalyseDescriber(DescriberNode describerNode) => new Describer(GatherDescribers(describerNode));

        private DescriberEnum GatherDescribers(DescriberNode describerNode)
        {
            DescriberEnum describer = 0;

            for (int i = 0; i < describerNode.ChildCount; i++)
            {
                var child = (DescriberKeywordNode)describerNode[i];

                switch (child.Keyword.SyntaxKind)
                {
                    case SyntaxKind.Public:
                        describer |= DescriberEnum.Public;
                        break;
                    case SyntaxKind.Private:
                        describer |= DescriberEnum.Private;
                        break;
                    case SyntaxKind.Protected:
                        describer |= DescriberEnum.Protected;
                        break;

                    case SyntaxKind.Static:
                        describer |= DescriberEnum.Static;
                        break;

                    case SyntaxKind.Sealed:
                        describer |= DescriberEnum.Sealed;
                        break;
                }
            }

            return describer;
        }
    }
}
