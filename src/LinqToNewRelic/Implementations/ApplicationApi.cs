using LinqToNewRelic.Models;

namespace LinqToNewRelic.Implementations
{
    class ApplicationApi : ApiBase, IApplicationQueries
    {
        public ApplicationApi(string apiKey)
            : base(apiKey) { }

        public Application[] GetApplications(string accountId)
        {
            return GetDeserializedResult<Application[]>(GetNewRelicUrl(accountId, "applications.json"));
        }
        public ApplicationInstance[] GetApplicationInstances(string accountId, int applicationId)
        {
            var instancesPath = string.Format("applications/{0}/instances.json", applicationId);
            return GetDeserializedResult<ApplicationInstance[]>(GetNewRelicUrl(accountId, instancesPath));
        }

        public Server[] GetApplicationServers(string accountId, int applicationId)
        {
            var serverPath = string.Format("applications/{0}/servers.json", applicationId);
            return GetDeserializedResult<Server[]>(GetNewRelicUrl(accountId, serverPath));
        }

        public ApplicationSetting[] GetApplicationSettings(string accountId)
        {
            return GetDeserializedResult<ApplicationSetting[]>(GetNewRelicUrl(accountId, "application_settings.json"));
        }

        public ApplicationSetting GetApplicationSettings(string accountId, int applicationId)
        {
            var settingsPath = string.Format("application_settings/{0}.json", applicationId);
            return GetDeserializedResult<ApplicationSetting>(GetNewRelicUrl(accountId, settingsPath));
        }

        public ApplicationThreshold[] GetApplicationAlertThresholds(string accountId, int applicationId)
        {
            var settingsPath = string.Format("applications/{0}/thresholds.json", applicationId);
            return GetDeserializedResult<ApplicationThreshold[]>(GetNewRelicUrl(accountId, settingsPath));
        }


        public ApplicationMetricsDefinition[] ApplicationMetricDefinitions(string accountId, int applicationId)
        {
            var metricPath = string.Format("agents/{0}/metrics.json", applicationId);
            return GetDeserializedResult<ApplicationMetricsDefinition[]>(GetNewRelicUrl(accountId, metricPath));
        }

        public MetricData[] ApplicationMetricData(string accountId, int applicationId,
                                                  MetricDataSearchParams searchParams)
        {
            var metricDataPath = string.Format("/agents/{0}/data.json", applicationId);
            var baseUrl = GetNewRelicUrl(accountId, metricDataPath);
            var nvc = ToNamedValueCollection(searchParams);
            var url = GetUrl(baseUrl, nvc);
            return GetDeserializedResult<MetricData[]>(url);
        }

        public ThresholdValues ApplicationSummaryMetricData(string accountId, int applicationId)
        {
            var summarydataPath = string.Format("applications/{0}/threshold_values.json", applicationId);
            return GetDeserializedXmlResult<ThresholdValues>(GetNewRelicUrl(accountId, summarydataPath));
        }
    }
}