using Newtonsoft.Json;

namespace LinqToNewRelic.Models
{
    public class ApplicationSetting
    {
        [JsonProperty("application_id")]
        public int ApplicationId { get; set; }
        public string Name { get; set; }
        [JsonProperty("alerts_enabled")]
        public bool AlertsEnabled { get; set; }
        [JsonProperty("app-apdex-t")]
        public float AppApdexT { get; set; }
        [JsonProperty("rum_apdex_t")]
        public float RumApdexT { get; set; }
        [JsonProperty("rum_enabled")]
        public bool Rumenabled { get; set; }
        //Only comes back on get all
        public string Url { get; set; }
    }
}