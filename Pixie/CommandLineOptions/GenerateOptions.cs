using CommandLine;

namespace Pixie
{
    [Verb("generate", HelpText = "generates a graphical pattern optionally filled with font")]
    internal class GenerateOptions : CommonOptions
    {
        [Option('w', "width", HelpText = "grid pattern width in symbols", Required = true)]
        public int PatternWidth { get; set; }

        [Option('h', "height", HelpText = "grid pattern height in symbols", Required = true)]
        public int PatternHeight { get; set; }

        [Option('o', "output", HelpText = "output file name", Default = "output.png")]
        public override string OutputFileName { get; set; }

        [Option('n', "enumerate", HelpText = "draw grid rows and columns numbers. Use '-n Hex' or '-n Decimal'. Resulting image should be parsed with -h option", Default = Pixie.EnumerationStyle.None)]
        public EnumerationStyle EnumerationStyle { get; set; }

        [Option('i', "input", HelpText = "path to csv file with HEX data, that will be parsed and used to fill images with symbols")]
        public string InputFileName { get; set; }
    }
}