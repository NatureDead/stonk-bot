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
    public class CoinModule : ModuleBase<SocketCommandContext>
    {
        private readonly ConfigurationService _configurationService;

        public CoinModule(ConfigurationService configurationService)
        {
            _configurationService = configurationService ??
                throw new ArgumentNullException(nameof(configurationService));
        }

        [Command("price", RunMode = RunMode.Async)]
        public async Task GetPriceAsync()
        {
            var loadingImage = "loading.gif";
            Resources.CreateResource(Reflections.GetBasePath(), loadingImage);
            var userMessage = await Context.Channel.SendFileAsync(loadingImage).ConfigureAwait(false);

            var browserEngineType = _configurationService.ConfigFile.BrowserEngineType;
            var coinAddress = _configurationService.ConfigFile.CoinAddress;

            var graphFileName = "";
            var poocoinWebDriver = new PoocoinWebDriver(browserEngineType);
            var dexGuruRestApi = new DexGuruRestApi();

            try
            {
                var graphFileNameTask = poocoinWebDriver.GetGraphFileName(coinAddress, TimeSpan.FromSeconds(5));

                var coinDataTask = dexGuruRestApi.GetCoinData(coinAddress);

                await Task.WhenAll(graphFileNameTask, coinDataTask).ConfigureAwait(false);

                graphFileName = graphFileNameTask.Result;
                var coinData = coinDataTask.Result;

                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"Token: {coinData.Name} ({coinData.Symbol})");
                stringBuilder.AppendLine($"Price: ${coinData.Price}");

                await Context.Channel.DeleteMessageAsync(userMessage).ConfigureAwait(false);
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
