using Newtonsoft.Json;

namespace LinqToNewRelic.Models
{
    public class Application
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("account_id")]
        public int AccountId { get; set; }
        [JsonProperty("hidden")]
        public bool Hidden { get; set; }
        [JsonProperty("has_rum_data")]
        public bool HasRumData { get; set; }
        [JsonProperty("health_status")]
        public int HealthStatus { get; set; }
        [JsonProperty("agent_summary")]
        public AgentSummary AgentSummary { get; set; }
    }
}