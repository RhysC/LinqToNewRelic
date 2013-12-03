using System.Xml.Serialization;

namespace LinqToNewRelic.Models
{
    [XmlType(AnonymousType = true)]
    public class ThresholdValue
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("begin_time")]
        public string BeginTime { get; set; }

        [XmlAttribute("end_time")]
        public string EndTime { get; set; }

        [XmlAttribute("formatted_metric_value")]
        public string FormattedMetricValue { get; set; }

        [XmlAttribute("threshold_value")]
        public byte Value { get; set; }

        [XmlAttribute("metric_value")]
        public decimal MetricValue { get; set; }
    }
}