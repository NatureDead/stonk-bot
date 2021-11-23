using Newtonsoft.Json;

namespace StonkBot.Entities
{
    public class JsonList<T>
    {
        [JsonProperty("data")]
        public T[] Items { get; set; }
    }
}
