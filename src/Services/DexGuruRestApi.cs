using Newtonsoft.Json;
using RestSharp;
using StonkBot.Entities;
using System.Threading.Tasks;

namespace StonkBot.Services
{
    public class DexGuruRestApi
    {
        private RestClient _restClient;

        private const string BaseUrl = "https://api.dex.guru/v1/tokens/";

        public DexGuruRestApi()
        {
            _restClient = new RestClient();
        }

        public async Task<CoinData> GetCoinData(string coinAddress)
        {
            var url = $"{BaseUrl}{coinAddress}-bsc/";

            var request = new RestRequest(url, Method.GET, DataFormat.Json);
            var response = await _restClient.ExecuteAsync(request).ConfigureAwait(false);

            return JsonConvert.DeserializeObject<CoinData>(response.Content);
        }
    }
}
