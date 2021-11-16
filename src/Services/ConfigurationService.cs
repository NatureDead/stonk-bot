using StonkBot.Settings;

namespace StonkBot.Services
{
    public class ConfigurationService
    {
        public ConfigFile ConfigFile { get; }

        public ConfigurationService()
        {
            ConfigFile = YamlFile.Get<ConfigFile>();
        }
    }
}