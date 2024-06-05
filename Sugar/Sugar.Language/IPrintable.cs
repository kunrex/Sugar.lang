using System;

namespace Sugar.Language
{
    internal interface IPrintable
    {
        public void Print(string indent, bool last);
    }
}
