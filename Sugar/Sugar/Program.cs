using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

using Sugar.Language;
using Sugar.Configurations;


namespace Sugar
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var basePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            var configuration = JsonSerializer.Deserialize<SugarConfiguration>(File.ReadAllText(Path.Combine(basePath, "Sugar", "Config.json")));

            var compiler = new Compiler(ReadFiles(configuration.SourceFolder), ReadFiles(configuration.WrapperFileLocation));
            compiler.Compile();
        }

        private static List<string> ReadFiles(string location)
        {
            List<string> sourceFiles = new List<string>();
            foreach (var path in Directory.GetFiles(location))
            {
                if (path.EndsWith(".sugar"))
                    sourceFiles.Add(File.ReadAllText(path));
            }

            return sourceFiles;
        }
    }
}
