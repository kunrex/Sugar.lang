using System;
using System.Collections.Generic;

namespace Sugar.Language.Parsing.Parser
{
    internal static class ParsingExtensions
    {
        public static T Pop<T>(this List<T> list)
        {
            if (list.Count == 0)
                throw new IndexOutOfRangeException();

            var pop = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return pop;
        }
    }
}
