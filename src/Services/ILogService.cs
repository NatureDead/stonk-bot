using System;
using System.Threading.Tasks;
using Discord;

namespace StonkBot.Services
{
    public interface ILogService
    {
        Task LogAsync(LogMessage logMessage);

        void Log(Exception exception);
    }
}