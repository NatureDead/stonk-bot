using Discord;
using Newtonsoft.Json;
using RestSharp;
using StonkBot.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace StonkBot.Services
{
    public class DexGuruRestApi
    {
        private RestClient _restClient;

        private const string BaseUrl = "https://api.dev.dex.guru/v1";
        private readonly ILogService _logService;
        private readonly string _apiKey;

        public DexGuruRestApi(ILogService logService, string apiKey)
        {
            _restClient = new RestClient();
            _logService = logService;
            _apiKey = apiKey;
        }

        public async Task<Token> GetTokenAsync(string chainName, string tokenAddress)
        {
            var chain = await GetChainAsync(chainName).ConfigureAwait(false);
            var tokenFinance = await GetTokenFinanceAsync(chain, tokenAddress).ConfigureAwait(false);
            var tokenInventory = await GetTokenInventoryAsync(chain, tokenAddress).ConfigureAwait(false);

            return new Token(tokenFinance, tokenInventory);
        }

        private async Task<Chain> GetChainAsync(string chainName)
        {
            var url = $"{BaseUrl}/chain/";
            var request = new RestRequest(url, Method.GET, DataFormat.Json);
            AddApiKey(request);

            var response = await _restClient.ExecuteAsync(request).ConfigureAwait(false);
            CheckResponse(response);
            var chains = JsonConvert.DeserializeObject<JsonList<Chain>>(response.Content);
            var chain = chains.Items.First(chain => string.Equals(chain.Name, chainName, System.StringComparison.InvariantCultureIgnoreCase));

            return chain;
        }

        private async Task<TokenFinance> GetTokenFinanceAsync(Chain chain, string tokenAddress)
        {
            var url = $"{BaseUrl}/chain/{chain.Id}/tokens/{tokenAddress}/market/";
            var request = new RestRequest(url, Method.GET, DataFormat.Json);
            AddApiKey(request);

            var response = await _restClient.ExecuteAsync(request).ConfigureAwait(false);
            CheckResponse(response);
            var tokenFinance = JsonConvert.DeserializeObject<TokenFinance>(response.Content);

            return tokenFinance;
        }

        private async Task<TokenInventory> GetTokenInventoryAsync(Chain chain, string tokenAddress)
        {
            var url = $"{BaseUrl}/chain/{chain.Id}/tokens/{tokenAddress}/";
            var request = new RestRequest(url, Method.GET, DataFormat.Json);
            AddApiKey(request);

            var response = await _restClient.ExecuteAsync(request).ConfigureAwait(false);
            CheckResponse(response);
            var tokenInventory = JsonConvert.DeserializeObject<TokenInventory>(response.Content);

            return tokenInventory;
        }

        private void AddApiKey(IRestRequest restRequest)
        {
            restRequest.AddHeader("api-key", _apiKey);
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
