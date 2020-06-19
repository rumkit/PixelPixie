using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using CommandLine;

namespace Pixie
{

    class Program
    {
        public static PixelSettings Settings { get; private set; }
        private static string _outputFileName;

        static int Main(string[] args)
        {
            // Parse command-line arguments
            try
            {
                Parser.Default.ParseArguments<GenerateOptions, ParseOptions>(args)
                    .MapResult(
                        (GenerateOptions options) => GeneratePattern(options),
                        (ParseOptions options) => ParseFontImage(options),
                        // just exit, this usually means that no arguments were given
                        (errs) =>
                        {
                            System.Environment.Exit(1);
                            return null;
                        });
            }
            catch (Exception e)
            {
                NotifyError(e);
                return 1;
            }

            ConsoleLogger.WriteMessage($"SUCCESS! \nFile written: \"{_outputFileName}\"", MessageType.Info);

            return 0;
        }

        // Notifies user about error details
        private static void NotifyError(Exception e)
        {
            switch (e)
            {
                case FileNotFoundException _:
                    ConsoleLogger.WriteMessage($"File not found: {e.Message}", MessageType.Error);
                    return;
                case ArgumentException _:
                    ConsoleLogger.WriteMessage($"Error parsing file: {e.Message}", MessageType.Error);
                    return;
                default:
                    ConsoleLogger.WriteMessage($"Unexpected error: {e.Message}", MessageType.Error);
                    return;
            }
        }

        /// <summary>
        /// parses image and write arrays to output file
        /// </summary>
        /// <param name="options">parsed command line args</param>
        /// <returns>always returns null</returns>
        static object ParseFontImage(ParseOptions options)
        {
            if (options.ExcessValue != null)
            {
                throw new ArgumentException("Argument mismatch!");
            }

            Settings = PixelSettings.FromFile(options.PixelSettingsPath);
            _outputFileName = options.OutputFileName;

            var bitmap = new Bitmap(Image.FromFile(options.InputFileName));
            var mapper = new PixelMapper(bitmap, Settings);
            var map = mapper.MapPixels(options.SkipHeaders);
            OutputFileFormatter.WriteOutput(map, options.OutputFileName, options.SingleArray, options.ArrayContentOnly);

            return null;
        }

        /// <summary>
        /// Grid pattern generation and writing to file
        /// </summary>
        /// <param name="options">parsed command line args</param>
        /// <returns>always returns null</returns>
        static object GeneratePattern(GenerateOptions options)
        {
            if (options.ExcessValue != null)
            {
                throw new ArgumentException("Argument mismatch!");
            }

            Settings = PixelSettings.FromFile(options.PixelSettingsPath);
            _outputFileName = options.OutputFileName;

            var generator = new PatternGenerator(Settings);
            byte[] sampleData = null;
            if (options.InputFileName != null)
                sampleData = ParseDataFile(options.InputFileName);
            var pattern = generator.GeneratePattern(options.PatternWidth,
                options.PatternHeight, options.EnumerationStyle, sampleData);
            pattern.Save(options.OutputFileName);

            return null;
        }

        // Parses file with font hex'es (csv file with hex values e.g. 0xDE, 0xAD, 0xBE, 0xEF ...)
        private static byte[] ParseDataFile(string optionsInputFileName)
        {
            var text = File.ReadAllText(optionsInputFileName);
            var byteStrings = text.Split(new []{","}, StringSplitOptions.RemoveEmptyEntries);
            var bytes = byteStrings.Select(
                s => byte.Parse(s.Trim().Replace("0x",""), NumberStyles.HexNumber)).ToArray();
            return bytes;
        }
    }
}
