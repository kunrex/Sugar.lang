using System;
using System.IO;
using System.Collections.Generic;

using Sugar.Language.Compilers;

using Sugar.Language.Exceptions;

using Sugar.Language.Configurations;

namespace Sugar.Language
{
    public sealed class SugarCompiler : ICompiler
    {
        private readonly SugarConfiguration configuration;

        public SugarCompiler(SugarConfiguration config)
        {
            configuration = config;
        }

        public bool Compile()
        {
            Console.WriteLine($"SUGAR: [Version: {configuration.Version}]");
            Console.WriteLine();

            try
            {
                var compiler = new FileCompiler();
                return compiler.Compile(ReadFiles(configuration.SourceFolder), ReadFiles(configuration.WrapperFileLocation));
            }
            catch(CompileException e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                return false;
            }
        }


        /// <summary>
        /// Read files starting in the base directory and subdirectories.
        /// </summary>
        private string[] ReadFiles(string location)
        {
            var extension = configuration.SugarFileExtension;
            if (extension == null)
                return null;

            var files = new List<string>();
            ReadFiles(location, in files, in extension);

            return files.ToArray();
        }

        /// <summary>
        /// Read files in a specified directory and subdirectories.
        /// </summary>
        private void ReadFiles(string location, in List<string> sourceFiles, in string extension)
        {
            foreach (var path in Directory.GetFiles(location))
                if (path.Contains(extension))
                    sourceFiles.Add(File.ReadAllText(path));

            foreach (var directory in Directory.GetDirectories(location))
                ReadFiles(directory, in sourceFiles, in extension);
        }
    }
}
