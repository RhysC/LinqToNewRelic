using Newtonsoft.Json;

namespace LinqToNewRelic.Models
{
    public class AlertSetting
    {
        [JsonProperty("application_id")]
        public int ApplicationId { get; set; }
        [JsonProperty("application_name")]
        public string ApplicationName { get; set; }
        [JsonProperty("email_alerts")]
        public string EmailAlerts { get; set; }
        public string Url { get; set; }
    }
}