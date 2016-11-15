using System;
using System.Linq;
using CommandLine;
using CommandLine.Text;

namespace Pixie
{
    /// <summary>
    /// Options for CommandLine parser
    /// each field represent a key in console args
    /// </summary>
    class CommandLineOptions
    {
        [ParserState]
        public IParserState LastParserState { get; set; }

        [VerbOption("generate", HelpText = "generates a graphical pattern")]
        public GenerateOptions GenerateOptions { get; set; }

        [VerbOption("parse", HelpText = "parses  a pattern filled with graphical font to a byte array")]
        public ParseOptions ParseOptions { get; set; }
        
        [HelpVerbOption]
        public string GetUsage(string verb)
        {
            return HelpText.AutoBuild(this, verb);
        }
    }
}