using System.Drawing;
using System.Globalization;
using System.IO;

namespace Pixie
{
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

        private void FillBackground(Bitmap pattern)
        {
            for (int j = 0; j < pattern.Height; j++)
            {
                for (int i = 0; i < pattern.Width; i++)
                {
                   pattern.SetPixel(i,j, BackGround);
                }
            }
        }

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