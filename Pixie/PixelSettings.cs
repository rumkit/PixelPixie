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
#pragma warning disable 0649
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
        public string BackgroundColor;
#pragma warning restore 0649

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
                   ConsoleLogger.WriteMessage($"Loaded configuration from {stream.Name}", MessageType.Info);
                   return serializer.ReadObject(stream) as PixelSettings;
                }
            }
            catch (IOException e)
            {
                ConsoleLogger.WriteMessage("Error reading from config file", MessageType.Error);
                ConsoleLogger.WriteMessage(e.Message, MessageType.Error);
                throw;
            }
            
        }
    }
}