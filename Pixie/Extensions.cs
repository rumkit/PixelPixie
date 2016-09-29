using System.Collections;

namespace Pixie
{
    /// <summary>
    /// Class for extensions methods
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// Converts BitArray to byte[]
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }
    }
}