using System;
using System.Collections.Generic;

namespace Sugar.Language.Services.Interfaces
{
    internal interface ICustomCollection<Enumerable> : IEnumerable<Enumerable>
    {
        public int Length { get; }
        public Enumerable this[int index] { get; }
    }
}
