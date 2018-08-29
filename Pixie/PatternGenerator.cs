using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Xml;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Pixie
{
    /// <summary>
    /// Generates empty grid pattern bitmap
    /// </summary>
    internal class PatternGenerator
    {
        private PixelSettings _settings;
        private Color _delimeterColor;
        private Color _backGroundColor;
        private Dictionary<int, Color> Colors = new Dictionary<int, Color>();

        public PatternGenerator(PixelSettings settings)
        {
            _settings = settings;
            _delimeterColor = ColorTranslator.FromHtml(_settings.DelimeterColor);
            _backGroundColor = ColorTranslator.FromHtml(_settings.BackgroundColor);
            foreach (var i in settings.ColorMapping)
            {
                Colors.Add(i.Value, ColorTranslator.FromHtml(i.Key));
            }
        }

        /// <summary>
        /// Fills bitmap with solid color and draws a grid on it
        /// </summary>
        /// <param name="patternWidthCount">width of grid in symbols (cells)</param>
        /// <param name="patternHeightCount">height of grid in symbols (cells)</param>
        /// <returns>production ready bitmap</returns>
        public Bitmap GeneratePattern(int patternWidthCount, int patternHeightCount,
            EnumerationStyle enumerationStyle, byte[] sampleData = null)
        {
            if (enumerationStyle != EnumerationStyle.None)
            {
                patternHeightCount++;
                patternWidthCount++;
            }

            var pattentWidth = patternWidthCount * _settings.SymbolWidth +
                               (patternWidthCount - 1) * _settings.DelimeterWidth;
            var patternHeight = patternHeightCount * _settings.SymbolHeight +
                                (patternHeightCount - 1) * _settings.DelimeterHeight;
            var pattern = new Bitmap(pattentWidth, patternHeight);

            ConsoleLogger.WriteMessage($"Generating pattern\nBitmap size {pattentWidth} x {patternHeight} px", MessageType.Info);

            FillBackground(pattern);
            DrawVerticalLines(pattern);
            DrawHorizontalLines(pattern);
            Enumerate(pattern, enumerationStyle);
            if (sampleData != null)
                FillSampleData(pattern, sampleData, enumerationStyle);
            return pattern;
        }

        /// <summary>
        /// Fills background with a solid color
        /// </summary>
        /// <param name="pattern">bitmap to fill</param>
        private void FillBackground(Bitmap pattern)
        {
            var g = Graphics.FromImage(pattern);
            g.FillRectangle(new SolidBrush(_backGroundColor), new Rectangle(0, 0, pattern.Width, pattern.Height));
        }

        /// <summary>
        /// Draws horizontal lines, wich will devide grid cells
        /// </summary>
        /// <param name="pattern">bitmap to draw in</param>
        private void DrawHorizontalLines(Bitmap pattern)
        {
            var g = Graphics.FromImage(pattern);
            for (var i = _settings.SymbolHeight; i < pattern.Height; i += _settings.SymbolHeight + _settings.DelimeterHeight)
            {
                g.DrawLine(new Pen(_delimeterColor, _settings.DelimeterHeight), 0, i, pattern.Width, i);
            }

        }

        /// <summary>
        /// Draws vertical lines, wich will devide grid cells
        /// </summary>
        /// <param name="pattern">bitmap to draw in</param>
        private void DrawVerticalLines(Bitmap pattern)
        {
            var g = Graphics.FromImage(pattern);
            for (var i = _settings.SymbolWidth; i < pattern.Width; i += _settings.SymbolWidth + _settings.DelimeterWidth)
            {
                g.DrawLine(new Pen(_delimeterColor, _settings.DelimeterWidth), i, 0, i, pattern.Height);
            }
        }

        /// <summary>
        /// Loads custom font from resource and returns it
        /// </summary>
        /// <param name="resourceName">name of resource, containing font</param>
        /// <param name="size">font size</param>
        private Font GetCustomFont(string resourceName, float size)
        {
            var assembly = Assembly.GetExecutingAssembly();
            
            // create private font collection object
            PrivateFontCollection pfc = new PrivateFontCollection();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                var buffer = new byte[stream.Length];
                var result = stream.Read(buffer, 0, buffer.Length);
                // create an unsafe memory block for the font data
                System.IntPtr data = Marshal.AllocCoTaskMem(buffer.Length);
                // copy the bytes to the unsafe memory block
                Marshal.Copy(buffer, 0, data, buffer.Length);
                // pass the font to the font collection
                pfc.AddMemoryFont(data, buffer.Length);
            }
            return new Font(pfc.Families[0], size); 
        }

        /// <summary>
        /// Draws line and column numbers
        /// </summary>
        /// <param name="pattern">bitmap to draw in</param>
        /// <param name="enumerationStyle">style of digits</param>
        private void Enumerate(Bitmap pattern, EnumerationStyle enumerationStyle)
        {
            // 72 pixels in one pt
            const int pixelsPerPoint = 72;
            const string fontName = "Pixie.Resources.XpaiderPE.TTF";
            if (enumerationStyle == EnumerationStyle.None)
                return;

            var graphics = Graphics.FromImage(pattern);

            // rows and column numbers will be 0.75 of symbol size
            var fontSize = (float)(_settings.SymbolHeight * 0.75 * pixelsPerPoint / pattern.VerticalResolution);

            var font = GetCustomFont(fontName, fontSize);
            var brush = new SolidBrush(_delimeterColor);

            // select string format specifier based on enumeration style
            var numbersStyle = enumerationStyle == EnumerationStyle.Hex ? "X" : "D2";

            // we use 6px sized font so we don't need any anti-aliasing
            graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;

            // Enumerate rows
            for (int rowHeight = _settings.SymbolHeight + _settings.DelimeterHeight, rowNumber = 0, i = rowHeight;
                i < pattern.Height;
                i += rowHeight)
            {
                graphics.DrawString(rowNumber++.ToString(numbersStyle), font, brush, 0, i);
            }

            // Enumerate columns
            for (int columnWidth = _settings.SymbolWidth + _settings.DelimeterWidth, columnNumber = 0, i = columnWidth;
                i < pattern.Width;
                i += columnWidth)
            {
                graphics.DrawString(columnNumber++.ToString(numbersStyle), font, brush, i, 0);
            }
        }

        private void FillSampleData(Bitmap pattern, byte[] sampleData, EnumerationStyle enumerationStyle)
        {
            var bitArray = sampleData.ToBitArray();
            int bitArrayIndex = 0;
            

            var columnCount = (pattern.Width + _settings.DelimeterWidth) /
                              (_settings.SymbolWidth + _settings.DelimeterWidth);
            var rowCount = (pattern.Height + _settings.DelimeterHeight) /
                           (_settings.SymbolHeight + _settings.DelimeterHeight);

            var pixelTracker = new BitmapPixelTracker(_settings.CellsLookupDirection)
            {
                // Skip first row and columnt if they were used for rows/columns numbers
                XStart = enumerationStyle == EnumerationStyle.None? 0: 1,
                XEnd = columnCount,
                XDelta = 1,
                // Skip first row and columnt if they were used for rows/columns numbers
                YStart = enumerationStyle == EnumerationStyle.None? 0: 1,
                YEnd = rowCount,
                YDelta = 1
            };

            // Process cells
            // in this case pixel coords will correspond to cell
            foreach (var pixel in pixelTracker)
            {
                // get symbol top leftPoint
                var symbolX = pixel.X * (_settings.DelimeterWidth + _settings.SymbolWidth);
                var symbolY = pixel.Y * (_settings.DelimeterHeight + _settings.SymbolHeight);
                FillSymbol(symbolX, symbolY, pattern, bitArray, ref bitArrayIndex);
            }
        }

        private void FillSymbol(int i, int j, Bitmap pattern, BitArray sampleData, ref int index)
        {
            var pixelTracker = new BitmapPixelTracker(_settings.PixelsLookupDirection)
            {
                XStart = i,
                XEnd = _settings.SymbolWidth + i,
                XDelta = 1,
                YStart = j,
                YEnd = _settings.SymbolHeight + j,
                YDelta = 1
            };
            foreach (var pixel in pixelTracker)
            {
                // If there was not enough data, just skip the symbol (leaves cell empty)
                if (index >= sampleData.Length)
                    return;
                var pixelValue = sampleData.ToByte(index, _settings.BitsPerPixel);
                pattern.SetPixel(pixel.X,  pixel.Y, Colors[pixelValue]);
                index += _settings.BitsPerPixel;
            }
        }
    }
}