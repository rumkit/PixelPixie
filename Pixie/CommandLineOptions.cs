using CommandLine;
using CommandLine.Text;

namespace Pixie
{
    /// <summary>
    /// Options for CommandLine parser
    /// </summary>
    class CommandLineOptions
    {
        [Option('s', "single-array", HelpText = "place all characters to single array", MutuallyExclusiveSet = "Parse")]
        public bool SingleArray { get; set; }

        [Option('c', "config", DefaultValue = "config.json", HelpText = "configuration file path")]
        public string PixelSettingsPath { get; set; }

        [Option('o',"output", Required = true, HelpText = "output file name")]
        public string OutputFileName { get; set; }

        [Option('i', "input", HelpText = "output file name", MutuallyExclusiveSet = "Parse")]
        public string InputFileName { get; set; }

        [Option('g',"generate",HelpText = "generate grid pattern", MutuallyExclusiveSet = "Generation")]
        public bool IsPatternRequested { get; set; }

        [Option('w', "width", HelpText = "grid pattern width in symbols", MutuallyExclusiveSet = "Generation")]
        public int PatternWidth { get; set; }

        [Option('h', "height", HelpText = "grid pattern height in symbols", MutuallyExclusiveSet = "Generation")]
        public int PatternHeight { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}