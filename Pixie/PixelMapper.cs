using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;

namespace Pixie
{
    internal class PixelMapper
    {
        private readonly Bitmap _bitmap;
        private readonly PixelSettings _settings;
        private Dictionary<Color, int> ColorMappings { get; }
        
        public PixelMapper(Bitmap bitmap, PixelSettings settings)
        {
            _bitmap = bitmap;
            _settings = settings;
            
            ColorMappings = new Dictionary<Color, int>();
            foreach (var i in settings.ColorMapping)
            {
                ColorMappings.Add(ColorTranslator.FromHtml(i.Key), i.Value);
            }
        }


        /// <summary>
        /// map all cells in grid to byte arrays
        /// </summary>
        /// <returns>list of mapped arrays</returns>
        public List<byte[]> MapPixels()
        {
            if(_settings.SymbolWidth * _settings.SymbolHeight * _settings.BitsPerPixel % 8 != 0)
                ConsoleLogger.WriteMessage("Number of bits per pixel is not a multiple of 8. Output values will be padded", MessageType.Warning);

            var symbols = new List<byte[]>();

            for (int j = 0; j < _bitmap.Height; j += _settings.SymbolHeight + _settings.DelimeterHeight)
            {
                for (int i = 0; i < _bitmap.Width; i += _settings.SymbolWidth + _settings.DelimeterWidth)
                {
                    symbols.Add(ProcessSymbol(i, i + _settings.SymbolWidth, j, j + _settings.SymbolHeight));
                }
            }

            return symbols;
        }

        /// <summary>
        /// processes one symbol (one grid cell)
        /// </summary>
        /// <param name="symbolXStart">upper left corner of cell X coord</param>
        /// <param name="symbolXEnd">bottom right corner of cell X coord</param>
        /// <param name="symbolYStart">upper left corner of cell Y coord</param>
        /// <param name="symbolYEnd">bottom right corner of cell Y coord</param>
        /// <returns>byte representation of symbol</returns>
        private byte[] ProcessSymbol(int symbolXStart, int symbolXEnd, int symbolYStart, int symbolYEnd)
        {
            var bitsCount = (symbolXEnd - symbolXStart)*(symbolYEnd - symbolYStart)*_settings.BitsPerPixel;
            var bitArray = new BitArray(bitsCount);
            int arrayPosition = 0;

            for (var j = symbolYStart; j < symbolYEnd; j++)
            {
                for (var i = symbolXStart; i < symbolXEnd; i++)
                {
                    try
                    {
                        ProcessPixel(_bitmap.GetPixel(i, j), _settings.BitsPerPixel, bitArray, ref arrayPosition);
                    }
                    catch (PixelProcessingException e)
                    {
                        throw new PixelProcessingException( $"Problem detected while processing pixel at {i},{j}", e);
                    }
                }
            }

            return bitArray.ToByteArray();
        }

        /// <summary>
        /// Processes one pixel of image and writes corresponding bits in output array
        /// </summary>
        /// <param name="color">color of pixel</param>
        /// <param name="bitsPerPixel">bpp in output array</param>
        /// <param name="outputArray">output array</param>
        /// <param name="outputArrayPosition">current element in array</param>
        private void ProcessPixel(Color color, int bitsPerPixel, BitArray outputArray, ref int outputArrayPosition)
        {
            if (!ColorMappings.ContainsKey(color))
                throw new PixelProcessingException($"Can't find corresponding bits to pixel color " +
                                                   $"#{color.R.ToString("X2")}{color.G.ToString("X2")}{color.B.ToString("X2")}. Check your config");
            // Bits corresponding to color
            var colorBits = ColorMappings[color];
            // Bits left to process in current pixel
            var bitsToProcess = bitsPerPixel;
            while (bitsToProcess > 0)
            {
                // Check next bit and set flag in bit array if necessary
                if ((colorBits & (1 << (bitsToProcess - 1))) != 0)
                    outputArray[outputArrayPosition] = true;
                outputArrayPosition++;
                bitsToProcess--;
            }
        }
    }

    [Serializable]
    internal class PixelProcessingException : Exception
    {
        public PixelProcessingException()
        {
        }

        public PixelProcessingException(string message) : base(message)
        {
           
        }

        public PixelProcessingException(string message, Exception inner) : base(message, inner)
        {
           
        }

        protected PixelProcessingException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
     
    
}