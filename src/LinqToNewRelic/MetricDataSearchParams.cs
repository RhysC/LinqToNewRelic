using System;

namespace LinqToNewRelic
{
    public class MetricDataSearchParams
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //eg []{"Custom/Info/Message_Loop_Mercury.Messages.Command_Delay"}
        public string[] Metrics { get; set; }
        public string MetricField { get; set; }
    }
}