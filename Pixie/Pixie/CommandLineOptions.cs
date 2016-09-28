using System.Text;
using CommandLine;
using CommandLine.Text;

namespace Pixie
{
    /// <summary>
    /// Options for CommandLine parser
    /// </summary>
    class CommandLineOptions
    {
        [Option('c', "config", DefaultValue = "config.json", HelpText = "configuration file path")]
        public string PixelSettingsPath { get; set; }

        [Option('o',"output", DefaultValue = "output.txt", HelpText = "output file name")]
        public string OutputFileName { get; set; }

        [Option('i', "input", HelpText = "output file name", Required = true)]
        public string InputFileName { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}