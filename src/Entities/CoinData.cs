using Newtonsoft.Json;

namespace StonkBot.Entities
{
    public class CoinData
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        [JsonProperty("priceUSD")]
        public decimal Price { get; set; }
    }
}
