﻿using System;

using Sugar.Language.Parsing.Nodes;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes
{
    internal interface IBody
    {
        public ParseNodeCollection Body { get; }
    }
}
