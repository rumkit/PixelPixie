using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixie
{
    abstract class LookUpDirectionSelector
    {
        public int ColumntStart { get; set; }
        public int RowStart { get; set; }
        public int ColumnEnd { get; set; }
        public int RowEnd { get; set; }
        public int RowDelta { get; set; }
        public int ColumntDelta { get; set; }

        private LookUpDirectionSelector()
        {

        }

        /// <summary>
        /// Returns the correct direction selector based on settings
        /// </summary>
        /// <param name="settings">PixelSettings instance</param>
        /// <returns>instance of Selector's descendant</returns>
        public static LookUpDirectionSelector GetSelector(LookupDirection direction)
        {
            switch (direction)
            {
                case LookupDirection.RowWise:
                    return new RowWiseSelector();
                case LookupDirection.ColumnWise:
                    return new ColumnWiseSelector();
                default:
                    throw new ArgumentException("Lookupdirection " + direction + " not supported");
            }
        }

        
        public abstract void ProcessSymbol(Action<int, int> symbolProcesser);

        private class RowWiseSelector : LookUpDirectionSelector
        {
            public override void ProcessSymbol(Action<int, int> symbolProcessor)
            {
                for (int j = ColumntStart; j < ColumnEnd; j += ColumntDelta)
                {
                    for (int i = RowStart; i < RowEnd; i += RowDelta)
                    {
                        symbolProcessor(i, j);
                    }
                }
            }
        }
        
        private class ColumnWiseSelector : LookUpDirectionSelector
        {
            public override void ProcessSymbol(Action<int, int> symbolProcessor)
            {
                for (int i = RowStart; i < RowEnd; i += RowDelta)
                {
                    for (int j = ColumntStart; j < ColumnEnd; j += ColumntDelta)
                    {
                        symbolProcessor(i, j);
                    }
                }

            }
        }

    }
}
