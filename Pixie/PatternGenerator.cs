using System.Drawing;

namespace Pixie
{
    /// <summary>
    /// Generates empty grid pattern bitmap
    /// </summary>
    internal class PatternGenerator
    {
        private PixelSettings _settings;
        private Color _delimeterColor;
        private static readonly Color BackGround = Color.Gray;

        public PatternGenerator(PixelSettings settings)
        {
            _settings = settings;
            _delimeterColor = ColorTranslator.FromHtml(_settings.DelimeterColor);
        }

        /// <summary>
        /// Fills bitmap with solid color and draws a grid on it
        /// </summary>
        /// <param name="patternWidthCount">width of grid in symbols (cells)</param>
        /// <param name="patternHeightCount">height of grid in symbols (cells)</param>
        /// <returns>production ready bitmap</returns>
        public Bitmap GeneratePattern(int patternWidthCount, int patternHeightCount)
        {
            var pattentWidth = patternWidthCount * _settings.SymbolWidth +
                               (patternWidthCount - 1) * _settings.DelimeterWidth;
            var patternHeight = patternHeightCount * _settings.SymbolHeight +
                                (patternHeightCount - 1) * _settings.DelimeterHeight;
            var pattern = new Bitmap(pattentWidth, patternHeight);

            FillBackground(pattern);
            DrawVerticalLines(pattern);
            DrawHorizontalLines(pattern);

            return pattern;
        }

        /// <summary>
        /// Fills background with a solid color
        /// </summary>
        /// <param name="pattern">bitmap to fill</param>
        private void FillBackground(Bitmap pattern)
        {
            var g = Graphics.FromImage(pattern);
            g.FillRectangle(new SolidBrush(BackGround), new Rectangle(0, 0, pattern.Width, pattern.Height));

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
    }
}