using LinqToNewRelic.Models;

namespace LinqToNewRelic
{
    public interface IApplicationQueries
    {
        Application[] GetApplications(string accountId);
        ApplicationInstance[] GetApplicationInstances(string accountId, int applicationId);
        Server[] GetApplicationServers(string accountId, int applicationId);
        ApplicationSetting[] GetApplicationSettings(string accountId);
        ApplicationSetting GetApplicationSettings(string accountId, int applicationId);
        ApplicationThreshold[] GetApplicationAlertThresholds(string accountId, int applicationId);
        ApplicationMetricsDefinition[] ApplicationMetricDefinitions(string accountId, int applicationId);
        MetricData[] ApplicationMetricData(string accountId, int applicationId, MetricDataSearchParams searchParams);
        ThresholdValues ApplicationSummaryMetricData(string accountId, int applicationId);
    }
}