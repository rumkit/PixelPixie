using System;
using System.Collections;
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
            var errorCode = Parser.Default.ParseArguments<GenerateOptions, ParseOptions>(args)
                .MapResult(
                    (GenerateOptions options) => GeneratePattern(options),
                    (ParseOptions options) => ParseFontImage(options),
                    errs => ErrorCode.ArgumentsMismatch);
            
            if(errorCode == ErrorCode.NoError)
                ConsoleLogger.WriteMessage($"SUCCESS! \nFile written: \"{_outputFileName}\"", MessageType.Info);
            else
            {
                NotifyError(errorCode);
            }
            return (int)errorCode;
        }

        private static void NotifyError(ErrorCode errorCode)
        {
            switch (errorCode)
            {
                case ErrorCode.UknownError:
                    ConsoleLogger.WriteMessage("There was error, but we have no idea why.", MessageType.Error);
                    break;
                case ErrorCode.ArgumentsMismatch:
                    ConsoleLogger.WriteMessage("Error parsing arguments. Check command line.", MessageType.Error);
                    break;
                case ErrorCode.FileNotFound:
                    ConsoleLogger.WriteMessage("File was not found", MessageType.Error);
                    break;
                case ErrorCode.FileParsingError:
                    ConsoleLogger.WriteMessage("Error parsing file", MessageType.Error);
                    break;
            }
        }

        /// <summary>
        /// parses image and write arrays to output file
        /// </summary>
        /// <param name="options">parsed command line args</param>
        /// <returns>error code</returns>
        static ErrorCode ParseFontImage(ParseOptions options)
        {
            try
            {
                if (options.ExcessValue != null)
                    return ErrorCode.ArgumentsMismatch;

                Settings = PixelSettings.FromFile(options.PixelSettingsPath);
                _outputFileName = options.OutputFileName;

                var bitmap = new Bitmap(Image.FromFile(options.InputFileName));
                var mapper = new PixelMapper(bitmap, Settings);
                var map = mapper.MapPixels(options.SkipHeaders);
                OutputFileFormatter.WriteOutput(map, options.OutputFileName, options.SingleArray);
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case FileNotFoundException _:
                        return ErrorCode.FileNotFound;
                    case ArgumentException _:
                        return ErrorCode.FileParsingError;
                    default:
                        return ErrorCode.UknownError;
                }
            }
            return ErrorCode.NoError;
        }

        /// <summary>
        /// Grid pattern generation and writing to file
        /// </summary>
        /// <param name="options">parsed command line args</param>
        /// <returns>error code</returns>
        static ErrorCode GeneratePattern(GenerateOptions options)
        {
            try
            {
                if (options.ExcessValue != null)
                    return ErrorCode.ArgumentsMismatch;

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
                switch (e)
                {
                    case PixelProcessingException _:
                        ConsoleLogger.WriteMessage(e.Message, MessageType.Error);
                        return ErrorCode.FileParsingError;
                    case FileNotFoundException _:
                        return ErrorCode.FileNotFound;
                    default:
                        return ErrorCode.UknownError;
                }
            }
            return ErrorCode.NoError;
        }

        private static byte[] ParseDataFile(string optionsInputFileName)
        {
            var text = File.ReadAllText(optionsInputFileName);
            var byteStrings = text.Split(new []{","}, StringSplitOptions.RemoveEmptyEntries);
            var bytes = byteStrings.Select(
                s =>
                {
                    //todo: cutout empty lines
                    return byte.Parse(s.Trim('\r', '\n', ' ').Replace("0x",""), NumberStyles.HexNumber);
                }).ToArray();
            return bytes;
        }
    }
}
