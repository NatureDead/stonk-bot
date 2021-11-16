using System;
using Discord;

namespace StonkBot.Settings
{
    public class ConfigFile : YamlFile
    {
        protected override int NewestVersion => 0;
        protected override string ResourceName => "config.yml";

        public LogSeverity LogSeverity { get; private set; }
        public string DiscordToken { get; private set; }

        protected override void OnChanged()
        {
            base.OnChanged();

            LogSeverity = GetLogSeverity();
            DiscordToken = GetDiscordToken();
        }

        public LogSeverity GetLogSeverity()
        {
            var loggingValue = Configuration["logging"];
            return (LogSeverity)Enum.Parse(typeof(LogSeverity), loggingValue, true);
        }

        public string GetDiscordToken()
        {
            return Configuration["discord:token"];
        }
    }
}