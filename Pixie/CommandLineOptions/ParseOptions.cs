using CommandLine;

namespace Pixie
{
    internal class ParseOptions : CommonOptions
    {
        [Option('s', "single-array", HelpText = "place all characters to single array")]
        public bool SingleArray { get; set; }

        [ValueOption(0)]
        public string InputFileName { get; set; }

        [Option('o', "output", HelpText = "output file name", DefaultValue = "output.txt")]
        public override string OutputFileName { get; set; }

        [Option('h', "skip-headers", HelpText = "skip headers with rows and columns numbers (first row and first column are not processed)", DefaultValue = false)]
        public bool SkipHeaders { get; set; }
    }
}