using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixie
{
    class BitmapPixelTracker : IEnumerable<Pixel>
    {
        public int YStart { get; set; }
        public int XStart { get; set; }
        public int YEnd { get; set; }
        public int XEnd { get; set; }
        public int XDelta { get; set; }
        public int YDelta { get; set; }
        public BitmapPixelTracker(LookupDirection direction)
        {
            _direction = direction;
        }
        private readonly LookupDirection _direction;

        public IEnumerator<Pixel> GetEnumerator()
        {
            switch (_direction)
            {
                case LookupDirection.RowWise:
                    return GetRowWiseEnumerator();
                case LookupDirection.ColumnWise:
                    return GetColumnWiseEnumerator();
                case LookupDirection.RowWiseYReverse:
                    return GetRowWiseReverseEnumerator();
                case LookupDirection.ColumnWiseYReverse:
                    return GetColumnWiseReverseEnumerator();
                case LookupDirection.UserDefined:
                    return GetUserDefinedEnumerator();
                default:
                    throw new ArgumentException("Lookupdirection " + _direction + " not supported");
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerator<Pixel> GetRowWiseEnumerator()
        {
            for (int y = YStart; y < YEnd; y += YDelta)
            {
                for (int x = XStart; x < XEnd; x += XDelta)
                {
                    yield return new Pixel(x, y);
                }
            }
        }

        private IEnumerator<Pixel> GetColumnWiseEnumerator()
        {
            for (int x = XStart; x < XEnd; x += XDelta)
            {
                for (int y = YStart; y < YEnd; y += YDelta)
                {
                    yield return new Pixel(x, y);
                }
            }
        }

        private IEnumerator<Pixel> GetRowWiseReverseEnumerator()
        {

            for (int y = YEnd - YDelta; y >= YStart; y -= YDelta)
            {
                for (int x = XStart; x < XEnd; x += XDelta)
                {
                    yield return new Pixel(x, y);
                }
            }
        }

        private IEnumerator<Pixel> GetColumnWiseReverseEnumerator()
        {
            for (int x = XStart; x < XEnd; x += XDelta)
            {
                for (int y = YEnd - YDelta; y >= YStart; y -= YDelta)
                {
                    yield return new Pixel(x, y);
                }
            }
        }

        private IEnumerator<Pixel> GetUserDefinedEnumerator()
        {
            var pixelOrder = Program.Settings.PixelOrder;
            for (int i = 0; i < pixelOrder.Count; i++)
            {
                yield return pixelOrder[i];
            }
        }
    }
}
