using CommandLine;

namespace Pixie
{
    internal class GenerateOptions : CommonOptions
    {
        [Option('w', "width", HelpText = "grid pattern width in symbols", MutuallyExclusiveSet = "Generation")]
        public int PatternWidth { get; set; }

        [Option('h', "height", HelpText = "grid pattern height in symbols", MutuallyExclusiveSet = "Generation")]
        public int PatternHeight { get; set; }

        [Option('o', "output", HelpText = "output file name", DefaultValue = "output.png")]
        public override string OutputFileName { get; set; }

        [Option('n', "enumerate", HelpText = "draw grid rows and columns numbers. Use '-n Hex' or '-n Decimal'", DefaultValue = Pixie.EnumerationStyle.None)]
        public EnumerationStyle EnumerationStyle { get; set; }
    }
}