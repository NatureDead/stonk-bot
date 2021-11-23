using Newtonsoft.Json;

namespace StonkBot.Entities
{
    public class Chain
    {
        [JsonProperty("chain_id")]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
    }
}
