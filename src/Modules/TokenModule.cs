using Discord.Commands;
using OpenQA.Selenium;
using StonkBot.Services;
using StonkBot.Settings;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StonkBot.Modules
{
    public class TokenModule : ModuleBase<SocketCommandContext>
    {
        private readonly ConfigurationService _configurationService;
        private readonly ILogService _logService;

        public TokenModule(ConfigurationService configurationService, ILogService logService)
        {
            _configurationService = configurationService ??
                throw new ArgumentNullException(nameof(configurationService));

            _logService = logService ??
                throw new ArgumentNullException(nameof(logService)); 
        }

        [Command("price", RunMode = RunMode.Async)]
        public async Task GetPriceAsync()
        {
            var loadingImage = "loading.gif";
            Resources.CreateResource(Reflections.GetBasePath(), loadingImage);
            var loadingMessage = await Context.Channel.SendFileAsync(loadingImage).ConfigureAwait(false);

            var browserEngineType = _configurationService.ConfigFile.BrowserEngineType;
            var tokenChain = _configurationService.ConfigFile.TokenChain;
            var tokenAddress = _configurationService.ConfigFile.TokenAddress;
            var dexGuruApiKey = _configurationService.ConfigFile.DexGuruApiKey;
            var pageDelay = _configurationService.ConfigFile.PageDelay;

            var graphFileName = "";
            var dexGuruRestApi = new DexGuruRestApi(_logService, dexGuruApiKey);
            var poocoinWebDriver = new PoocoinWebDriver(browserEngineType);

            try
            {
                var graphFileNameTask = poocoinWebDriver.GetGraphFileName(tokenAddress, TimeSpan.FromSeconds(pageDelay));
                var getTokenTask = dexGuruRestApi.GetTokenAsync(tokenChain, tokenAddress);

                await Task.WhenAll(graphFileNameTask, getTokenTask).ConfigureAwait(false);

                graphFileName = graphFileNameTask.Result;
                var token = getTokenTask.Result;

                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"Token: {token.Inventory.Name} ({token.Inventory.Symbol})");
                stringBuilder.AppendLine($"Price: ${token.Finance.Price}");

                await Context.Channel.DeleteMessageAsync(loadingMessage).ConfigureAwait(false);
                await Context.Channel.SendFileAsync(graphFileName, stringBuilder.ToString()).ConfigureAwait(false);
            }
            finally
            {
                if (!string.IsNullOrEmpty(graphFileName))
                    File.Delete(graphFileName);

                poocoinWebDriver.Dispose();
            }
        }
    }
}
