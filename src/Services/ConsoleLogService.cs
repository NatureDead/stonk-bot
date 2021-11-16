using System;
using System.Threading.Tasks;
using Discord;

namespace StonkBot.Services
{
    public class ConsoleLogService : ILogService
    {
        public Task LogAsync(LogMessage logMessage)
        {
            Log(logMessage.Severity, logMessage.Message);
            return Task.CompletedTask;
        }

        public void Log(Exception exception)
        {
            Log(LogSeverity.Error, exception.Message);
        }

        private void Log(LogSeverity logSeverity, string message)
        {
            Console.WriteLine($"[{DateTime.Now:T}] [{logSeverity}] {message}");
        }
    }
}