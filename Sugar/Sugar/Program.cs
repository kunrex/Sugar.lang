using System;

using Sugar.Language;

namespace Sugar
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var source = "import enum x.z.x; namespace x { class z {  enum x { } } } namespace x.y { class x { } } class z { }";
            new Compiler(source).Compile();

            Console.ReadKey();
        }        
    }
}
