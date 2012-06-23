using System;

namespace Sekhmet.Serialization.Utility.Logging
{
    public class DefaultConsoleWriter : IConsoleWriter
    {
        private static readonly object _mutex = new object();

        public void WriteLine(ConsoleColor consoleColor, string message, Exception exception)
        {
            lock (_mutex)
            {
                Console.ForegroundColor = consoleColor;
                Console.WriteLine(message);
                if (exception != null)
                    Console.WriteLine(exception);

                Console.ResetColor();
            }
        }
    }
}