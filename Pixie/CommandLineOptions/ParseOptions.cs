using CommandLine;

namespace Pixie
{
    internal class ParseOptions : CommonOptions
    {
        [Option('s', "single-array", HelpText = "place all characters to single array")]
        public bool SingleArray { get; set; }

        [Option('i', "input", HelpText = "input file name", Required = true)]
        public string InputFileName { get; set; }

        [Option('o', "output", HelpText = "output file name", DefaultValue = "output.txt")]
        public override string OutputFileName { get; set; }
    }
}