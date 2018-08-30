using CommandLine;

namespace Pixie
{
    internal abstract class CommonOptions
    {
        [Option('c', "config", Default = "config.json", HelpText = "configuration file path")]
        public string PixelSettingsPath { get; set; }
        
        public abstract string OutputFileName { get; set; }

        [Value(1, Hidden = true)]
        public string ExcessValue { get; set; }
    }
}