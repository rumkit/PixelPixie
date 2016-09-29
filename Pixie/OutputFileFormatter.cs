using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pixie
{
    static class OutputFileFormatter
    {
        private const int ElementsPerLine = 10;

        public static void WriteOutput(List<byte[]> symbols, string fileName, bool singleArray = false)
        {
            try
            {
                using (var stream = File.Open(fileName, FileMode.Create))
                using (var writer = new StreamWriter(stream))
                {
                    writer.WriteLine("//");
                    writer.WriteLine("//");
                    writer.WriteLine("// Generated with PixelPixie (c) 2016");
                    writer.WriteLine("//");
                    writer.WriteLine("//");
                    writer.WriteLine();
                    writer.WriteLine();

                    if (singleArray)
                    {
                        var totalLength = (from s in symbols
                            select s.Length).Sum();

                        writer.Write($"unsigned char c[{totalLength}] = \n    ");
                        writer.Write("{");

                        int elementCounter = 0;

                        for (int i = 0; i < symbols.Count; i++)
                        {
                            for (int j = 0; j < symbols[i].Length; j++)
                            {
                                writer.Write($"0x{symbols[i][j]:X}");
                                elementCounter++;
                                if (j + 1 < symbols[i].Length || i < symbols.Count - 1)
                                {
                                    writer.Write(", ");
                                    if (elementCounter%ElementsPerLine == ElementsPerLine - 1)
                                        writer.Write("\n    ");
                                }
                            }
                        }
                        writer.Write("}");
                        writer.WriteLine("\n");
                    }
                    else
                    {
                        for (int i = 0; i < symbols.Count; i++)
                        {
                            writer.WriteLine($"//symbol {i + 1}");
                            writer.Write($"unsigned char c{i + 1}[{symbols[i].Length}] = \n    ");
                            writer.Write("{");
                            for (int j = 0; j < symbols[i].Length; j++)
                            {
                                writer.Write($"0x{symbols[i][j]:X}");
                                if (j + 1 < symbols[i].Length)
                                {
                                    writer.Write(", ");
                                    if (j%ElementsPerLine == ElementsPerLine - 1)
                                        writer.Write("\n    ");
                                }
                            }
                            writer.Write("}");
                            writer.WriteLine("\n");
                        }
                    }
                }
            }
            catch (IOException exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed writing output");
                Console.WriteLine(exception.Message);
                throw;
            }
        }
    }
}