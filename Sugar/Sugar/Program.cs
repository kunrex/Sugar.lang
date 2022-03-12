using System;

using Sugar.Language;

namespace Sugar
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var source = "namespace name { class X { Y: y; } } namespace name2 { class Y { } }";
            new Compiler(source).Compile();

            Console.ReadKey();
        }        
    }
}
