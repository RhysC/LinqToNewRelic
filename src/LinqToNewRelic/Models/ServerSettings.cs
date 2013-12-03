using Newtonsoft.Json;

namespace LinqToNewRelic.Models
{
    public class ServerSettings
    {
        [JsonProperty("server_id")]
        public int ServerId { get; set; }
        [JsonProperty("hostname")]
        public string HostName { get; set; }
        [JsonProperty("alerts_enabled")]
        public bool IsAlertsEnabled { get; set; }
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        public string Url { get; set; }
    }
}
