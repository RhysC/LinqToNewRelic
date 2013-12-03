using Newtonsoft.Json;

namespace LinqToNewRelic.Models
{
    public class AgentSummary
    {
        [JsonProperty("app_response_time")]
        public float AppResponseTime { get; set; }
        [JsonProperty("error_rate")]
        public float ErrorRate { get; set; }
    }
}