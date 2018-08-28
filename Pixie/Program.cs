using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
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
            var errorCode = Parser.Default.ParseArguments<GenerateOptions, ParseOptions>(args)
                .MapResult(
                    (GenerateOptions options) => GeneratePattern(options),
                    (ParseOptions options) => ParseFontImage(options),
                    errs => (int) ErrorCode.ArgumentsMismatch);
            
            if(errorCode == (int)ErrorCode.NoError)
                ConsoleLogger.WriteMessage($"SUCCESS! \nFile written: \"{_outputFileName}\"", MessageType.Info);
            else
            {
                //todo: write all error-related text here. e.g. MakeError(...)
                Console.WriteLine("Error! Can't parse arguments");
                Environment.Exit(errorCode);
            }
            return errorCode;
        }

        /// <summary>
        /// parses image and write arrays to output file
        /// </summary>
        /// <param name="options">parsed command line args</param>
        /// <returns>error code</returns>
        static int ParseFontImage(ParseOptions options)
        {
            try
            {
                if (options.ExcessValue != null)
                    return (int)ErrorCode.ArgumentsMismatch;

                Settings = PixelSettings.FromFile(options.PixelSettingsPath);
                _outputFileName = options.OutputFileName;

                var bitmap = new Bitmap(Image.FromFile(options.InputFileName));
                var mapper = new PixelMapper(bitmap, Settings);
                var map = mapper.MapPixels(options.SkipHeaders);
                OutputFileFormatter.WriteOutput(map, options.OutputFileName, options.SingleArray);
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException)
                {
                    ConsoleLogger.WriteMessage($"File not found or invalid file name \"{e.Message}\"", MessageType.Error);
                    return (int)ErrorCode.FileNotFound;
                }

                ConsoleLogger.WriteMessage(e.Message + "\n" + e.InnerException?.Message, MessageType.Error);

                if (e is ArgumentException)
                {
                   return (int)ErrorCode.FileParsingError;
                }
               
                return (int)ErrorCode.UknownError;
            }
            return (int) ErrorCode.NoError;
        }

        /// <summary>
        /// Grid pattern generation and writing to file
        /// </summary>
        /// <param name="options">parsed command line args</param>
        /// <returns>error code</returns>
        static int GeneratePattern(GenerateOptions options)
        {
            try
            {
                if (options.ExcessValue != null)
                    return (int)ErrorCode.ArgumentsMismatch;

                Settings = PixelSettings.FromFile(options.PixelSettingsPath);
                _outputFileName = options.OutputFileName;

                var generator = new PatternGenerator(Settings);
                byte[] sampleData = null;
                if (options.InputFileName != null)
                    sampleData = ParseDataFile(options.InputFileName);
                var pattern = generator.GeneratePattern(options.PatternWidth,
                    options.PatternHeight, options.EnumerationStyle, sampleData);
                pattern.Save(options.OutputFileName);
            }
            catch (Exception e)
            {
                ConsoleLogger.WriteMessage(e.Message + "\n" + e.InnerException?.Message, MessageType.Error);
                if (e is FileNotFoundException)
                    return (int)ErrorCode.FileNotFound;
                return (int)ErrorCode.UknownError;
            }
            return (int)ErrorCode.NoError;
        }

        private static byte[] ParseDataFile(string optionsInputFileName)
        {
            var text = File.ReadAllText(optionsInputFileName);
            var byteStrings = text.Split(new []{","}, StringSplitOptions.RemoveEmptyEntries);
            var bytes = byteStrings.Select(
                s =>
                {
                    return byte.Parse(s.Trim('\r', '\n', ' ').Replace("0x",""), NumberStyles.HexNumber);
                }).ToArray();
            return bytes;
        }
    }
}
