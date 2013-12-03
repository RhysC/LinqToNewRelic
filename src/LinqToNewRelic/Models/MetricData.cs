using System;
using Newtonsoft.Json;

namespace LinqToNewRelic.Models
{
    public class MetricData
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("begin")]
        public DateTime Begin { get; set; }
        [JsonProperty("end")]
        public DateTime End { get; set; }
        [JsonProperty("app")]
        public string App { get; set; }
        [JsonProperty("agent_id")]
        public int AgentId { get; set; }
        [JsonProperty("call_count")]
        public float CallCount { get; set; }
        [JsonProperty("average_call_time")]
        public float AverageCallTime { get; set; }
        [JsonProperty("average_response_time")]
        public float AverageResponseTime { get; set; }
        [JsonProperty("max_call_time")]
        public float MaxCallTime { get; set; }
        [JsonProperty("min_call_time")]
        public float MinCallTime { get; set; }
        [JsonProperty("requests_per_minute")]
        public float RequestsPerMinute { get; set; }
        [JsonProperty("throughput")]
        public float Throughput { get; set; }
        [JsonProperty("total_call_time")]
        public float TotalCallTime { get; set; }
    }
}