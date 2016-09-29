using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Pixie
{
    class Program
    {
        static int Main(string[] args)
        {
            var options = new CommandLineOptions();

            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                if (options.IsPatternRequested)
                {
                    return GeneratePattern(options);
                }
                return ParseFontImage(options);
            }

            return (int) ErrorCode.ArgumentsMismatch;
        }

        static int ParseFontImage(CommandLineOptions options)
        {
            try
            {
                var bitmap = new Bitmap(Image.FromFile(options.InputFileName));
                var settings = PixelSettings.FromFile(options.PixelSettingsPath);
                var mapper = new PixelMapper(bitmap, settings);
                var map = mapper.MapPixels();
                OutputFileFormatter.WriteOutput(map, options.OutputFileName);
            }
            catch (Exception e)
            {
                Console.WriteLine("Critical error, exiting =(");
                if (e is FileNotFoundException)
                    return (int)ErrorCode.FileNotFound;
                if (e is ArgumentException)
                    return (int)ErrorCode.FileParsingError;
                return (int)ErrorCode.UknownError;
            }
            return (int) ErrorCode.NoError;
        }

        static int GeneratePattern(CommandLineOptions options)
        {
            try
            {
                var settings = PixelSettings.FromFile(options.PixelSettingsPath);
                var generator = new PatternGenerator(settings);
                var pattern = generator.GeneratePattern(options.PatternWidth, options.PatternHeight);
                pattern.Save(options.OutputFileName);
            }
            catch (Exception e)
            {
                Console.WriteLine("Critical error, exiting =(");
                if (e is FileNotFoundException)
                    return (int)ErrorCode.FileNotFound;
                return (int)ErrorCode.UknownError;
            }
            return (int)ErrorCode.NoError;
        }

    }
}
