using Newtonsoft.Json;

namespace EchoBot
{
    public class RecognizerResult
    {
        [JsonProperty("topScoringIntent")]
        public RecognizerIntent Intent { get; set; }

        [JsonProperty("entities")]
        public RecognizerEntity[] Entities { get; set; }
    }

    public class RecognizerIntent
    {
        [JsonProperty("intent")]
        public string Intent { get; set; }
    }

    public class RecognizerEntity
    {
        [JsonProperty("entity")]
        public string Entity { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("score")]
        public string Score { get; set; }
    }
}
