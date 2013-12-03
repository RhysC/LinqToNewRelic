using Newtonsoft.Json;

namespace LinqToNewRelic.Models
{
    public class ServerThreshold
    {
        public int Id { get; set; }
        public string Type { get; set; }
        [JsonProperty("caution_value")]
        public float CautionValue { get; set; }
        [JsonProperty("critical_value")]
        public float CriticalValue { get; set; }
        public string Url { get; set; }
    }
}