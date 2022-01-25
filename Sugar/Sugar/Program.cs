using System;
using System.IO;

using Sugar.Language;

namespace Sugar
{
    internal static class Program
    {
        static void Main(string[] args)
        { 
            var source = "print(\"hello world!\")";
            new Compiler(source).Compile();

            Console.ReadKey();
        }        
    }
}
