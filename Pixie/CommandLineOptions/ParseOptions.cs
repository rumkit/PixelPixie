using CommandLine;

namespace Pixie
{
    [Verb("parse", HelpText = "parses  a pattern filled with graphical font to a byte array")]
    internal class ParseOptions : CommonOptions
    {
        [Option('s', "single-array", HelpText = "place all characters into single array")]
        public bool SingleArray { get; set; }
        
        [Option( "array-contents-only", HelpText = "Omit single array name and curly braces, only produce array contents")]
        public bool ArrayContentOnly { get; set; }

        [Value(0, HelpText = "image containing font bitmap", MetaName = "file name", Required = true)]
        public string InputFileName { get; set; }

        [Option('o', "output", HelpText = "output file name", Default = "output.txt")]
        public override string OutputFileName { get; set; }

        [Option('h', "skip-headers", HelpText = "skip headers with rows and columns numbers (first row and first column are not processed)", Default = false)]
        public bool SkipHeaders { get; set; }
    }
}