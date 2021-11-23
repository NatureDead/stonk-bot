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
        public string TokenChain { get; private set; }
        public string TokenAddress { get; private set; }
        public string DiscordBotToken { get; private set; }
        public string DexGuruApiKey { get; private set; }
        public int PageDelay {  get; private set; }

        protected override void OnChanged()
        {
            base.OnChanged();

            LogSeverity = GetLoggingSeverity();
            BrowserEngineType = GetBrowserEngineType();
            TokenChain = GetTokenChain();
            TokenAddress = GetTokenAddress();
            DiscordBotToken = GetDiscordBotToken();
            DexGuruApiKey = GetDexGuruApiKey();
            PageDelay = GetPageDelay();
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

        private string GetTokenChain()
        {
            return Configuration["token:chain"];
        }

        private string GetTokenAddress()
        {
            return Configuration["token:address"];
        }

        public string GetDiscordBotToken()
        {
            return Configuration["discord:bot_token"];
        }

        public string GetDexGuruApiKey()
        {
            return Configuration["dex_guru_api_key"];
        }

        private int GetPageDelay()
        {
            var pageDelayValue = Configuration["page_delay"];
            return int.Parse(pageDelayValue);
        }
    }
}