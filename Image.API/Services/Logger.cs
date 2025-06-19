namespace Image.API.Services
{
    public static class Logger 
    {
        private static readonly string logPath = "conversion.log";

        public static void Log(string message)
        {
            File.AppendAllText(logPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}");
        }
    }
}
