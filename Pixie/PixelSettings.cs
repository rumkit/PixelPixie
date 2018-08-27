using System;
using System.Collections.Concurrent;
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

        [DataMember(Name="CellsLookupDirection")]
        private string _cellsLookupDirection;

        [DataMember(Name = "PixelsLookupDirection")]
        private string _pixelsLookupDirection;

        [DataMember(Name = "UserDefinedPixelOrder")]
        private int[][] _pixelOrder;

        [DataMember(Name = "UserDefinedBlockOrder")]
        private int[][] _blockOrder;
#pragma warning restore 0649

        [DataMember]
        public Dictionary<string, int> ColorMapping;

        private Dictionary<int, Pixel> _symbolPixelOrder;
        public Dictionary<int,Pixel> PixelOrder
        {
            get
            {
                if (_pixelOrder != null && _blockOrder != null)
                    return _symbolPixelOrder ?? (_symbolPixelOrder = GetPixelOrder());
                throw new ArgumentException("Check config for UserDefined orders");
            }
        }

        public LookupDirection CellsLookupDirection
        {
            get
            {
                var success = Enum.TryParse(_cellsLookupDirection, true, out LookupDirection direction);
                return success ? direction : LookupDirection.Unsupported;
            }
        }

        public LookupDirection PixelsLookupDirection
        {
            get
            {
                var success = Enum.TryParse(_pixelsLookupDirection, true, out LookupDirection direction);
                return success ? direction : LookupDirection.Unsupported;
            }
        }

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

        private Dictionary<int, Pixel> GetPixelOrder()
        {
            var pixelOrder2D = _pixelOrder.To2D();
            var blockOrder2D = _blockOrder.To2D();
            var elementsPerBlock = pixelOrder2D.Length;
            
            var symbolHeight = blockOrder2D.GetLength(0) * pixelOrder2D.GetLength(0);
            var symbolWidth = blockOrder2D.GetLength(1) * pixelOrder2D.GetLength(1);
            var resultOrder = new Dictionary<int, Pixel>();
            for (int j = 0; j < symbolHeight; j++)
            {
                for (int i = 0; i < symbolWidth; i++)
                {
                    var blockI = i / pixelOrder2D.GetLength(1);
                    var blockJ = j / pixelOrder2D.GetLength(0);
                    var startValue = blockOrder2D[blockJ,blockI] * elementsPerBlock;
                    var pixelI = i % pixelOrder2D.GetLength(1);
                    var pixelJ = j % pixelOrder2D.GetLength(0);
                    resultOrder.Add(startValue + pixelOrder2D[pixelJ,pixelI], new Pixel(i, j));
                }
            }

            return resultOrder;
        }
    }
}