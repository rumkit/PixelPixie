using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Pixie
{
    internal class PixelMapper
    {
        private Bitmap _bitmap;

        public Dictionary<Color, int> ColorMapping { get; set; }

        public PixelMapper(Bitmap bitmap)
        {
            _bitmap = bitmap;
            ColorMapping = new Dictionary<Color, int>();
        }

        public byte[] MapPixels(int bitsPerPixel)
        {
            var bitsCount = _bitmap.Width * _bitmap.Height * bitsPerPixel;
            var bitArray = new BitArray(bitsCount);
            int arrayPosition = 0;

            for (var i = 0; i < _bitmap.Height; i++)
            {
                for (var j = 0; j < _bitmap.Width; j++)
                {
                    ProcessPixel(_bitmap.GetPixel(j, i), bitsPerPixel, bitArray, ref arrayPosition);
                }
            }

            return bitArray.ToByteArray();
        }

        private void ProcessPixel(Color color, int bitsPerPixel, BitArray outputArray, ref int outputArrayPosition)
        {
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