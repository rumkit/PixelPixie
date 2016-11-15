using CommandLine;

namespace Pixie
{
    internal abstract class CommonOptions
    {
        [Option('c', "config", DefaultValue = "config.json", HelpText = "configuration file path")]
        public string PixelSettingsPath { get; set; }
        
        public abstract string OutputFileName { get; set; }
    }
}