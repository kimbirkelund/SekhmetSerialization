using System;

namespace Sekhmet.Serialization.Utility.Logging
{
    public interface IConsoleWriter
    {
        void WriteLine(ConsoleColor consoleColor, string message, Exception exception);
    }
}