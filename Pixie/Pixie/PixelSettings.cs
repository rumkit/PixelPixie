using System;
using System.Collections.Generic;
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
            catch (IOException)
            {
                Console.WriteLine("Error reading from file");
                return null;
            }
            
        }
    }
}