﻿using System;
using System.Collections;
using System.Collections.Specialized;
using System.Net;
using LinqToNewRelic.Models;
using Newtonsoft.Json;

namespace LinqToNewRelic
{
    //NOTE : Only supporting query operation at the moment. Newrelic does expose non query methods too - see the docs
    public class Api
    {
        private readonly string _apiKey;

        public Api(string apiKey)
        {
            _apiKey = apiKey;
        }

        public Application[] GetApplications(string accountId)
        {
            return GetDeserializedResult<Application[]>(GetNewRelicUrl(accountId, "applications.json"));
        }

        public Server[] GetApplicationServers(string accountId, int applicationId)
        {
            var serverPath = string.Format("applications/{0}/servers.json", applicationId);
            return GetDeserializedResult<Server[]>(GetNewRelicUrl(accountId, serverPath));
        }
        public ServerSettings[] GetServerSettings(string accountId)
        {
            return GetDeserializedResult<ServerSettings[]>(GetNewRelicUrl(accountId, "server_settings.json"));
        }
        public ServerSettings GetServerSettings(string accountId, int serverId)
        {
            var settingsPath = string.Format("server_settings/{0}.json", serverId);
            return GetDeserializedResult<ServerSettings>(GetNewRelicUrl(accountId, settingsPath));
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

        public ServerThreshold[] GetServerAlertThresholds(string accountId, int serverId)
        {
            var settingsPath = string.Format("servers/{0}/thresholds.json", serverId);
            return GetDeserializedResult<ServerThreshold[]>(GetNewRelicUrl(accountId, settingsPath));
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

        private static string GetNewRelicUrl(string accountId, string extendedPath)
        {
            return string.Format("https://api.newrelic.com/api/v1/accounts/{0}/{1}", accountId, extendedPath);
        }

        private T GetDeserializedResult<T>(string url)
        {
            var webClient = new WebClient();
            webClient.Headers.Add("x-api-key", _apiKey);
            var json = webClient.DownloadString(url);
            return JsonConvert.DeserializeObject<T>(json);
        }

        private static string GetUrl(string baseUrl, NameValueCollection parameters)
        {
            var urlbuilder = new UriBuilder(baseUrl) { Port = -1, Query = parameters.ToQueryString(true) };
            return urlbuilder.ToString();
        }

        private static NameValueCollection ToNamedValueCollection(MetricDataSearchParams searchParams)
        {
            //http://newrelic.github.io/newrelic_api/README_rdoc.html#label-Metric+Data
            var parameters = new NameValueCollection();

            parameters["begin"] = searchParams.StartDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            // Begin time, in XML UTC format For example: 2011-04-20T15:47:00Z
            parameters["end"] = searchParams.EndDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            //End time, in XML UTC format. For example: 2011-04-20T15:52:00Z
            foreach (var metric in searchParams.Metrics)
            {
                //Included one or many times, this lists the metrics you are interested in. You can only specify a specific metric once. If you specify multiple metrics, the request parameter should look like metrics[]=foo&metrics[]=bar (append '[]' to the end of the name of the parameter). You can specify metrics[] even if there is a single metric.
                parameters["metrics[]"] = metric;
            }
            parameters["field"] = searchParams.MetricField;
            //Each metric supports different fields, such as 'average_response_time'. If the field is not valid for a given metric, you will get a blank value, and if there are no valid fields for the metrics you request, you will get an error.
            parameters["summary"] = "0";
            //1 or 0, defaults to 0. This determines whether you get back a single value aggregated over the entire time period, or a time series. Summary results do not include the begin and end times in the result. Time s
            return parameters;
        }
    }
}