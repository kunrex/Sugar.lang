using System;

using Sugar.Language;

namespace Sugar
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var source = "var: x = create Array(10)<int>;";
            new Compiler(source).Compile();

            Console.ReadKey();
        }        
    }
}
