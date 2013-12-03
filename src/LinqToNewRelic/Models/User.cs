using Newtonsoft.Json;

namespace LinqToNewRelic.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        [JsonProperty("send_weekly_report")]
        public bool SendWeeklyReport { get; set; }
        [JsonProperty("send_alert_email")]
        public bool SendAlertEmail { get; set; }
        [JsonProperty("alert_settings_url")]
        public string AlertSettingUrl { get; set; }
    }
}