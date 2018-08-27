using System;
using System.Runtime.Serialization;

namespace Pixie
{
    [Serializable]
    internal class PixelProcessingException : Exception
    {
        public PixelProcessingException()
        {
        }

        public PixelProcessingException(string message) : base(message)
        {
           
        }

        public PixelProcessingException(string message, Exception inner) : base(message, inner)
        {
           
        }

        protected PixelProcessingException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}