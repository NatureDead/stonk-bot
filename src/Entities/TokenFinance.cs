using Newtonsoft.Json;

namespace StonkBot.Entities
{
    public class TokenFinance
    {
        public string Address { get; set; }

        [JsonProperty("price_usd")]
        public decimal Price { get; set; }
    }
}
