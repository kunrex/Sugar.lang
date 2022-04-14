﻿using System;

namespace Sugar.Language.Parsing.Nodes.Types.Enums
{
    internal enum TypeNodeEnum : byte
    {
        Void,
        Array,
        Action,
        Custom,
        BuiltIn,
        Function,
        Anonymous,
        Constructor
    }
}
