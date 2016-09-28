using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Pixie
{
    /// <summary>
    /// Stores color mapping settings
    /// </summary>
    [DataContract]
    internal class PixelSettings
    {
        [DataMember]
        public int BitsPerPixel;

        [DataMember]
        public int SymbolHeight;

        [DataMember]
        public int SymbolWidth;

        [DataMember]
        public int DelimeterWidth;

        [DataMember]
        public int DelimeterHeight;
        
        [DataMember]
        public string DelimeterColor;

        [DataMember]
        public Dictionary<string, int> ColorMapping;

        public PixelSettings()
        {
            ColorMapping = new Dictionary<string, int>();
        }

        /// <summary>
        /// Opens a filestream and deserialize JSON formatted <see cref="PixelSettings"/>
        /// </summary>
        /// <param name="fileName">Path to file</param>
        /// <returns><see cref="PixelSettings"/> isntance</returns>
        public static PixelSettings FromFile(string fileName)
        {
            try
            {
                using (var stream = File.OpenRead(fileName))
                {
                    var serializer = new DataContractJsonSerializer(typeof(PixelSettings), new DataContractJsonSerializerSettings() {UseSimpleDictionaryFormat = true});
                   return serializer.ReadObject(stream) as PixelSettings;
                }
            }
            catch (IOException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error reading from config file");
                Console.WriteLine(e.Message);
                throw;
            }
            
        }
    }
}