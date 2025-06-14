using System;
using System.IO;

namespace BackendLib
{
    internal static class Logger
    {
        private static readonly string logPath = "conversion.log";

        internal static void Log(string message)
        {
            File.AppendAllText(logPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}");
        }
    }
}
