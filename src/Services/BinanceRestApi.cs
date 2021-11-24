using Discord;
using Newtonsoft.Json;
using RestSharp;
using StonkBot.Entities;
using System.Net;
using System.Threading.Tasks;

namespace StonkBot.Services
{
    internal class BinanceRestApi
    {
        private RestClient _restClient;

        private const string BaseUrl = "https://api.binance.com/api/v3";
        private readonly ILogService _logService;

        public BinanceRestApi(ILogService logService)
        {
            _restClient = new RestClient();
            _logService = logService;
        }

        public async Task<TickerPrice> GetTickerPriceAsync(string tickerSymbol)
        {
            var url = $"{BaseUrl}/ticker/price?symbol={tickerSymbol.ToUpper()}";
            var request = new RestRequest(url, Method.GET, DataFormat.Json);

            var response = await _restClient.ExecuteAsync(request).ConfigureAwait(false);
            CheckResponse(response);
            var tickerPrice = JsonConvert.DeserializeObject<TickerPrice>(response.Content);

            return tickerPrice;
        }

        private void CheckResponse(IRestResponse restResponse)
        {
            if (restResponse.StatusCode != HttpStatusCode.OK)
            {
                _logService.Log(LogSeverity.Debug, $"Code: {restResponse.StatusCode}");
                _logService.Log(LogSeverity.Debug, $"Code: {restResponse.Content}");
            }
        }
    }
}
