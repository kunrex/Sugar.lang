using System;

namespace Sugar.Language.Configurations
{
    public sealed class SugarConfiguration
    {
        public string SugarFileExtension { get; set; }

        public int Version { get; set; }

        public string SourceFolder { get; set; }
        public string WrapperFileLocation { get; set; }
    }
}
