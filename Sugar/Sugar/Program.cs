using System;

using Sugar.Language;

namespace Sugar
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var source = "import a; namespace x { class y {  enum x { } } } namespace x.y { class x { } } class z { }";
            new Compiler(source).Compile();

            Console.ReadKey();
        }        
    }
}
