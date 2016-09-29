using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Instrumentation;

namespace Pixie
{
    static class OutputFileFormatter
    {
        private const int DigitsPerLine = 10;

        public static void WriteOutput(List<byte[]> symbols, string fileName)
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


                    for (int i = 0; i < symbols.Count; i++)
                    {
                        writer.WriteLine($"//symbol {i+1}");
                        writer.Write($"unsigned char c{i+1}[{symbols[i].Length}] = \n    ");
                        writer.Write("{");
                        for (int j = 0; j < symbols[i].Length; j++)
                        {
                            writer.Write($"0x{symbols[i][j]:X}");
                            if(j + 1 < symbols[i].Length)
                            {
                                writer.Write(", ");
                                if(j % DigitsPerLine == DigitsPerLine - 1)
                                    writer.Write("\n    ");
                            }
                        }
                        writer.Write("}");
                        writer.WriteLine("\n");
                        
                    }
                }
            }
            catch(IOException exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed writing output");
                Console.WriteLine(exception.Message);
                throw;
            }
        }
    }
}