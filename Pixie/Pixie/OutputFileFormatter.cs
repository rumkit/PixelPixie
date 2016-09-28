using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Instrumentation;

namespace Pixie
{
    class OutputFileFormatter
    {
        public static void WriteOutput(List<byte[]> symbols, string fileName)
        {
            try
            {
                using (var stream = File.Open(fileName, FileMode.Create))
                using (var writer = new StreamWriter(stream))
                {
                    for (int i = 0; i < symbols.Count; i++)
                    {
                        writer.WriteLine($"//symbol {i+1}");
                        writer.Write("{");
                        for (int j = 0; j < symbols[i].Length; j++)
                        {
                            writer.Write($"0x{symbols[i][j]:X}");
                            if(j + 1 < symbols[i].Length) writer.Write(", ");
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