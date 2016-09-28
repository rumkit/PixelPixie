using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Pixie
{
    public enum ErrorCode
    {
        NoError = 0,
        FileNotFound,
        FileParsingError
    }
    class Program
    {
        static int Main(string[] args)
        {
            var options = new CommandLineOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                try
                {
                    var bitmap = new Bitmap(Image.FromFile(options.InputFileName));
                    var settings = PixelSettings.FromFile(options.PixelSettingsPath);
                    var mapper = new PixelMapper(bitmap, settings);
                    var map = mapper.MapPixels();
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Critical error, exiting =(");
                    return (int)ErrorCode.FileNotFound;
                }
                catch (Exception)
                {
                    Console.WriteLine("Critical error, exiting =(");
                    return (int)ErrorCode.FileParsingError;
                }
            }

            return (int)ErrorCode.NoError;

        }




    }
}
