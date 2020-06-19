using System.Collections;
using System.Collections.Generic;
using System.Drawing;

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
        /// <param name="skipHeaders">will skip first row and first column</param>
        /// <returns>list of mapped arrays</returns>
        public List<byte[]> MapPixels(bool skipHeaders)
        {
            ConsoleLogger.WriteMessage($"Parsing bitmap to byte array", MessageType.Info);
            if (_settings.SymbolWidth * _settings.SymbolHeight * _settings.BitsPerPixel % 8 != 0)
                ConsoleLogger.WriteMessage("Number of bits per symbol is not a multiple of 8. Output values will be padded", MessageType.Warning);

            var symbols = new List<byte[]>();

            var columnCount = (_bitmap.Width + _settings.DelimeterWidth) /
                              (_settings.SymbolWidth + _settings.DelimeterWidth);
            var rowCount = (_bitmap.Height + _settings.DelimeterHeight) /
                           (_settings.SymbolHeight + _settings.DelimeterHeight);

            var pixelTracker = new BitmapPixelTracker(_settings.CellsLookupDirection)
            {
                // Skip first row and columnt if they were used for rows/columns numbers
                XStart = skipHeaders ? 1 : 0,
                XEnd = columnCount,
                XDelta = 1,
                // Skip first row and columnt if they were used for rows/columns numbers
                YStart = skipHeaders ? 1 : 0,
                YEnd = rowCount,
                YDelta = 1
            };

            // Process cells (symbols)
            // in this case pixel coords will correspond to cell
            foreach (var pixel in pixelTracker)
            {
                // get symbol top leftPoint
                var symbolX = pixel.X * (_settings.DelimeterWidth + _settings.SymbolWidth);
                var symbolY = pixel.Y * (_settings.DelimeterHeight + _settings.SymbolHeight);
                symbols.Add(ProcessSymbol(symbolX, symbolX + _settings.SymbolWidth, symbolY, symbolY + _settings.SymbolHeight));
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

            var pixelTracker = new BitmapPixelTracker(_settings.PixelsLookupDirection)
            {
                XStart = symbolXStart,
                XEnd = symbolXEnd,
                XDelta = 1,
                YStart = symbolYStart,
                YEnd = symbolYEnd,
                YDelta = 1
            };
            foreach (var pixel in pixelTracker)
            {
                try
                {
                    ProcessPixel(_bitmap.GetPixel(pixel.X, pixel.Y), _settings.BitsPerPixel, bitArray, ref arrayPosition);
                }
                catch (PixelProcessingException e)
                {
                    throw new PixelProcessingException($"Problem detected while processing pixel at x:{pixel.X}, y:{pixel.Y}. " +
                                                       $"{e.Message}");
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
}