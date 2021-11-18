using Discord;
using StonkBot.Services;
using System;

namespace StonkBot.Settings
{
    public class ConfigFile : YamlFile
    {
        protected override int NewestVersion => 0;
        protected override string ResourceName => "config.yml";

        public LogSeverity LogSeverity { get; private set; }
        public BrowserEngineType BrowserEngineType { get; private set; }
        public string BotToken { get; private set; }
        public string CoinAddress { get; private set; }

        protected override void OnChanged()
        {
            base.OnChanged();

            LogSeverity = GetLoggingSeverity();
            BrowserEngineType = GetBrowserEngineType();
            BotToken = GetBotToken();
            CoinAddress = GetCoinAddress();
        }

        public LogSeverity GetLoggingSeverity()
        {
            var loggingLevelValue = Configuration["logging_level"];
            return (LogSeverity)Enum.Parse(typeof(LogSeverity), loggingLevelValue, true);
        }

        private BrowserEngineType GetBrowserEngineType()
        {
            var browserEngineValue = Configuration["browser_engine"];
            return (BrowserEngineType)Enum.Parse(typeof(BrowserEngineType), browserEngineValue, true);
        }

        public string GetBotToken()
        {
            return Configuration["bot_token"];
        }

        private string GetCoinAddress()
        {
            return Configuration["coin_address"];
        }
    }
}