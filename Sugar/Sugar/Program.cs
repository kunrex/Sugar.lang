using System;
using System.IO;

using Sugar.Language;

namespace Sugar
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var source = "foreach(var: x in array) \n    print(x);";
            new Compiler(source).Compile();

            Console.ReadKey();
        }        
    }
}
