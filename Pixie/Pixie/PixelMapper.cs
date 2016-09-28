using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;

namespace Pixie
{
    internal class PixelMapper
    {
        private Bitmap _bitmap;
        private PixelSettings _settings;

        private Color DelimeterColor { get; }
        private Dictionary<Color, int> ColorMapping { get; }

        public PixelMapper(Bitmap bitmap, PixelSettings settings)
        {
            _bitmap = bitmap;
            _settings = settings;
            DelimeterColor = ColorTranslator.FromHtml(settings.DelimeterColor);
            ColorMapping = new Dictionary<Color, int>();
            foreach (var i in settings.ColorMapping)
            {
                ColorMapping.Add(ColorTranslator.FromHtml(i.Key), i.Value);
            }

        }


        public List<byte[]> MapPixels(bool splitCharacters = true)
        {
            var symbols = new List<byte[]>();

            if (splitCharacters)
            {
                for (int i = 0; i < _bitmap.Height; i += _settings.SymbolHeight + _settings.DelimeterHeight)
                {
                    for (int j = 0; j < _bitmap.Width; j += _settings.SymbolWidth + _settings.DelimeterWidth)
                    {
                        symbols.Add(ProcessSymbol(j, j + _settings.SymbolWidth, i, i + _settings.SymbolHeight));
                    }
                }
            }
            else
            {
                symbols.Add(ProcessSymbol(0,_bitmap.Width,0,_bitmap.Height));
            }

            return symbols;
        }

        private byte[] ProcessSymbol(int symbolXStart, int symbolXEnd, int symbolYStart, int symbolYEnd)
        {
            var bitsCount = (symbolXEnd - symbolXStart) * (symbolYEnd - symbolYStart) * _settings.BitsPerPixel;
            var bitArray = new BitArray(bitsCount);
            int arrayPosition = 0;

            for (var i = symbolYStart; i < symbolYEnd; i++)
            {
                for (var j = symbolXStart; j < symbolXEnd; j++)
                {
                    try
                    {
                        ProcessPixel(_bitmap.GetPixel(j, i), _settings.BitsPerPixel, bitArray, ref arrayPosition);
                    }
                    catch (ArgumentException exception)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Problem detected at pixel {j},{i}");
                        Console.WriteLine(exception.Message);
                        throw;
                    }
                    
                    
                }
            }

            return bitArray.ToByteArray();
        }

        private void ProcessPixel(Color color, int bitsPerPixel, BitArray outputArray, ref int outputArrayPosition)
        {
            if (color == DelimeterColor)
                return;
            if (!ColorMapping.ContainsKey(color))
                    throw new ArgumentException("Can't find corresponding bits to pixel color");
            // Bits corresponding to color
            var colorBits = ColorMapping[color];
            // Bits left to process in current pixel
            var bitsToProcess = bitsPerPixel;
            while (bitsToProcess > 0)
            {
                // Check next bit and set flag in bit array if necessary
                if ((colorBits & (1 << (bitsToProcess - 1))) > 0)
                    outputArray[outputArrayPosition] = true;
                outputArrayPosition++;
                bitsToProcess--;
            }

        }
    }
}