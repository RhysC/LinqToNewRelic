using Newtonsoft.Json;

namespace LinqToNewRelic.Models
{
    public class Server
    {
        public int Id { get; set; }
        public string HostName { get; set; }
        [JsonProperty("overview_url")]
        public string OverviewUrl { get; set; }
    }
}