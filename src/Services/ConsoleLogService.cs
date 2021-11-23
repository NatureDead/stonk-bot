using System;
using System.Threading.Tasks;
using Discord;

namespace StonkBot.Services
{
    public class ConsoleLogService : ILogService
    {
        public Task LogAsync(LogMessage logMessage)
        {
            if (logMessage.Exception == null)
                Log(logMessage.Severity, logMessage.Message);
            else
                Log(logMessage.Exception);

            return Task.CompletedTask;
        }

        public void Log(Exception exception)
        {
            while (exception != null)
            {
                Log(LogSeverity.Error, exception.Message + Environment.NewLine + exception.StackTrace);
                exception = exception.InnerException;
            }
        }

        public void Log(LogSeverity logSeverity, string message)
        {
            Console.WriteLine($"[{DateTime.Now:T}] [{logSeverity}] {message}");
        }
    }
}