using System;
using System.IO;
using System.Text.Json;

using Sugar.Language;
using Sugar.Language.Configurations;

namespace Sugar
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var basePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            var configuration = JsonSerializer.Deserialize<SugarConfiguration>(File.ReadAllText(Path.Combine(basePath, "Sugar", "Config.json")));

            var compiler = new SugarCompiler(configuration);

            compiler.Compile();

        }
    }
}
