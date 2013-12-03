﻿using Newtonsoft.Json;

namespace LinqToNewRelic.Models
{
    public class ApplicationSetting
    {
        [JsonProperty("application_id")]
        public int ApplicationId { get; set; }
        public string Name { get; set; }
        [JsonProperty("alerts_enabled")]
        public bool AlertsEnabled { get; set; }
        [JsonProperty("rum_apdex_t")]
        public float RumApdexT { get; set; }
        [JsonProperty("rum_enabled")]
        public bool Rumenabled { get; set; }
        public string Url { get; set; }
    }
}