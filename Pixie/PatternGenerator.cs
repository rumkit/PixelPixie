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
        private Color _backGroundColor;

        public PatternGenerator(PixelSettings settings)
        {
            _settings = settings;
            _delimeterColor = ColorTranslator.FromHtml(_settings.DelimeterColor);
            _backGroundColor = ColorTranslator.FromHtml(_settings.BackgroundColor);
        }

        /// <summary>
        /// Fills bitmap with solid color and draws a grid on it
        /// </summary>
        /// <param name="patternWidthCount">width of grid in symbols (cells)</param>
        /// <param name="patternHeightCount">height of grid in symbols (cells)</param>
        /// <returns>production ready bitmap</returns>
        public Bitmap GeneratePattern(int patternWidthCount, int patternHeightCount)
        {
            var pattentWidth = patternWidthCount*_settings.SymbolWidth +
                               (patternWidthCount - 1)*_settings.DelimeterWidth;
            var patternHeight = patternHeightCount*_settings.SymbolHeight +
                                (patternHeightCount - 1)*_settings.DelimeterHeight;
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
            for (int j = 0; j < pattern.Height; j++)
            {
                for (int i = 0; i < pattern.Width; i++)
                {
                   pattern.SetPixel(i,j, _backGroundColor);
                }
            }
        }

        /// <summary>
        /// Draws horizontal lines, wich will devide grid cells
        /// </summary>
        /// <param name="pattern">bitmap to draw in</param>
        private void DrawHorizontalLines(Bitmap pattern)
        {
            for (int i = 0; i < pattern.Width; i++)
            {
                for (int j = _settings.SymbolHeight; j < pattern.Height; j += _settings.SymbolHeight)
                {
                    var linePixelsLeft = _settings.DelimeterHeight;
                    while (linePixelsLeft > 0)
                    {
                        pattern.SetPixel(i, j, _delimeterColor);
                        j++;
                        linePixelsLeft--;
                    }
                }
            }
        }

        /// <summary>
        /// Draws vertical lines, wich will devide grid cells
        /// </summary>
        /// <param name="pattern">bitmap to draw in</param>
        private void DrawVerticalLines(Bitmap pattern)
        {
            for (int j = 0; j < pattern.Height; j++)
            {
                for (int i = _settings.SymbolWidth; i < pattern.Width; i+=_settings.SymbolWidth)
                {
                    var linePixelsLeft = _settings.DelimeterWidth;
                    while (linePixelsLeft > 0)
                    {
                        pattern.SetPixel(i, j, _delimeterColor);
                        i++;
                        linePixelsLeft--;
                    }
                }
            }
        }
    }
}