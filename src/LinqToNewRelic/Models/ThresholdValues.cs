using System.Xml.Serialization;

namespace LinqToNewRelic.Models
{
    [XmlRoot("threshold-values")]
    public class ThresholdValues
    {
        [XmlElement("threshold_value")]
        public ThresholdValue[] Values { get; set; }
    }
}