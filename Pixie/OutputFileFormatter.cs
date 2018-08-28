using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Pixie
{
    static class OutputFileFormatter
    {
        /// <summary>
        /// How many elements of array will be displayed in one line
        /// </summary>
        public static int ElementsPerLine = 10;

        /// <summary>
        /// Writes formatted output of symbols parser's result
        /// </summary>
        /// <param name="symbols">byte[] representation of symbols</param>
        /// <param name="fileName">path to the file to create</param>
        /// <param name="singleArray">shall output be written to single array or one array per symbol</param>
        public static void WriteOutput(List<byte[]> symbols, string fileName, bool singleArray = false)
        {
            try
            {
                using (var stream = File.Open(fileName, FileMode.Create))
                using (var writer = new StreamWriter(stream))
                {
                    var fileVersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
                    var copyRight = fileVersionInfo.LegalCopyright;
                    var version = fileVersionInfo.ProductVersion;
                    // Make header
                    writer.WriteLine("//");
                    writer.WriteLine("//");
                    writer.WriteLine($"// Generated with PixelPixie v.{version} {copyRight}");
                    writer.WriteLine("//");
                    writer.WriteLine("//");
                    writer.WriteLine();
                    writer.WriteLine();

                    // Write array(s)
                    if (singleArray)
                    {
                        var totalLength = (from s in symbols
                            select s.Length).Sum();

                        writer.Write($"unsigned char c[{totalLength}] = \n");
                        writer.Write("{\n    ");

                        int elementCounter = 0;

                        for (int i = 0; i < symbols.Count; i++)
                        {
                            for (int j = 0; j < symbols[i].Length; j++)
                            {
                                writer.Write($"0x{symbols[i][j]:X2}");
                                elementCounter++;
                                if (j + 1 < symbols[i].Length || i < symbols.Count - 1)
                                {
                                    writer.Write(", ");
                                    if (elementCounter%ElementsPerLine == ElementsPerLine - 1)
                                        writer.Write("\n    ");
                                }
                            }
                        }
                        writer.Write("\n};");
                        writer.WriteLine("\n");
                    }
                    else
                    {
                        for (int i = 0; i < symbols.Count; i++)
                        {
                            writer.WriteLine($"//symbol {i + 1}");
                            writer.Write($"unsigned char c{i + 1}[{symbols[i].Length}] = \n");
                            writer.Write("{\n    ");
                            for (int j = 0; j < symbols[i].Length; j++)
                            {
                                writer.Write($"0x{symbols[i][j]:X2}");
                                if (j + 1 < symbols[i].Length)
                                {
                                    writer.Write(", ");
                                    if (j%ElementsPerLine == ElementsPerLine - 1)
                                        writer.Write("\n    ");
                                }
                            }
                            writer.Write("\n};");
                            writer.WriteLine("\n");
                        }
                    }
                }
            }
            catch (IOException)
            {
                ConsoleLogger.WriteMessage("Failed writing output", MessageType.Error);
                throw;
            }
        }
    }
}