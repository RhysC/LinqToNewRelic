using Newtonsoft.Json;

namespace LinqToNewRelic.Models
{
    public class ApplicationInstance
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [JsonProperty("overview_url")]
        public string OverviewUrl { get; set; }
    }
}