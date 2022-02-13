using System;

using Sugar.Language;

namespace Sugar
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var source = "int: x = -+-+x; ";
            new Compiler(source).Compile();

            Console.ReadKey();
        }        
    }
}
