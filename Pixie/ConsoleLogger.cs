using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixie
{
    static class ConsoleLogger
    {
        private const ConsoleColor ErrorBackColor = ConsoleColor.Red;
        private const ConsoleColor WarningBackColor = ConsoleColor.Yellow;
        private const ConsoleColor ErrorForeColor = ConsoleColor.Black;
        private const ConsoleColor WarningForeColor = ConsoleColor.Black;

        private const string ErrorCaption = "Critical error! ";
        private const string WarningCaption = "WARNING: ";
        private const string InfoCaption = "INFO: ";


        /// <summary>
        /// Writes formated message to console
        /// </summary>
        /// <param name="message">message to write</param>
        /// <param name="messageType">message severity <see cref="MessageType"/></param>
        public static void WriteMessage(string message, MessageType messageType)
        {
            var previousBackColor = Console.BackgroundColor;
            var previousForeColor = Console.ForegroundColor;
            string caption;

            switch (messageType)
            {
                case MessageType.Warning:
                    Console.BackgroundColor = WarningBackColor;
                    Console.ForegroundColor = WarningForeColor;
                    caption = WarningCaption;
                    break;
                case MessageType.Info:
                    caption = InfoCaption;
                    break;
                case MessageType.Error:
                    Console.BackgroundColor = ErrorBackColor;
                    Console.ForegroundColor = ErrorForeColor;
                    caption = ErrorCaption;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null);
            }

            Console.Write(caption);
            Console.WriteLine(message);

            Console.BackgroundColor = previousBackColor;
            Console.ForegroundColor = previousForeColor;
        }
    }

    internal enum MessageType
    {
        Warning,
        Info,
        Error
    }
}
